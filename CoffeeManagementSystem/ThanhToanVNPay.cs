using CoffeeManagementSystem.Config;
using Microsoft.Web.WebView2.Core;
using System;
using System.Net;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class FormThanhToan : Form
    {
        private readonly double _payAmount;
        private readonly string _description;

        public FormThanhToan(double amount, string description)
        {
            InitializeComponent();
            _payAmount = amount;
            _description = description;
        }

        private async void FormThanhToan_Load(object sender, EventArgs e)
        {
            await InitializeWebView();
        }

        private async System.Threading.Tasks.Task InitializeWebView()
        {
            try
            {
                if (webView21.CoreWebView2 == null)
                    await webView21.EnsureCoreWebView2Async(null);

                webView21.CoreWebView2.NavigationStarting -= CoreWebView2_NavigationStarting;
                webView21.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;

                string paymentUrl = BuildVnPayUrl(_payAmount, _description);
                webView21.CoreWebView2.Navigate(paymentUrl);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi tạo VNPay: " + ex.Message);
            }
        }

        private string BuildVnPayUrl(double amount, string description)
        {
            var vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", "2.1.0");
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", VNPayConfig.Vnp_TmnCode);

            long vnpAmount = (long)(amount * 100);
            vnpay.AddRequestData("vnp_Amount", vnpAmount.ToString());

            vnpay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_IpAddr", "127.0.0.1");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", description);
            vnpay.AddRequestData("vnp_OrderType", "other");
            vnpay.AddRequestData("vnp_ReturnUrl", VNPayConfig.Vnp_ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString());

            return vnpay.CreateRequestUrl(VNPayConfig.Vnp_Url, VNPayConfig.Vnp_HashSecret);
        }

        private void CoreWebView2_NavigationStarting(object sender, CoreWebView2NavigationStartingEventArgs e)
        {
            if (e.Uri.StartsWith(VNPayConfig.Vnp_ReturnUrl))
            {
                e.Cancel = true;

                try
                {
                    var uri = new Uri(e.Uri);

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

                    if (code == "00")
                    {
                        MessageBox.Show("Thanh toán thành công!\nMã giao dịch: " + transNo);
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private async void FormThanhToan_Load_1(object sender, EventArgs e)
        {
            await InitializeWebView();
        }
    }
}
