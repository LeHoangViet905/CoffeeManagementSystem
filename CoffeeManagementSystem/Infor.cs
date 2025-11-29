using CoffeeManagementSystem.BLL;
using Microsoft.VisualBasic;
using MimeKit;
using OpenTK;
using System;
using System.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CoffeeManagementSystem
{
    public partial class Infor : Form
    {
        private InforBLL _inforBLL;
        private string _loggedInManhanvien;

        public Infor(string manhanvien)
        {
            InitializeComponent();
            this.Text = "Thông Tin Cá Nhân";
            _loggedInManhanvien = manhanvien;

            // Khởi tạo BLL và truyền mã nhân viên
            _inforBLL = new InforBLL(_loggedInManhanvien);

            // Gán sự kiện cho Form và nút
            this.Load += Infor_Load;
            this.btnLuuThayDoi.Click += btnLuuThayDoi_Click;

            // Cấu hình TextBox Mật khẩu là PasswordChar để ẩn ký tự
            txtMatKhau.PasswordChar = '●';
        }

        private void Infor_Load(object sender, EventArgs e)
        {
            LoadThongTinCaNhan();
        }

        /// <summary>
        /// Tải thông tin cá nhân bằng cách gọi BLL và hiển thị lên các control.
        /// </summary>
        private void LoadThongTinCaNhan()
        {
            try
            {
                // Gọi BLL để tải dữ liệu
                Tuple<Nhanvien, Taikhoan> data = _inforBLL.LoadInforData();

                if (data != null)
                {
                    Nhanvien nhanvien = data.Item1;
                    Taikhoan taikhoan = data.Item2;

                    // Hiển thị thông tin lên các control
                    txtMaNhanVien.Text = nhanvien.Manhanvien;
                    txtTenNhanVien.Text = nhanvien.Hoten;
                    dtpNgaySinh.Value = nhanvien.Ngaysinh;
                    txtGioiTinh.Text = nhanvien.Gioitinh;
                    txtDiaChi.Text = nhanvien.Diachi;
                    txtSoDienThoai.Text = nhanvien.Sodienthoai;
                    txtEmail.Text = nhanvien.Email;
                    dtpNgayVaoLam.Value = nhanvien.Ngayvaolam;

                    txtTenDangNhap.Text = taikhoan.Tendangnhap;
                    // Phân luồng theo quyền
                    if (taikhoan.Vaitro?.ToLower() == "admin" || taikhoan.Vaitro?.ToLower() == "quanly")
                    {
                        txtMatKhau.Text = "admin123";
                    }
                    else
                    {
                        txtMatKhau.Text = "nv123";
                    }

                    // Không cho chỉnh mật khẩu
                    txtMatKhau.ReadOnly = true;
                    txtMatKhau.PasswordChar = '●';


                    // Vô hiệu hóa các trường không cho phép chỉnh sửa (Logic UI)
                    txtMaNhanVien.Enabled = false;
                    dtpNgayVaoLam.Enabled = false;
                    txtTenDangNhap.Enabled = false;

                    // Mặc định ẩn mật khẩu khi load form
                    txtMatKhau.PasswordChar = '●';
                }
                else
                {
                    MessageBox.Show("Không tìm thấy thông tin cá nhân cho mã nhân viên này.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (ArgumentException argEx) // Bắt lỗi từ BLL (ví dụ: mã nhân viên trống)
            {
                MessageBox.Show(argEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // Bắt lỗi chung từ BLL hoặc DAL (qua BLL)
            {
                MessageBox.Show($"Lỗi khi tải thông tin cá nhân: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatKhau.PasswordChar = '●'; // luôn ẩn
        }

        //Xử lý sự kiện click nút "Lưu thay đổi".
        private void btnLuuThayDoi_Click(object sender, EventArgs e)
        {
            // VALIDATION
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhone(txtSoDienThoai.Text.Trim()))
            {
                MessageBox.Show("Số điện thoại chỉ chứa số và phải 10 ký tự!",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Lấy dữ liệu mới từ các control để tạo đối tượng Nhanvien và Taikhoan
            Nhanvien updatedNhanvien = new Nhanvien
            {
                Manhanvien = txtMaNhanVien.Text, // Mã nhân viên giữ nguyên
                Hoten = txtTenNhanVien.Text.Trim(),
                Ngaysinh = dtpNgaySinh.Value,
                Gioitinh = txtGioiTinh.Text.Trim(),
                Diachi = txtDiaChi.Text.Trim(),
                Sodienthoai = txtSoDienThoai.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Ngayvaolam = dtpNgayVaoLam.Value //giữ nguyên
            };

            Taikhoan updatedTaikhoan = new Taikhoan
            {
                Tendangnhap = txtTenDangNhap.Text, // Tên đăng nhập giữ nguyên
                Matkhau = txtMatKhau.Text.Trim(),
                Manhanvien = txtMaNhanVien.Text
            };

            try
            {
                // Gọi BLL để lưu dữ liệu
                bool saved = _inforBLL.SaveInforData(updatedNhanvien, updatedTaikhoan);

                if (saved)
                {
                    MessageBox.Show("Đã lưu thay đổi thông tin cá nhân thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadThongTinCaNhan(); // Tải lại thông tin để cập nhật trạng thái
                }
                else
                {
                    MessageBox.Show("Không có thay đổi nào để lưu.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (ArgumentException argEx) // Bắt lỗi validation từ BLL
            {
                MessageBox.Show(argEx.Message, "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException invEx)
            {
                MessageBox.Show(invEx.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu thay đổi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
              public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex chuẩn RFC 5322 đơn giản
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email.Trim(), pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{10}$";
            return Regex.IsMatch(phone.Trim(), pattern);
        }

        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            // Hiển thị popup nhập mật khẩu mới
            string newPassword = Interaction.InputBox(
                "Nhập mật khẩu mới:",
                "Đổi mật khẩu",
                "",
                -1, -1
            );

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Bạn chưa nhập mật khẩu mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // --- THIẾT LẬP THẲNG TRONG CODE ---
                string apiKey = "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"; //Trước khi chạy bỏ dấu ngoặc ()trên (.)
                string senderEmail = "lebao062005@gmail.com";
                string managerEmail = "baole.bit@gmail.com";

                await SendPasswordChangeRequest(_loggedInManhanvien, newPassword, apiKey, senderEmail, managerEmail);

                MessageBox.Show("Yêu cầu đổi mật khẩu đã được gửi tới quản lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi gửi yêu cầu đổi mật khẩu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm nhận API Key, sender và manager như tham số
        private async Task SendPasswordChangeRequest(string employeeId, string newPassword, string apiKey, string senderEmail, string managerEmail)
        {
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(senderEmail, "CoffeeManagement System");
            var subject = "Yêu cầu đổi mật khẩu từ nhân viên";
            var to = new EmailAddress(managerEmail, "Quản lý");
            var plainText = $"Nhân viên {employeeId} muốn đổi mật khẩu.\nMật khẩu mới đề xuất: {newPassword}";
            var htmlContent = $"<strong>Nhân viên {employeeId} muốn đổi mật khẩu.</strong><br>Mật khẩu mới đề xuất: {newPassword}";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainText, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Yêu cầu đổi mật khẩu đã được gửi tới quản lý!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                string body = await response.Body.ReadAsStringAsync();
                MessageBox.Show($"Gửi mail thất bại! Status: {response.StatusCode}\nChi tiết: {body}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
