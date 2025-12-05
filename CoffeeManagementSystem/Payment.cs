    using CoffeeManagementSystem.BLL;
using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing; // Dùng để in hóa đơn
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
            // Ràng buộc: bắt buộc nhập tên khách
            string tenKhach = txtKhachHangName.Text.Trim();
            if (string.IsNullOrEmpty(tenKhach))
            {
                MessageBox.Show(
                    "Vui lòng nhập tên khách hàng trước khi xác nhận thanh toán.",
                    "Thiếu thông tin khách hàng",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtKhachHangName.Focus();
                Logger.LogWarning("Thử thanh toán nhưng chưa nhập tên khách hàng.");
                return;
            }
            Logger.LogInfo($"Bắt đầu xử lý thanh toán. Hình thức: {hinhThucThanhToan}");

            try
            {
                Khachhang customerFromBLL;

                // Lấy mã nhân viên thu ngân từ BLL
                string manhanvienThuNgan = _paymentBLL.GetManhanvienLapHoaDon();
                string ghiChu = ""; // Có thể dùng nếu sau này cần lưu ghi chú

                // Gọi tầng BLL xử lý thanh toán + lưu hóa đơn
                bool success = _paymentBLL.ProcessPayment(
                    txtKhachHangName.Text.Trim(),
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
                                // PHÁT ÂM THANH WAV KHÔNG CHẶN UI
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

                        //CODE CŨ 
                        //// ĐỌC GIỌNG NÓI KHÔNG CHẶN UI (dùng thread STA riêng)
                        //try
                        //{
                        //    Task.Run(() =>
                        //    {
                        //        try
                        //        {
                        //            var t = new Thread(() =>
                        //            {
                        //                try
                        //                {
                        //                    //using (SpeechSynthesizer synth = new SpeechSynthesizer())
                        //                    //{
                        //                    //    synth.Rate = 1;
                        //                    //    synth.Volume = 100;

                        //                    //    string formattedTien = string.Format("{0:N0}", tongTien);
                        //                    //    synth.Speak($"Ding ding! Successful payment. We have received {formattedTien} Viet Nam Dong!");
                        //                    //}
                        //                    using (SpeechSynthesizer synth = new SpeechSynthesizer())
                        //                    {
                        //                        // Tốc độ bình thường / hơi chậm cho dễ nghe tiếng Việt
                        //                        synth.Rate = 0;
                        //                        synth.Volume = 100;

                        //                        // Cố gắng chọn voice tiếng Việt nếu máy có cài (ví dụ: Microsoft An, Microsoft Linh…)
                        //                        try
                        //                        {
                        //                            InstalledVoice vnVoice = null;
                        //                            foreach (var v in synth.GetInstalledVoices())
                        //                            {
                        //                                // Kiểm tra culture bắt đầu bằng "vi" (vi-VN)
                        //                                if (string.Equals(
                        //                                        v.VoiceInfo.Culture.TwoLetterISOLanguageName,
                        //                                        "vi",
                        //                                        StringComparison.OrdinalIgnoreCase))
                        //                                {
                        //                                    vnVoice = v;
                        //                                    break;
                        //                                }
                        //                            }

                        //                            if (vnVoice != null)
                        //                            {
                        //                                synth.SelectVoice(vnVoice.VoiceInfo.Name);
                        //                            }
                        //                            else
                        //                            {
                        //                                // Không có voice tiếng Việt thì log lại, vẫn dùng voice mặc định
                        //                                Logger.LogWarning("Không tìm thấy voice tiếng Việt, sử dụng voice mặc định của hệ thống.");
                        //                            }
                        //                        }
                        //                        catch (Exception voiceEx)
                        //                        {
                        //                            Logger.LogError("Lỗi khi chọn voice tiếng Việt: " + voiceEx.Message);
                        //                            // Nếu lỗi thì thôi dùng voice mặc định
                        //                        }

                        //                        string formattedTien = string.Format("{0:N0}", tongTien);

                        //                        // Câu đọc tiếng Việt
                        //                        synth.Speak($"Ting ting! Đơn hàng đã được thanh toán thành công. Cửa hàng đã nhận {formattedTien} đồng.");
                        //                    }

                        //                }
                        //                catch (Exception synthEx)
                        //                {
                        //                    Logger.LogError("Lỗi khi đọc giọng nói: " + synthEx.Message);
                        //                }
                        //            });
                        //            t.SetApartmentState(ApartmentState.STA);
                        //            t.Start();
                        //        }
                        //        catch (Exception threadEx)
                        //        {
                        //            Logger.LogError("Lỗi khi tạo thread giọng nói STA: " + threadEx.Message);
                        //        }
                        //    });
                        //}
                        //catch (Exception task2Ex)
                        //{
                        //    Logger.LogError("Lỗi khi khởi chạy Task giọng nói: " + task2Ex.Message);
                        //}
                        // ĐỌC GIỌNG NÓI KHÔNG CHẶN UI (dùng thread STA riêng)
                        // ĐỌC GIỌNG NÓI KHÔNG CHẶN UI (dùng thread STA riêng)
                        // PHÁT ÂM THANH GIỌNG ĐỌC "THANH TOÁN THÀNH CÔNG" KHÔNG CHẶN UI
                        try
                        {
                            Task.Run(() =>
                            {
                                try
                                {
                                    // Cho tiếng ting ting phát trước một chút
                                    Thread.Sleep(400);

                                    using (SoundPlayer playerVoice = new SoundPlayer(Properties.Resources.ThanhToanThanhCong))
                                    {
                                        // PlaySync cho nó đọc hết câu, nhưng vì đang ở Task riêng nên không block UI
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
                // Lỗi nghiệp vụ (VD: không cho thanh toán trong 1 số trường hợp)
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.LogError($"Thanh toán thất bại (lỗi nghiệp vụ): {ex.Message}", ex);
            }
            catch (KhachhangNotFoundException ex)
            {
                // Khách hàng không tồn tại trong DB
                Logger.LogError($"Khách hàng '{txtKhachHangName.Text.Trim()}' không tìm thấy khi thanh toán.", ex);

                DialogResult addCustomer = MessageBox.Show(
                    ex.Message + Environment.NewLine + "Bạn có muốn thêm mới khách hàng này không?",
                    "Xác nhận thêm khách hàng",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (addCustomer == DialogResult.Yes)
                {
                    Logger.LogInfo($"Người dùng muốn thêm mới khách hàng: {txtKhachHangName.Text.Trim()}.");
                    try
                    {
                        // Thêm mới khách hàng vào DB qua BLL
                        currentSelectedCustomer = _paymentBLL.AddNewKhachhang(txtKhachHangName.Text.Trim());
                        MessageBox.Show($"Đã thêm mới khách hàng: {txtKhachHangName.Text.Trim()}.",
                            "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Logger.LogInfo($"Đã thêm mới khách hàng '{txtKhachHangName.Text.Trim()}' thông qua UI prompt.");

                        // Sau khi thêm khách hàng, thử thanh toán lại với hình thức cũ
                        XuLyThanhToan(hinhThucThanhToan);
                    }
                    catch (Exception addEx)
                    {
                        MessageBox.Show($"Lỗi khi thêm mới khách hàng: {addEx.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Logger.LogError($"Lỗi khi thêm mới khách hàng '{txtKhachHangName.Text.Trim()}' từ UI.", addEx);
                        ClearCustomerInfo();
                    }
                }
                else
                {
                    // Người dùng không muốn thêm khách → clear
                    txtKhachHangName.Text = "";
                    ClearCustomerInfo();
                    Logger.LogInfo($"Người dùng từ chối thêm mới khách hàng '{txtKhachHangName.Text.Trim()}'.");
                }
            }
            catch (ArgumentException ex)
            {
                // Lỗi tham số không hợp lệ
                MessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Logger.LogError($"Thanh toán thất bại (tham số không hợp lệ): {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                // Lỗi hệ thống không xác định
                MessageBox.Show($"Lỗi khi thanh toán đơn hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError($"Lỗi hệ thống không xác định khi thanh toán đơn hàng. Tên khách hàng: '{txtKhachHangName.Text.Trim()}'", ex);
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
            Logger.LogInfo("Người dùng nhấn nút 'Thanh toán'.");

            // Ràng buộc: phải nhập tên khách trước khi thanh toán
            if (string.IsNullOrWhiteSpace(txtKhachHangName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng trước khi thanh toán.",
                                "Thiếu thông tin",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

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
            string customerName = txtKhachHangName.Text.Trim();
            Logger.LogDebug($"txtKhachHangName_Leave được kích hoạt với tên: '{customerName}'");

            if (string.IsNullOrEmpty(customerName))
            {
                ClearCustomerInfo();
                Logger.LogInfo("Tên khách hàng rỗng, thông tin khách hàng đã được xóa.");
                return;
            }

            try
            {
                // Tìm khách hàng theo tên
                Khachhang existingCustomer = _paymentBLL.GetKhachhangByName(customerName);

                if (existingCustomer == null)
                {
                    Logger.LogInfo($"Khách hàng '{customerName}' không tồn tại. Hỏi người dùng có muốn thêm mới.");
                    DialogResult confirmResult = MessageBox.Show(
                        $"Khách hàng '{customerName}' chưa tồn tại. Bạn có muốn thêm mới khách hàng này không?",
                        "Xác nhận thêm khách hàng",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirmResult == DialogResult.Yes)
                    {
                        Logger.LogInfo($"Người dùng muốn thêm mới khách hàng: {customerName}.");
                        try
                        {
                            currentSelectedCustomer = _paymentBLL.AddNewKhachhang(customerName);
                            MessageBox.Show($"Đã thêm mới khách hàng: {customerName}", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Logger.LogInfo($"Đã thêm mới khách hàng '{customerName}'.");
                        }
                        catch (Exception addEx)
                        {
                            MessageBox.Show($"Lỗi khi thêm mới khách hàng: {addEx.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Logger.LogError($"Lỗi khi thêm mới khách hàng '{customerName}'.", addEx);
                            ClearCustomerInfo();
                        }
                    }
                    else
                    {
                        txtKhachHangName.Text = "";
                        ClearCustomerInfo();
                        Logger.LogInfo($"Người dùng từ chối thêm mới khách hàng '{customerName}'.");
                    }
                }
                else
                {
                    // Khách đã tồn tại → chọn khách này
                    currentSelectedCustomer = existingCustomer;
                    MessageBox.Show($"Khách hàng '{customerName}' đã tồn tại.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Logger.LogInfo($"Đã tìm thấy và chọn khách hàng '{customerName}' (Mã: {currentSelectedCustomer.Makhachhang}).");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý khách hàng: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logger.LogError($"Lỗi khi xử lý khách hàng '{customerName}' trong PaymentForm.txtKhachHangName_Leave.", ex);
                ClearCustomerInfo();
            }
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

            // Tiêu đề hóa đơn
            StringFormat sfCenter = new StringFormat();
            sfCenter.Alignment = StringAlignment.Center;
            sfCenter.LineAlignment = StringAlignment.Center;
            graphics.DrawString("HÓA ĐƠN THANH TOÁN", headerFont, Brushes.Black, e.PageBounds.Width / 2, y, sfCenter);
            y += headerFont.GetHeight() + 20;

            // Thông tin hóa đơn chung
            graphics.DrawString($"Mã hóa đơn: {lblMaHoaDonValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight;
            graphics.DrawString($"Khách hàng: {txtKhachHangName.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight;
            graphics.DrawString($"Người lập: {lblNguoiLapValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight;
            graphics.DrawString($"Ngày: {lblNgayValue.Text}", normalFont, Brushes.Black, x, y);
            y += lineHeight + 20;

            // Định nghĩa độ rộng các cột
            float colSTTWidth = 50;
            float colTenDoUongWidth = 200;
            float colSoLuongWidth = 80;
            float colDonGiaWidth = 100;
            float colThanhTienWidth = 120;

            // In tiêu đề cột bảng chi tiết
            currentX = x;
            RectangleF headerRect;
            StringFormat sfHeader = new StringFormat();
            sfHeader.Alignment = StringAlignment.Center;
            sfHeader.LineAlignment = StringAlignment.Center;

            headerRect = new RectangleF(currentX, y, colSTTWidth, lineHeight + 5);
            graphics.FillRectangle(Brushes.LightGray, headerRect);
            graphics.DrawRectangle(borderPen, currentX, y, colSTTWidth, lineHeight + 5);
            graphics.DrawString("STT", subHeaderFont, Brushes.Black, headerRect, sfHeader);
            currentX += colSTTWidth;

            headerRect = new RectangleF(currentX, y, colTenDoUongWidth, lineHeight + 5);
            graphics.FillRectangle(Brushes.LightGray, headerRect);
            graphics.DrawRectangle(borderPen, currentX, y, colTenDoUongWidth, lineHeight + 5);
            graphics.DrawString("Tên đồ uống", subHeaderFont, Brushes.Black, headerRect, sfHeader);
            currentX += colTenDoUongWidth;

            headerRect = new RectangleF(currentX, y, colSoLuongWidth, lineHeight + 5);
            graphics.FillRectangle(Brushes.LightGray, headerRect);
            graphics.DrawRectangle(borderPen, currentX, y, colSoLuongWidth, lineHeight + 5);
            graphics.DrawString("Số lượng", subHeaderFont, Brushes.Black, headerRect, sfHeader);
            currentX += colSoLuongWidth;

            headerRect = new RectangleF(currentX, y, colDonGiaWidth, lineHeight + 5);
            graphics.FillRectangle(Brushes.LightGray, headerRect);
            graphics.DrawRectangle(borderPen, currentX, y, colDonGiaWidth, lineHeight + 5);
            graphics.DrawString("Đơn giá", subHeaderFont, Brushes.Black, headerRect, sfHeader);
            currentX += colDonGiaWidth;

            headerRect = new RectangleF(currentX, y, colThanhTienWidth, lineHeight + 5);
            graphics.FillRectangle(Brushes.LightGray, headerRect);
            graphics.DrawRectangle(borderPen, currentX, y, colThanhTienWidth, lineHeight + 5);
            graphics.DrawString("Thành tiền", subHeaderFont, Brushes.Black, headerRect, sfHeader);
            currentX += colThanhTienWidth;

            y += lineHeight + 5;

            // Các StringFormat phục vụ việc căng chữ
            StringFormat sfLeft = new StringFormat();
            sfLeft.Alignment = StringAlignment.Near;
            sfLeft.LineAlignment = StringAlignment.Center;
            sfLeft.Trimming = StringTrimming.EllipsisCharacter;
            sfLeft.FormatFlags = StringFormatFlags.NoWrap;

            StringFormat sfCenterData = new StringFormat();
            sfCenterData.Alignment = StringAlignment.Center;
            sfCenterData.LineAlignment = StringAlignment.Center;

            StringFormat sfRight = new StringFormat();
            sfRight.Alignment = StringAlignment.Far;
            sfRight.LineAlignment = StringAlignment.Center;
            sfRight.Trimming = StringTrimming.EllipsisCharacter;
            sfRight.FormatFlags = StringFormatFlags.NoWrap;

            // In từng dòng chi tiết đơn hàng
            List<Chitietdonhang> dsChiTiet = _paymentBLL.GetDsChiTietHoaDon();
            for (int i = 0; i < dsChiTiet.Count; i++)
            {
                Chitietdonhang chiTiet = dsChiTiet[i];
                currentX = x;

                // STT
                graphics.DrawString((i + 1).ToString(), smallFont, Brushes.Black,
                                    new RectangleF(currentX, y, colSTTWidth, lineHeight), sfCenterData);
                graphics.DrawRectangle(borderPen, currentX, y, colSTTWidth, lineHeight);
                currentX += colSTTWidth;

                // Tên đồ uống
                graphics.DrawString(chiTiet.Tendouong, smallFont, Brushes.Black,
                                    new RectangleF(currentX, y, colTenDoUongWidth, lineHeight), sfLeft);
                graphics.DrawRectangle(borderPen, currentX, y, colTenDoUongWidth, lineHeight);
                currentX += colTenDoUongWidth;

                // Số lượng
                graphics.DrawString(chiTiet.Soluong.ToString(), smallFont, Brushes.Black,
                                    new RectangleF(currentX, y, colSoLuongWidth, lineHeight), sfCenterData);
                graphics.DrawRectangle(borderPen, currentX, y, colSoLuongWidth, lineHeight);
                currentX += colSoLuongWidth;

                // Đơn giá
                graphics.DrawString(chiTiet.Dongia.ToString("N0"), smallFont, Brushes.Black,
                                    new RectangleF(currentX, y, colDonGiaWidth, lineHeight), sfRight);
                graphics.DrawRectangle(borderPen, currentX, y, colDonGiaWidth, lineHeight);
                currentX += colDonGiaWidth;

                // Thành tiền
                graphics.DrawString(chiTiet.Thanhtien.ToString("N0"), smallFont, Brushes.Black,
                                    new RectangleF(currentX, y, colThanhTienWidth, lineHeight), sfRight);
                graphics.DrawRectangle(borderPen, currentX, y, colThanhTienWidth, lineHeight);
                currentX += colThanhTienWidth;

                y += lineHeight;
            }

            // Tổng thành tiền
            y += 20;
            string totalText = $"Tổng thành tiền: {txtTongThanhTienValue.Text} VNĐ";
            graphics.DrawString(totalText, subHeaderFont, Brushes.Black,
                                e.MarginBounds.Right - graphics.MeasureString(totalText, subHeaderFont).Width, y);

            e.HasMorePages = false; // In trong 1 trang
        }

        /// <summary>
        /// Nút đóng form PaymentForm (nếu có nút Close trên giao diện).
        /// </summary>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
