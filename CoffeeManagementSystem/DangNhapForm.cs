using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class DangNhapForm : Form
    {
        private AuthBLL _authBLL; // Đối tượng xử lý nghiệp vụ đăng nhập (kiểm tra tài khoản)

        public DangNhapForm()
        {
            InitializeComponent();
            _authBLL = new AuthBLL(); // Khởi tạo lớp nghiệp vụ

            // Gán sự kiện click cho nút Đăng nhập
            // →Khi người dùng bấm nút sẽ thực thi hàm btnLogin_Click()
            this.btnDangNhap.Click += new EventHandler(this.btnLogin_Click);

            // Đặt ký tự hiển thị cho TextBox mật khẩu
            txtMatkhau.PasswordChar = '●';
        }

        private void txtMatkhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Lấy thông tin từ giao diện người dùng
            string tendangnhap = txtTenTaiKhoan.Text.Trim();
            string matkhau = txtMatkhau.Text.Trim();

            try
            {
                // Gọi BLL để xác thực tài khoản
                // → Trả về đối tượng Taikhoan nếu hợp lệ, ngược lại trả về null
                Taikhoan taiKhoan = _authBLL.AuthenticateUser(tendangnhap, matkhau);

                if (taiKhoan != null)
                {
                    // Lấy tên nhân viên để hiển thị lời chào
                    string tenNhanVienHienThi = _authBLL.GetEmployeeDisplayName(taiKhoan.Manhanvien, taiKhoan.Tendangnhap);

                    MessageBox.Show($"Đăng nhập thành công! Chào mừng {tenNhanVienHienThi} ({taiKhoan.Vaitro}).",
                                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Điều hướng theo vai trò tài khoản
                    if (taiKhoan.Vaitro.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        // Mở giao diện dành cho Admin
                        MainForm QuanLyForm = new MainForm(taiKhoan.Manhanvien, tenNhanVienHienThi);
                        QuanLyForm.Show();
                        this.Hide(); // Ẩn form đăng nhập
                    }
                    else if (taiKhoan.Vaitro.Trim().Equals("NhanVien", StringComparison.OrdinalIgnoreCase))
                    {
                        // Mở giao diện dành cho Nhân viên
                        MainEmployer NhanVienForm = new MainEmployer(taiKhoan.Manhanvien, tenNhanVienHienThi);
                        NhanVienForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Vai trò không hợp lệ hoặc chưa được hỗ trợ
                        MessageBox.Show($"Vai trò '{taiKhoan.Vaitro}' không được hỗ trợ.",
                                        "Lỗi vai trò", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Sai tên đăng nhập hoặc mật khẩu
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.",
                                    "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException ex)
            {
                // Lỗi dữ liệu đầu vào (ví dụ: bỏ trống tài khoản hoặc mật khẩu)
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                // Các lỗi hệ thống khác
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình đăng nhập: {ex.Message}\nChi tiết: {ex.InnerException?.Message}",
                                "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            // Khi tick vào checkbox → hiển thị mật khẩu, bỏ tick → ẩn mật khẩu
            txtMatkhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '●';
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            // Đóng form đăng nhập
            this.Close();
        }
    }
}
