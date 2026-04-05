using CoffeeManagementSystem.Config;
using Microsoft.Web.WebView2.Core;
using System;
using System.Net;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class FormThanhToan : Form
    {
        // Số tiền cần thanh toán (VNPay yêu cầu kiểu số, phía ngoài PaymentForm truyền vào)
        private readonly double _payAmount;

        // Mô tả nội dung đơn hàng (gửi sang VNPay để hiển thị cho khách)
        private readonly string _description;

        /// <summary>
        /// Constructor:
        /// - Được gọi từ PaymentForm khi người dùng chọn thanh toán bằng VNPay.
        /// - Nhận vào:
        ///   + amount: tổng số tiền cần thanh toán
        ///   + description: mô tả hóa đơn (mã hóa đơn, tên KH, ...)
        /// </summary>
        public FormThanhToan(double amount, string description)
        {
            InitializeComponent();
            _payAmount = amount;
            _description = description;
        }

        /// <summary>
        /// Khởi tạo WebView2 và điều hướng đến trang thanh toán VNPay.
        /// - Được gọi trong sự kiện FormThanhToan_Load_1 (khi form được load).
        /// - Bước xử lý:
        ///   1) Đảm bảo WebView2 đã khởi tạo CoreWebView2.
        ///   2) Gắn event NavigationStarting để bắt URL trả về từ VNPay.
        ///   3) Build URL thanh toán VNPay và gọi Navigate().
        /// </summary>
        private async System.Threading.Tasks.Task InitializeWebView()
        {
            try
            {
                // Nếu WebView2 chưa có engine CoreWebView2 thì khởi tạo
                if (webView21.CoreWebView2 == null)
                    await webView21.EnsureCoreWebView2Async(null);

                // Đảm bảo không gắn trùng event
                webView21.CoreWebView2.NavigationStarting -= CoreWebView2_NavigationStarting;
                webView21.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;

                // Tạo URL thanh toán VNPay từ số tiền + mô tả
                string paymentUrl = BuildVnPayUrl(_payAmount, _description);

                // Điều hướng WebView2 tới cổng thanh toán VNPay
                webView21.CoreWebView2.Navigate(paymentUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo VNPay: " + ex.Message);
            }
        }

        /// <summary>
        /// Xây dựng URL thanh toán VNPay đầy đủ tham số và chữ ký.
        /// - Được gọi bên trong InitializeWebView().
        /// - Sử dụng VnPayLibrary + VNPayConfig:
        ///   + Chèn các tham số vnp_* theo spec VNPay.
        ///   + Tạo secure hash (vnp_SecureHash) bằng HMAC SHA512.
        /// </summary>
        private string BuildVnPayUrl(double amount, string description)
        {
            var vnpay = new VnPayLibrary();

            // Version và command theo tài liệu VNPay
            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");

            // Mã terminal (website) lấy từ file config
            vnpay.AddRequestData("vnp_TmnCode", VNPayConfig.Vnp_TmnCode);

            // VNPay quy định vnp_Amount = số tiền * 100 (không dùng dấu thập phân)
            long vnpAmount = (long)(amount * 100);
            vnpay.AddRequestData("vnp_Amount", vnpAmount.ToString());

            // Thời điểm tạo giao dịch, tiền tệ, IP, ngôn ngữ, mô tả đơn hàng
            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1"); // demo: IP localhost
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", description);
            vnpay.AddRequestData("vnp_OrderType", "other");

            // URL để VNPay redirect kết quả về ứng dụng
            vnpay.AddRequestData("vnp_ReturnUrl", VNPayConfig.Vnp_ReturnUrl);

            // Mã tham chiếu giao dịch (duy nhất), ở đây dùng ticks thời gian
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            // Tạo URL đầy đủ (baseUrl + query + vnp_SecureHash)
            return vnpay.CreateRequestUrl(VNPayConfig.Vnp_Url, VNPayConfig.Vnp_HashSecret);
        }

        /// <summary>
        /// Bắt sự kiện khi WebView2 chuẩn bị điều hướng tới một URL mới.
        /// - Được gọi tự động bởi WebView2 mỗi lần chuyển trang.
        /// - Nghiệp vụ:
        ///   + Nếu URL bắt đầu bằng Vnp_ReturnUrl → đây là URL VNPay trả về.
        ///   + Parse query string lấy vnp_ResponseCode, vnp_TransactionNo.
        ///   + code == "00" → thanh toán thành công → DialogResult.OK.
        ///   + code != "00" → thanh toán thất bại → DialogResult.Cancel.
        /// </summary>
        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            // Kiểm tra xem URL hiện tại có phải là URL callback (ReturnUrl) không
            if (e.Uri.StartsWith(VNPayConfig.Vnp_ReturnUrl))
            {
                // Không cho WebView2 load trang returnUrl này, chỉ dùng để đọc dữ liệu
                e.Cancel = true;

                try
                {
                    var uri = new Uri(e.Uri);

                    // Lấy query string và tách thành các cặp key=value
                    string qs = uri.Query.TrimStart('?');
                    string[] parts = qs.Split('&');

                    string code = "";
                    string transNo = "";

                    foreach (string p in parts)
                    {
                        string[] kv = p.Split('=');
                        if (kv.Length == 2)
                        {
                            string key = kv[0];
                            string val = WebUtility.UrlDecode(kv[1]);

                            if (key == "vnp_ResponseCode") code = val;
                            if (key == "vnp_TransactionNo") transNo = val;
                        }
                    }

                    // Theo tài liệu VNPay: vnp_ResponseCode = "00" nghĩa là giao dịch thành công
                    if (code == "00")
                    {
                        // Có thể show message nếu muốn, hiện tại chỉ trả kết quả về PaymentForm
                        // MessageBox.Show("Thanh toán thành công!\nMã giao dịch: " + transNo);

                        // Báo cho PaymentForm biết: VNPay thanh toán OK
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        // Các mã khác "00" được xem là thất bại hoặc bị hủy
                        MessageBox.Show("Thanh toán thất bại! Mã lỗi: " + code);
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xử lý trả về: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Nút đóng form:
        /// - Nếu người dùng chủ động đóng form thanh toán VNPay,
        ///   coi như hủy/thất bại → trả DialogResult.Cancel cho PaymentForm.
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Sự kiện Load của FormThanhToan:
        /// - Được gắn trong Designer: this.Load += FormThanhToan_Load_1;
        /// - Khi form vừa mở, gọi InitializeWebView() để:
        ///   + Khởi tạo WebView2
        ///   + Gọi VNPay và hiển thị màn hình thanh toán.
        /// </summary>
        private async void FormThanhToan_Load_1(object sender, EventArgs e)
        {
            await InitializeWebView();
        }
    }
}
