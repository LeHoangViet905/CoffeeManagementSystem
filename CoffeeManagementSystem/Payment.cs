    using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing; // Dùng để in hóa đơn
using System.Linq;
using System.Media;
using System.Speech.Synthesis; // Dùng để đọc giọng nói khi thanh toán xong
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class PaymentForm : Form
    {
        // Lớp nghiệp vụ xử lý thanh toán (tính tiền, lưu hóa đơn, khách hàng, ...)
        private PaymentBLL _paymentBLL;

        // Lưu khách hàng hiện đang được chọn/xử lý
        private Khachhang currentSelectedCustomer;

        // Đối tượng phục vụ việc in hóa đơn
        private PrintDocument printDocumentInvoice;
        private PrintPreviewDialog printPreviewDialogInvoice;

        // Hằng số tên hình thức thanh toán
        private const string HINH_THUC_TIEN_MAT = "Tiền mặt";
        private const string HINH_THUC_CHUYEN_KHOAN = "Chuyển khoản";

        // Lưu hình thức thanh toán đang chờ xác nhận (tiền mặt / chuyển khoản)
        private string _pendingPaymentMethod = null;
        private bool _isGuestForCurrentPhone = false;

        /// <summary>
        /// Constructor PaymentForm:
        /// - Được gọi từ OrderForm/MainForm khi bấm nút "Thanh toán"
        /// - Nhận:
        ///   + dsChiTiet: danh sách chi tiết đồ uống trong đơn
        ///   + manhanvien, tenNhanVien: thông tin nhân viên thu ngân
        ///   + maDonHang: mã đơn hàng hiện tại
        /// - Khởi tạo BLL, in ấn, gắn event Load + Click cho nút Thanh toán.
        /// </summary>
        public PaymentForm(List<Chitietdonhang> dsChiTiet, string manhanvien, string tenNhanVien, string maDonHang)
        {
            InitializeComponent();
            // Khởi tạo lớp nghiệp vụ thanh toán với dữ liệu đơn hàng
            _paymentBLL = new PaymentBLL(dsChiTiet, manhanvien, tenNhanVien, maDonHang);

            this.Text = "Payment";

            // Khi form load xong sẽ gọi PaymentForm_Load
            this.Load += PaymentForm_Load;
            // Nút Thanh toán trên form sẽ gọi btnThanhToan_Click
            this.btnThanhToan.Click += btnThanhToan_Click;

            // Khởi tạo đối tượng in hóa đơn
            printDocumentInvoice = new PrintDocument();
            printDocumentInvoice.PrintPage += new PrintPageEventHandler(this.printDocumentInvoice_PrintPage);
            printPreviewDialogInvoice = new PrintPreviewDialog();
            printPreviewDialogInvoice.Document = printDocumentInvoice;

            Logger.LogInfo("PaymentForm đã được khởi tạo.");
        }

        /// <summary>
        /// Form Load:
        /// - Lấy thông tin hóa đơn từ BLL để hiển thị.
        /// - Đổ chi tiết đơn hàng lên ListView.
        /// - Tính tổng tiền.
        /// - Thiết lập trạng thái mặc định: chọn Tiền mặt, ẩn các nút kết quả.
        /// </summary>
        private void PaymentForm_Load(object sender, EventArgs e)
        {
            lblMaHoaDonValue.Text = _paymentBLL.GetMaHoaDonHienTai();
            lblNguoiLapValue.Text = _paymentBLL.GetTenNhanVienLapHoaDon();
            lblNgayValue.Text = _paymentBLL.GetNgayLapHoaDon().ToShortDateString();

            SetupListViewColumns();
            LoadChiTietHoaDon();
            TinhTongTien();

            // Mặc định chọn thanh toán bằng tiền mặt
            if (rdbTienMat != null)
                rdbTienMat.Checked = true;

            // Ẩn hai nút "Thanh toán thành công" / "Thanh toán thất bại" (dùng cho flow khác)
            if (btnThanhToanThanhCong != null)
                btnThanhToanThanhCong.Visible = false;
            if (btnThanhToanThatBai != null)
                btnThanhToanThatBai.Visible = false;

            _pendingPaymentMethod = null;

            Logger.LogInfo("PaymentForm đã tải xong dữ liệu và hiển thị ban đầu.");
        }

        /// <summary>
        /// Thiết lập các cột cho ListView hiển thị chi tiết đơn hàng.
        /// </summary>
        private void SetupListViewColumns()
        {
            if (lvwChiTietHoaDon != null)
            {
                lvwChiTietHoaDon.View = View.Details;
                lvwChiTietHoaDon.GridLines = true;
                lvwChiTietHoaDon.FullRowSelect = true;

                lvwChiTietHoaDon.Columns.Clear();

                lvwChiTietHoaDon.Columns.Add("STT", 50, HorizontalAlignment.Center);
                lvwChiTietHoaDon.Columns.Add("Tên đồ uống", 200, HorizontalAlignment.Left);
                lvwChiTietHoaDon.Columns.Add("Số lượng", 80, HorizontalAlignment.Center);
                lvwChiTietHoaDon.Columns.Add("Đơn giá", 100, HorizontalAlignment.Right);
                lvwChiTietHoaDon.Columns.Add("Thành tiền", 120, HorizontalAlignment.Right);

                Logger.LogDebug("ListView columns for ChiTietHoaDon have been set up.");
            }
        }

        /// <summary>
        /// Tải dữ liệu chi tiết đơn hàng (từ BLL) vào ListView.
        /// </summary>
        private void LoadChiTietHoaDon()
        {
            if (lvwChiTietHoaDon != null)
            {
                lvwChiTietHoaDon.Items.Clear();
                List<Chitietdonhang> dsChiTiet = _paymentBLL.GetDsChiTietHoaDon();

                for (int i = 0; i < dsChiTiet.Count; i++)
                {
                    Chitietdonhang chiTiet = dsChiTiet[i];

                    ListViewItem lvi = new ListViewItem((i + 1).ToString());
                    lvi.SubItems.Add(chiTiet.Tendouong);
                    lvi.SubItems.Add(chiTiet.Soluong.ToString());
                    lvi.SubItems.Add(chiTiet.Dongia.ToString("N0"));
                    lvi.SubItems.Add(chiTiet.Thanhtien.ToString("N0"));

                    lvwChiTietHoaDon.Items.Add(lvi);
                }

                Logger.LogDebug($"Đã tải {dsChiTiet.Count} chi tiết đơn hàng vào ListView.");
            }
        }

        /// <summary>
        /// Tính toán và hiển thị tổng thành tiền lên textbox.
        /// </summary>
        private void TinhTongTien()
        {
            decimal tongTien = _paymentBLL.CalculateTongTien();
            txtTongThanhTienValue.Text = tongTien.ToString("N0");

            Logger.LogDebug($"Tổng tiền hiển thị trên UI: {tongTien:N0}");
        }

        /// <summary>
        /// Hàm chung thực hiện thanh toán (cả tiền mặt lẫn chuyển khoản).
        /// - Được gọi từ:
        ///   + btnThanhToan_Click (sau khi VNPay OK hoặc tiền mặt đủ tiền)
        ///   + btnThanhToanThanhCong_Click (khi dùng flow xác nhận thủ công)
        /// - Nhiệm vụ:
        ///   + Kiểm tra tên khách hàng
        ///   + Gọi PaymentBLL.ProcessPayment để lưu hóa đơn
        ///   + Phát âm thanh + đọc giọng nói
        ///   + Hỏi in hóa đơn
        ///   + Đóng form với DialogResult.OK nếu thành công
        /// </summary>
        private void XuLyThanhToan(string hinhThucThanhToan)
        {
            // Lấy SĐT từ textbox
            string phoneFromTextbox = txtKhachHangName.Text.Trim();

            // Nếu là khách vãng lai hoặc không có SĐT → không gửi SĐT xuống BLL
            string soDienThoaiDeTruyen =
                (!_isGuestForCurrentPhone && !string.IsNullOrEmpty(phoneFromTextbox))
                ? phoneFromTextbox
                : null;

            Logger.LogInfo($"Bắt đầu xử lý thanh toán. Hình thức: {hinhThucThanhToan}, SĐT gửi xuống BLL: {soDienThoaiDeTruyen ?? "Khách vãng lai"}");

            try
            {
                Khachhang customerFromBLL;
                string manhanvienThuNgan = _paymentBLL.GetManhanvienLapHoaDon();
                string ghiChu = "";

                bool success = _paymentBLL.ProcessPayment(
                    soDienThoaiDeTruyen,  // giờ SĐT có thể là null nếu khách vãng lai
                    hinhThucThanhToan,
                    manhanvienThuNgan,
                    ghiChu,
                    out customerFromBLL
                );

                // Cập nhật khách hàng hiện tại (nếu có)
                currentSelectedCustomer = customerFromBLL;

                if (success)
                {
                    MessageBox.Show("Đơn hàng đã được thanh toán và lưu thành công!",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    try
                    {
                        decimal tongTien = _paymentBLL.CalculateTongTien();

                        // PHÁT ÂM THANH WAV KHÔNG CHẶN UI
                        try
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    using (SoundPlayer player = new SoundPlayer(Properties.Resources.Payment))
                                    {
                                        player.Play(); // Play() non-blocking
                                    }
                                }
                                catch (Exception soundEx)
                                {
                                    Logger.LogError("Không phát được âm thanh WAV: " + soundEx.Message);
                                }
                            });
                        }
                        catch (Exception taskEx)
                        {
                            Logger.LogError("Lỗi khi khởi chạy Task âm thanh: " + taskEx.Message);
                        }

                        // PHÁT ÂM THANH "THANH TOÁN THÀNH CÔNG" KHÔNG CHẶN UI
                        try
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    Thread.Sleep(400);
                                    using (SoundPlayer playerVoice = new SoundPlayer(Properties.Resources.ThanhToanThanhCong))
                                    {
                                        playerVoice.PlaySync();
                                    }
                                }
                                catch (Exception voiceEx)
                                {
                                    Logger.LogError("Không phát được âm thanh ThanhToanThanhCong: " + voiceEx.Message);
                                }
                            });
                        }
                        catch (Exception taskVoiceEx)
                        {
                            Logger.LogError("Lỗi khi khởi chạy Task giọng đọc ThanhToanThanhCong: " + taskVoiceEx.Message);
                        }

                        Logger.LogInfo($"Thanh toán hoàn tất thành công cho hóa đơn: {lblMaHoaDonValue.Text}");
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError("Lỗi tổng thể khi phát âm thanh + đọc giọng: " + ex.Message, ex);
                    }

                    // HỎI CÓ IN HÓA ĐƠN KHÔNG
                    DialogResult printConfirm = MessageBox.Show("Bạn có muốn in hóa đơn này không?",
                                                                "In Hóa Đơn",
                                                                MessageBoxButtons.YesNo,
                                                                MessageBoxIcon.Question);
                    if (printConfirm == DialogResult.Yes)
                    {
                        printPreviewDialogInvoice.ShowDialog();
                    }

                    // Đóng PaymentForm, báo cho form gọi là thanh toán OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.LogError($"Thanh toán thất bại (lỗi nghiệp vụ): {ex.Message}", ex);
            }
            // ⚠️  ĐÃ BỎ catch KhachhangNotFoundException CŨ ở đây
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.LogError($"Thanh toán thất bại (tham số không hợp lệ): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thanh toán đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError($"Lỗi hệ thống không xác định khi thanh toán đơn hàng. SĐT: '{phoneFromTextbox}'", ex);
            }
        }


        /// <summary>
        /// Popup thu tiền mặt:
        /// - Hiển thị tổng tiền, cho nhập "Khách đưa"
        /// - Tính khách thừa / thiếu
        /// - Trả về true nếu khách đưa đủ tiền và bấm "Thành công"
        /// - Trả về false nếu hủy / thiếu tiền
        /// - Được gọi trong btnThanhToan_Click khi chọn tiền mặt.
        /// </summary>
        private bool ShowCashPaymentDialog()
        {
            decimal tongTien = _paymentBLL.CalculateTongTien();

            Form cashForm = new Form
            {
                Text = "Thanh toán tiền mặt",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                ClientSize = new Size(420, 190),
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false
            };

            // Label tổng cần thu
            Label lblTong = new Label
            {
                AutoSize = true,
                Text = "Tổng cần thu: " + tongTien.ToString("N0") + " VND",
                Location = new Point(20, 20)
            };

            // Label "Khách đưa"
            Label lblKhachDua = new Label
            {
                AutoSize = true,
                Text = "Khách đưa:",
                Location = new Point(20, 55)
            };

            // Ô nhập tiền khách đưa
            TextBox txtKhachDua = new TextBox
            {
                Location = new Point(110, 52),
                Width = 150
            };

            // Label trạng thái: khách thừa / thiếu
            Label lblTrangThai = new Label
            {
                AutoSize = true,
                Text = "Khách thừa: 0 VND",
                Location = new Point(20, 90),
                ForeColor = Color.Green
            };

            // Cập nhật trạng thái khi người dùng gõ số tiền
            txtKhachDua.TextChanged += (s, e) =>
            {
                decimal khachDua;
                if (decimal.TryParse(txtKhachDua.Text.Replace(".", "").Replace(",", ""), out khachDua))
                {
                    decimal chenhLech = khachDua - tongTien;
                    if (chenhLech >= 0)
                    {
                        lblTrangThai.Text = "Khách thừa: " + chenhLech.ToString("N0") + " VND";
                        lblTrangThai.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblTrangThai.Text = "Khách thiếu: " + Math.Abs(chenhLech).ToString("N0") + " VND";
                        lblTrangThai.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblTrangThai.Text = "Khách thừa: 0 VND";
                    lblTrangThai.ForeColor = Color.Green;
                }
            };

            bool result = false;   // kết quả trả về

            // Nút THÀNH CÔNG (khách đưa đủ tiền)
            Button btnThanhCong = new Button
            {
                Text = "Thành công",
                Size = new Size(110, 30),
                Location = new Point(220, 130)
            };

            btnThanhCong.Click += (s, e) =>
            {
                decimal khachDua;
                if (!decimal.TryParse(txtKhachDua.Text.Replace(".", "").Replace(",", ""), out khachDua))
                {
                    MessageBox.Show("Vui lòng nhập số tiền hợp lệ.", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtKhachDua.Focus();
                    return;
                }

                if (khachDua < tongTien)
                {
                    // Thiếu tiền thì không cho thanh toán
                    lblTrangThai.Text = "Khách thiếu: " +
                                        (tongTien - khachDua).ToString("N0") + " VND";
                    lblTrangThai.ForeColor = Color.Red;
                    return;
                }

                Logger.LogInfo(
                    $"Tiền mặt: khách đưa {khachDua:N0}, tổng {tongTien:N0}, thối lại {(khachDua - tongTien):N0}.");

                result = true;   // cho phép thanh toán
                cashForm.Close();
            };

            // Nút THẤT BẠI (không nhận được tiền)
            Button btnThatBai = new Button
            {
                Text = "Thất bại",
                Size = new Size(110, 30),
                Location = new Point(90, 130)
            };

            btnThatBai.Click += (s, e) =>
            {
                Logger.LogInfo("Thu ngân chọn THANH TOÁN TIỀN MẶT THẤT BẠI.");

                cashForm.Close();
                ShowThanhToanThatBaiMessage(); // Hiện popup thông báo thất bại
            };

            cashForm.Controls.Add(lblTong);
            cashForm.Controls.Add(lblKhachDua);
            cashForm.Controls.Add(txtKhachDua);
            cashForm.Controls.Add(lblTrangThai);
            cashForm.Controls.Add(btnThanhCong);
            cashForm.Controls.Add(btnThatBai);

            cashForm.ShowDialog(this);
            return result;
        }

        /// <summary>
        /// Xử lý khi bấm nút "Thanh toán" trên PaymentForm.
        /// - Bước 1: kiểm tra đã nhập tên khách chưa.
        /// - Bước 2:
        ///   + Nếu chọn CHUYỂN KHOẢN → mở FormThanhToan (VNPay).
        ///     · Nếu VNPay OK (ResponseCode = "00") → gọi XuLyThanhToan("Chuyển khoản").
        ///   + Nếu chọn TIỀN MẶT → mở popup ShowCashPaymentDialog().
        ///     · Nếu khách đưa đủ tiền → gọi XuLyThanhToan("Tiền mặt").
        /// </summary>
        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            string phone = txtKhachHangName.Text.Trim();
            Khachhang customer = null;
            

            // Reset trạng thái mỗi lần bấm Thanh toán
            _isGuestForCurrentPhone = false;
            currentSelectedCustomer = null;

            // ========== BƯỚC 1: XỬ LÝ SỐ ĐIỆN THOẠI & KHÁCH HÀNG ==========

            // 1.1. Nếu KHÔNG nhập SĐT → khách vãng lai, bỏ qua tích điểm
            if (string.IsNullOrEmpty(phone))
            {
                _isGuestForCurrentPhone = true;
            }
            else
            {
                // 1.2. Có nhập SĐT → tìm trong DB
                try
                {
                    customer = _paymentBLL.GetKhachhangByPhone(phone);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi kiểm tra khách hàng: " + ex.Message,
                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (customer != null)
                {
                    // ĐÃ CÓ TÀI KHOẢN → thành viên, sẽ được tích điểm
                    currentSelectedCustomer = customer;
                    _isGuestForCurrentPhone = false;
                }
                else
                {
                    // CHƯA CÓ TÀI KHOẢN → hỏi có muốn tạo không
                    DialogResult rs = MessageBox.Show(
                        "Số điện thoại này chưa có tài khoản thành viên.\nBạn có muốn tạo mới không?",
                        "Khách hàng chưa tồn tại",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (rs == DialogResult.No)
                    {
                        // Không muốn tạo → thanh toán như khách vãng lai
                        _isGuestForCurrentPhone = true;
                    }
                    else
                    {
                        // Có → mở FormChitiet để tạo tài khoản
                        using (var form = new FormChitiet())
                        {
                            form.txtSDT.Text = phone;                   // điền sẵn SĐT
                            form.txtSDT.Enabled = false;                // khóa SĐT, tránh sửa sai
                            form.dateTimePickerNgayDangKy.Value = DateTime.Now;
                            form.numericUpDownDiem.Value = 0;

                            var dialogResult = form.ShowDialog(this);

                            if (dialogResult == DialogResult.OK)
                            {
                                // Lấy lại khách hàng sau khi tạo
                                try
                                {
                                    customer = _paymentBLL.GetKhachhangByPhone(phone);
                                    if (customer != null)
                                    {
                                        currentSelectedCustomer = customer;
                                        _isGuestForCurrentPhone = false;   // thành viên
                                    }
                                    else
                                    {
                                        // Trường hợp hiếm: tạo xong nhưng không lấy được → coi như vãng lai
                                        _isGuestForCurrentPhone = true;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("Lỗi khi lấy lại thông tin khách hàng vừa tạo: " + ex.Message,
                                                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                // User đóng form mà không lưu
                                DialogResult confirmGuest = MessageBox.Show(
                                    "Bạn chưa lưu thông tin khách hàng.\nBạn vẫn muốn thanh toán như khách vãng lai chứ?",
                                    "Xác nhận",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question
                                );

                                if (confirmGuest == DialogResult.Yes)
                                {
                                    _isGuestForCurrentPhone = true;   // vãng lai
                                }
                                else
                                {
                                    // Không muốn vãng lai → hủy luôn quy trình thanh toán
                                    return;
                                }
                            }
                        }
                    }
                }
            }

            // ========== BƯỚC 2: CHỌN HÌNH THỨC THANH TOÁN (SAU KHI XỬ LÝ KHÁCH HÀNG) ==========

            // Nhánh CHUYỂN KHOẢN (VNPay)
            if (rdbChuyenKhoan.Checked)
            {
                _pendingPaymentMethod = HINH_THUC_CHUYEN_KHOAN;

                // Lấy tổng tiền từ BLL
                decimal tongTienDecimal = _paymentBLL.CalculateTongTien();
                double amount = (double)tongTienDecimal;   // FormThanhToan dùng double

                // Lấy mã hóa đơn để đưa vào mô tả
                string maHoaDon = lblMaHoaDonValue.Text?.Trim();

                // Mô tả giao dịch gửi sang VNPay
                string description = $"Thanh toán hóa đơn {maHoaDon} - KH: {txtKhachHangName.Text}";

                // Mở form thanh toán VNPay (WebView2 + VNPay)
                using (var frm = new FormThanhToan(amount, description))
                {
                    var result = frm.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        // VNPay báo thành công (vnp_ResponseCode == "00")
                        XuLyThanhToan(HINH_THUC_CHUYEN_KHOAN);
                        Logger.LogInfo("Thanh toán VNPay thành công.");
                        return;
                    }
                    else
                    {
                        // Người dùng đóng form VNPay hoặc có lỗi
                        Logger.LogInfo("Thanh toán VNPay không thành công hoặc bị hủy.");
                        MessageBox.Show("Thanh toán chưa hoàn tất.",
                                        "Thông báo",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);
                        return;
                    }
                }
            }

            // Nhánh TIỀN MẶT
            _pendingPaymentMethod = HINH_THUC_TIEN_MAT;
            rdbTienMat.Checked = true;

            // Mở popup nhập tiền khách đưa
            if (ShowCashPaymentDialog())
            {
                // Nếu khách đưa đủ tiền → xử lý thanh toán
                XuLyThanhToan(HINH_THUC_TIEN_MAT);
            }
            else
            {
                Logger.LogInfo("Thanh toán tiền mặt chưa hoàn tất (hủy hoặc khách đưa thiếu).");
            }
        }


        /// <summary>
        /// Xử lý khi chọn radio Tiền mặt (hiện tại chỉ log lại).
        /// </summary>
        private void rdbTienMat_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbTienMat.Checked)
            {
                Logger.LogDebug("Hình thức thanh toán được chọn: Tiền mặt.");
            }
        }

        /// <summary>
        /// Nút "Thành công" (trong flow cũ hoặc xác nhận thủ công):
        /// - Kiểm tra đã bắt đầu quy trình thanh toán (đã bấm nút Thanh toán) chưa.
        /// - Nếu rồi thì gọi XuLyThanhToan với hình thức đang pending.
        /// </summary>
        private void btnThanhToanThanhCong_Click(object sender, EventArgs e)
        {
            MainForm.PlayClickSound();
            if (string.IsNullOrEmpty(_pendingPaymentMethod))
            {
                MessageBox.Show(
                    "Bạn chưa bắt đầu quy trình thanh toán.\n" +
                    "Vui lòng nhấn nút 'Thanh toán' trước, sau đó mới bấm 'Thành công'.",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            Logger.LogInfo($"Người dùng xác nhận THANH TOÁN THÀNH CÔNG ({_pendingPaymentMethod}).");
            XuLyThanhToan(_pendingPaymentMethod);
        }

        /// <summary>
        /// Thông báo thanh toán thất bại dùng chung cho nhiều chỗ.
        /// </summary>
        private void ShowThanhToanThatBaiMessage()
        {
            MessageBox.Show(
                "Thanh toán chưa thành công. Bạn có thể thực hiện lại quy trình thanh toán.",
                "Thanh toán thất bại",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        /// <summary>
        /// Nút "Thất bại":
        /// - Log lại hình thức thanh toán thất bại.
        /// - Ẩn 2 nút Thành công / Thất bại.
        /// - Reset _pendingPaymentMethod.
        /// - Hiện popup báo thất bại.
        /// </summary>
        private void btnThanhToanThatBai_Click(object sender, EventArgs e)
        {
            Logger.LogInfo($"Người dùng xác nhận THANH TOÁN THẤT BẠI ({_pendingPaymentMethod ?? "chưa bắt đầu"}).");

            // Ẩn 2 nút kết quả
            if (btnThanhToanThanhCong != null)
                btnThanhToanThanhCong.Visible = false;
            if (btnThanhToanThatBai != null)
                btnThanhToanThatBai.Visible = false;

            // Reset trạng thái, yêu cầu bấm lại nút "Thanh toán"
            _pendingPaymentMethod = null;

            // Dùng chung popup thất bại
            ShowThanhToanThatBaiMessage();
        }

        /// <summary>
        /// Sự kiện Leave của textbox tên khách:
        /// - Khi rời ô nhập tên:
        ///   + Nếu rỗng → clear info.
        ///   + Nếu có tên → kiểm tra trong DB:
        ///       · Nếu chưa có → hỏi user có muốn thêm mới KH không.
        ///       · Nếu đã tồn tại → chọn khách đó, thông báo đã tồn tại.
        /// </summary>
        private void txtKhachHangName_Leave(object sender, EventArgs e)
        {
            string phoneNumber = txtKhachHangName.Text.Trim();

            if (string.IsNullOrEmpty(phoneNumber))
            {
                // khách vãng lai → không làm gì
                ClearCustomerInfo();
                return;
            }

            try
            {
                Khachhang existingCustomer = _paymentBLL.GetKhachhangByPhone(phoneNumber);

                if (existingCustomer != null)
                {
                    currentSelectedCustomer = existingCustomer;
                    MessageBox.Show(
                        $"Khách hàng: {existingCustomer.Hoten}\nSĐT: {existingCustomer.Sodienthoai}",
                        "Tìm thấy khách hàng",
                        MessageBoxButtons.OK, MessageBoxIcon.Information
                    );
                }
                else
                {
                    ClearCustomerInfo();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xử lý khách hàng: {ex.Message}");
                ClearCustomerInfo();
            }
        }


        /// <summary>
        /// Hiện một form nhỏ để nhập tên khách hàng mới cho số điện thoại đã cho,
        /// sau đó gọi BLL để thêm khách hàng mới.
        /// </summary>
        private Khachhang CreateNewCustomerWithPhone(string phoneNumber)
        {
            // Tạo form nhập tên
            Form form = new Form
            {
                Text = "Thêm khách hàng mới",
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterParent,
                ClientSize = new Size(400, 180),
                MinimizeBox = false,
                MaximizeBox = false,
                ShowInTaskbar = false
            };

            Label lblPhone = new Label
            {
                AutoSize = true,
                Text = "Số điện thoại:",
                Location = new Point(20, 20)
            };
            TextBox txtPhone = new TextBox
            {
                Location = new Point(150, 17),
                Width = 200,
                ReadOnly = true,
                Text = phoneNumber
            };

            Label lblName = new Label
            {
                AutoSize = true,
                Text = "Tên khách hàng:",
                Location = new Point(20, 60)
            };
            TextBox txtName = new TextBox
            {
                Location = new Point(150, 57),
                Width = 200
            };

            Button btnOK = new Button
            {
                Text = "Lưu",
                Location = new Point(190, 110),
                Width = 80
            };
            Button btnCancel = new Button
            {
                Text = "Hủy",
                Location = new Point(280, 110),
                Width = 80
            };

            Khachhang newCustomer = null;

            btnOK.Click += (s, e) =>
            {
                string name = txtName.Text.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show("Vui lòng nhập tên khách hàng.", "Thiếu thông tin",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtName.Focus();
                    return;
                }

                try
                {
                    newCustomer = _paymentBLL.AddNewKhachhang(name, phoneNumber);
                    form.DialogResult = DialogResult.OK;
                    form.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnCancel.Click += (s, e) =>
            {
                form.DialogResult = DialogResult.Cancel;
                form.Close();
            };

            form.Controls.Add(lblPhone);
            form.Controls.Add(txtPhone);
            form.Controls.Add(lblName);
            form.Controls.Add(txtName);
            form.Controls.Add(btnOK);
            form.Controls.Add(btnCancel);

            var result = form.ShowDialog(this);
            return result == DialogResult.OK ? newCustomer : null;
        }


        /// <summary>
        /// Xóa thông tin khách hàng hiện tại trên form.
        /// </summary>
        private void ClearCustomerInfo()
        {
            currentSelectedCustomer = null;
            Logger.LogDebug("Thông tin khách hàng đã được xóa trên UI.");
        }

        private void lblNguoiLapValue_Click(object sender, EventArgs e)
        {
            // Không xử lý gì, để trống
        }

        /// <summary>
        /// Hàm PrintPage: vẽ nội dung hóa đơn để in.
        /// - Được gọi tự động khi printPreviewDialogInvoice.ShowDialog() hoặc in trực tiếp.
        /// - Vẽ:
        ///   + Tiêu đề "HÓA ĐƠN THANH TOÁN"
        ///   + Thông tin hóa đơn (Mã, Khách hàng, Người lập, Ngày)
        ///   + Bảng chi tiết từng món
        ///   + Tổng thành tiền
        /// </summary>
        /// 
        //private void printDocumentInvoice_PrintPage(object sender, PrintPageEventArgs e)
        //{

        //    Graphics graphics = e.Graphics;
        //    Font headerFont = new Font("Arial", 16, FontStyle.Bold);
        //    Font subHeaderFont = new Font("Arial", 11, FontStyle.Bold);
        //    Font normalFont = new Font("Arial", 10);
        //    Font smallFont = new Font("Arial", 9);
        //    Pen borderPen = new Pen(Color.Black, 1);

        //    float lineHeight = normalFont.GetHeight() + 2;
        //    float x = e.MarginBounds.Left;
        //    float y = e.MarginBounds.Top;
        //    float currentX;

        //    // Tiêu đề hóa đơn
        //    StringFormat sfCenter = new StringFormat();
        //    sfCenter.Alignment = StringAlignment.Center;
        //    sfCenter.LineAlignment = StringAlignment.Center;
        //    graphics.DrawString("HÓA ĐƠN THANH TOÁN", headerFont, Brushes.Black, e.PageBounds.Width / 2, y, sfCenter);
        //    y += headerFont.GetHeight() + 20;

        //    // Thông tin hóa đơn chung
        //    graphics.DrawString($"Mã hóa đơn: {lblMaHoaDonValue.Text}", normalFont, Brushes.Black, x, y);
        //    y += lineHeight;
        //    string khachHangText;
        //    if (currentSelectedCustomer != null)
        //    {
        //        khachHangText = $"{currentSelectedCustomer.Hoten} - {currentSelectedCustomer.Sodienthoai}";
        //    }
        //    else
        //    {
        //        khachHangText = txtKhachHangName.Text; // chỉ có SĐT
        //    }
        //    graphics.DrawString($"Khách hàng: {khachHangText}", normalFont, Brushes.Black, x, y);
        //    y += lineHeight;
        //    graphics.DrawString($"Người lập: {lblNguoiLapValue.Text}", normalFont, Brushes.Black, x, y);
        //    y += lineHeight;
        //    graphics.DrawString($"Ngày: {lblNgayValue.Text}", normalFont, Brushes.Black, x, y);
        //    y += lineHeight + 20;

        //    // Định nghĩa độ rộng các cột
        //    float colSTTWidth = 50;
        //    float colTenDoUongWidth = 200;
        //    float colSoLuongWidth = 80;
        //    float colDonGiaWidth = 100;
        //    float colThanhTienWidth = 120;

        //    // In tiêu đề cột bảng chi tiết
        //    currentX = x;
        //    RectangleF headerRect;
        //    StringFormat sfHeader = new StringFormat();
        //    sfHeader.Alignment = StringAlignment.Center;
        //    sfHeader.LineAlignment = StringAlignment.Center;

        //    headerRect = new RectangleF(currentX, y, colSTTWidth, lineHeight + 5);
        //    graphics.FillRectangle(Brushes.LightGray, headerRect);
        //    graphics.DrawRectangle(borderPen, currentX, y, colSTTWidth, lineHeight + 5);
        //    graphics.DrawString("STT", subHeaderFont, Brushes.Black, headerRect, sfHeader);
        //    currentX += colSTTWidth;

        //    headerRect = new RectangleF(currentX, y, colTenDoUongWidth, lineHeight + 5);
        //    graphics.FillRectangle(Brushes.LightGray, headerRect);
        //    graphics.DrawRectangle(borderPen, currentX, y, colTenDoUongWidth, lineHeight + 5);
        //    graphics.DrawString("Tên đồ uống", subHeaderFont, Brushes.Black, headerRect, sfHeader);
        //    currentX += colTenDoUongWidth;

        //    headerRect = new RectangleF(currentX, y, colSoLuongWidth, lineHeight + 5);
        //    graphics.FillRectangle(Brushes.LightGray, headerRect);
        //    graphics.DrawRectangle(borderPen, currentX, y, colSoLuongWidth, lineHeight + 5);
        //    graphics.DrawString("Số lượng", subHeaderFont, Brushes.Black, headerRect, sfHeader);
        //    currentX += colSoLuongWidth;

        //    headerRect = new RectangleF(currentX, y, colDonGiaWidth, lineHeight + 5);
        //    graphics.FillRectangle(Brushes.LightGray, headerRect);
        //    graphics.DrawRectangle(borderPen, currentX, y, colDonGiaWidth, lineHeight + 5);
        //    graphics.DrawString("Đơn giá", subHeaderFont, Brushes.Black, headerRect, sfHeader);
        //    currentX += colDonGiaWidth;

        //    headerRect = new RectangleF(currentX, y, colThanhTienWidth, lineHeight + 5);
        //    graphics.FillRectangle(Brushes.LightGray, headerRect);
        //    graphics.DrawRectangle(borderPen, currentX, y, colThanhTienWidth, lineHeight + 5);
        //    graphics.DrawString("Thành tiền", subHeaderFont, Brushes.Black, headerRect, sfHeader);
        //    currentX += colThanhTienWidth;

        //    y += lineHeight + 5;

        //    // Các StringFormat phục vụ việc căng chữ
        //    StringFormat sfLeft = new StringFormat();
        //    sfLeft.Alignment = StringAlignment.Near;
        //    sfLeft.LineAlignment = StringAlignment.Center;
        //    sfLeft.Trimming = StringTrimming.EllipsisCharacter;
        //    sfLeft.FormatFlags = StringFormatFlags.NoWrap;

        //    StringFormat sfCenterData = new StringFormat();
        //    sfCenterData.Alignment = StringAlignment.Center;
        //    sfCenterData.LineAlignment = StringAlignment.Center;

        //    StringFormat sfRight = new StringFormat();
        //    sfRight.Alignment = StringAlignment.Far;
        //    sfRight.LineAlignment = StringAlignment.Center;
        //    sfRight.Trimming = StringTrimming.EllipsisCharacter;
        //    sfRight.FormatFlags = StringFormatFlags.NoWrap;

        //    // In từng dòng chi tiết đơn hàng
        //    List<Chitietdonhang> dsChiTiet = _paymentBLL.GetDsChiTietHoaDon();
        //    for (int i = 0; i < dsChiTiet.Count; i++)
        //    {
        //        Chitietdonhang chiTiet = dsChiTiet[i];
        //        currentX = x;

        //        // STT
        //        graphics.DrawString((i + 1).ToString(), smallFont, Brushes.Black,
        //                            new RectangleF(currentX, y, colSTTWidth, lineHeight), sfCenterData);
        //        graphics.DrawRectangle(borderPen, currentX, y, colSTTWidth, lineHeight);
        //        currentX += colSTTWidth;

        //        // Tên đồ uống
        //        graphics.DrawString(chiTiet.Tendouong, smallFont, Brushes.Black,
        //                            new RectangleF(currentX, y, colTenDoUongWidth, lineHeight), sfLeft);
        //        graphics.DrawRectangle(borderPen, currentX, y, colTenDoUongWidth, lineHeight);
        //        currentX += colTenDoUongWidth;

        //        // Số lượng
        //        graphics.DrawString(chiTiet.Soluong.ToString(), smallFont, Brushes.Black,
        //                            new RectangleF(currentX, y, colSoLuongWidth, lineHeight), sfCenterData);
        //        graphics.DrawRectangle(borderPen, currentX, y, colSoLuongWidth, lineHeight);
        //        currentX += colSoLuongWidth;

        //        // Đơn giá
        //        graphics.DrawString(chiTiet.Dongia.ToString("N0"), smallFont, Brushes.Black,
        //                            new RectangleF(currentX, y, colDonGiaWidth, lineHeight), sfRight);
        //        graphics.DrawRectangle(borderPen, currentX, y, colDonGiaWidth, lineHeight);
        //        currentX += colDonGiaWidth;

        //        // Thành tiền
        //        graphics.DrawString(chiTiet.Thanhtien.ToString("N0"), smallFont, Brushes.Black,
        //                            new RectangleF(currentX, y, colThanhTienWidth, lineHeight), sfRight);
        //        graphics.DrawRectangle(borderPen, currentX, y, colThanhTienWidth, lineHeight);
        //        currentX += colThanhTienWidth;

        //        y += lineHeight;
        //    }

        //    // Tổng thành tiền
        //    y += 20;
        //    string totalText = $"Tổng thành tiền: {txtTongThanhTienValue.Text} VNĐ";
        //    graphics.DrawString(totalText, subHeaderFont, Brushes.Black,
        //                        e.MarginBounds.Right - graphics.MeasureString(totalText, subHeaderFont).Width, y);

        //    e.HasMorePages = false; // In trong 1 trang
        //}
        private void printDocumentInvoice_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font headerFont = new Font("Arial", 16, FontStyle.Bold);
            Font subHeaderFont = new Font("Arial", 11, FontStyle.Bold);
            Font normalFont = new Font("Arial", 10);
            Font smallFont = new Font("Arial", 9);
            Pen borderPen = new Pen(Color.Black, 1);

            float lineHeight = normalFont.GetHeight() + 2;
            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float currentX;

            // ===================== (A) LOGO + THÔNG TIN CÔNG TY =====================
            int logoSize = 70; // chỉnh 60-90 tùy bạn
            Image logo = null;
            try { logo = Properties.Resources.CompanyLogo; } catch { /* nếu chưa có logo */ }

            if (logo != null)
            {
                // vẽ logo góc trái
                Rectangle logoRect = new Rectangle((int)x, (int)y, logoSize, logoSize);
                graphics.DrawImage(logo, logoRect);

                // thông tin công ty bên phải logo (tuỳ chỉnh)
                float infoX = x + logoSize + 10;
                float infoY = y + 2;
                graphics.DrawString("COFFEE HOUSE", subHeaderFont, Brushes.Black, infoX, infoY);
                infoY += lineHeight;
                graphics.DrawString("Địa chỉ: 125 Nguyễn Trãi, P. Bến Thành, Q.1, TP.HCM", smallFont, Brushes.Black, infoX, infoY);
                infoY += smallFont.GetHeight() + 2;
                graphics.DrawString("Hotline: 0901 234 567", smallFont, Brushes.Black, infoX, infoY);
                // đẩy y xuống dưới logo
                y += logoSize + 10;
            }
            else
            {
                // nếu chưa có logo, vẫn chừa khoảng cho phần đầu trang
                y += 10;
            }

            // ===================== (B) TIÊU ĐỀ HÓA ĐƠN =====================
            StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            graphics.DrawString("HÓA ĐƠN THANH TOÁN", headerFont, Brushes.Black, e.PageBounds.Width / 2, y, sfCenter);
            y += headerFont.GetHeight() + 15;

            // ===================== (C) THÔNG TIN HÓA ĐƠN =====================
            graphics.DrawString($"Mã hóa đơn: {lblMaHoaDonValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight;

            string khachHangText = (currentSelectedCustomer != null)
                ? $"{currentSelectedCustomer.Hoten} - {currentSelectedCustomer.Sodienthoai}"
                : txtKhachHangName.Text;

            graphics.DrawString($"Khách hàng: {khachHangText}", normalFont, Brushes.Black, x, y);
            y += lineHeight;
            graphics.DrawString($"Người lập: {lblNguoiLapValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight;
            graphics.DrawString($"Ngày: {lblNgayValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight + 15;

            // ===================== (D) BẢNG CHI TIẾT (GIỮ NGUYÊN CODE CỦA BẠN) =====================
            float colSTTWidth = 50;
            float colTenDoUongWidth = 200;
            float colSoLuongWidth = 80;
            float colDonGiaWidth = 100;
            float colThanhTienWidth = 120;

            // ... (phần vẽ header bảng + dữ liệu như bạn đang có) ...

            // ===================== (E) TỔNG TIỀN =====================
            y += 20;
            string totalText = $"Tổng thành tiền: {txtTongThanhTienValue.Text} VNĐ";
            graphics.DrawString(totalText, subHeaderFont, Brushes.Black,
                e.MarginBounds.Right - graphics.MeasureString(totalText, subHeaderFont).Width, y);

            // ===================== (F) KHU VỰC CHỮ KÝ =====================
            y += 50; // khoảng cách sau tổng tiền
            float signTop = y;

            // Font chữ ký kiểu viết tay + fallback
            Font signFont;
            try
            {
                signFont = new Font("Segoe Script", 13, FontStyle.Italic);
            }
            catch
            {
                signFont = new Font("Lucida Handwriting", 12, FontStyle.Italic);
            }

            // Màu chữ ký xám đậm cho giống thật
            Brush signBrush = new SolidBrush(Color.FromArgb(60, 60, 60));

            // Lấy tên khách để ký (ưu tiên tên thành viên; nếu không có thì coi như khách vãng lai)
            string tenKhachKy = (currentSelectedCustomer != null && !string.IsNullOrWhiteSpace(currentSelectedCustomer.Hoten))
                ? currentSelectedCustomer.Hoten.Trim()
                : "Khách vãng lai";

            if (string.IsNullOrWhiteSpace(tenKhachKy))
                tenKhachKy = "Khách vãng lai";

            // Lấy tên thu ngân để ký
            string tenThuNganKy = !string.IsNullOrWhiteSpace(lblNguoiLapValue.Text)
                ? lblNguoiLapValue.Text.Trim()
                : "Thu ngân";

            // Tính layout 2 cột
            float halfWidth = e.MarginBounds.Width / 2f;
            float leftX = x;
            float rightX = x + halfWidth;

            float blockWidth = halfWidth;
            float lineY = signTop + 80;    // vị trí dòng kẻ
            float signY = signTop + 50;    // vị trí chữ ký

            StringFormat sfCenterText = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };

            // ----- Cột trái: Khách hàng -----
            graphics.DrawString("Khách hàng", subHeaderFont, Brushes.Black,
                new RectangleF(leftX, signTop, blockWidth, 20), sfCenterText);

            graphics.DrawString("(Ký, ghi rõ họ tên)", smallFont, Brushes.Black,
                new RectangleF(leftX, signTop + 18, blockWidth, 20), sfCenterText);

            // dòng kẻ
            graphics.DrawLine(Pens.Black, leftX + 40, lineY, leftX + blockWidth - 40, lineY);

            // chữ ký kiểu
            graphics.DrawString(tenKhachKy, signFont, signBrush,
                new RectangleF(leftX, signY, blockWidth, 30), sfCenterText);


            // ----- Cột phải: Thu ngân -----
            graphics.DrawString("Thu ngân", subHeaderFont, Brushes.Black,
                new RectangleF(rightX, signTop, blockWidth, 20), sfCenterText);

            graphics.DrawString("(Ký, ghi rõ họ tên)", smallFont, Brushes.Black,
                new RectangleF(rightX, signTop + 18, blockWidth, 20), sfCenterText);

            // dòng kẻ
            graphics.DrawLine(Pens.Black, rightX + 40, lineY, rightX + blockWidth - 40, lineY);

            // chữ ký kiểu
            graphics.DrawString(tenThuNganKy, signFont, signBrush,
                new RectangleF(rightX, signY, blockWidth, 30), sfCenterText);


            // (Tuỳ chọn) Lời cảm ơn ở cuối
            float thanksY = signTop + 95;
            string thanks = "Cảm ơn quý khách. Hẹn gặp lại!";
            graphics.DrawString(thanks, normalFont, Brushes.Black,
                e.PageBounds.Width / 2, thanksY, sfCenter);
        }



        /// <summary>
        /// Nút đóng form PaymentForm (nếu có nút Close trên giao diện).
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtKhachHangName_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void txtKhachHangName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Chỉ cho phép chữ số và phím điều khiển (Backspace, Delete, ...)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }

            // Đã giới hạn MaxLength = 10, nhưng để chắc ăn có thể chặn luôn ở đây
            if (!char.IsControl(e.KeyChar) && txtKhachHangName.Text.Length >= 10)
            {
                e.Handled = true;
            }
        }
    }

    /// <summary>
    /// Exception riêng để thông báo không tìm thấy khách hàng.
    /// - Được BLL ném ra trong các trường hợp tìm KH không thấy.
    /// - PaymentForm bắt và hỏi user có muốn thêm mới khách hàng hay không.
    /// </summary>
    public class KhachhangNotFoundException : Exception
    {
        public KhachhangNotFoundException() { }
        public KhachhangNotFoundException(string message) : base(message) { }
        public KhachhangNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
