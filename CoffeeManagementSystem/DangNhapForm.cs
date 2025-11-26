using CoffeeManagementSystem.BLL;
using System;
using System.Windows.Forms;

namespace CoffeeManagementSystem
{
    public partial class DangNhapForm : Form
    {
        private AuthBLL _authBLL; // Khai báo đối tượng BLL

        public DangNhapForm()
        {
            InitializeComponent();
            _authBLL = new AuthBLL(); // Khởi tạo BLL

            // Gán sự kiện click cho nút Đăng nhập
            this.btnDangNhap.Click += new EventHandler(this.btnLogin_Click);

            // Tùy chọn: Xử lý sự kiện KeyDown trên TextBox mật khẩu để nhấn Enter cũng đăng nhập
            //this.txtMatkhau.KeyDown += new KeyEventHandler(this.txtMatkhau_KeyDown);

            // Đặt PasswordChar mặc định khi khởi tạo
            txtMatkhau.PasswordChar = '●';
        }

        private void txtMatkhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e); //giả lập bấm nút Đăng nhập!
                e.Handled = true; //Tránh việc Enter bị xử lý thêm ở chỗ khác.
                e.SuppressKeyPress = true; //Không cho tiếng “ting” hoặc dấu xuống dòng xảy ra trong textbox khi Enter được bấm.
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string tendangnhap = txtTenTaiKhoan.Text.Trim();
            string matkhau = txtMatkhau.Text.Trim();


            //Dùng try để bắt lỗi. Nếu có lỗi, chương trình không bị sập mà sẽ xử lý trong catch.
            try
            {
                // Gọi BLL để xác thực người dùng
                Taikhoan taiKhoan = _authBLL.AuthenticateUser(tendangnhap, matkhau);
                //Kiểm tra xem kết quả có hợp lệ không.
                if (taiKhoan != null)
                {
                    // Lấy tên hiển thị từ BLL
                    string tenNhanVienHienThi = _authBLL.GetEmployeeDisplayName(taiKhoan.Manhanvien, taiKhoan.Tendangnhap);

                    // Đăng nhập thành công
                    MessageBox.Show($"Đăng nhập thành công! Chào mừng {tenNhanVienHienThi} ({taiKhoan.Vaitro}).", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Xác định vai trò và hiển thị Main Form tương ứng
                    // SỬ DỤNG TRIM() VÀ EQUALS(..., StringComparison.OrdinalIgnoreCase) ĐỂ SO SÁNH VAI TRÒ LINH HOẠT HƠN
                    //Trim(): xóa khoảng trắng dư
                    //Equals(..., StringComparison.OrdinalIgnoreCase): so sánh không phân biệt chữ hoa thường
                    if (taiKhoan.Vaitro.Trim().Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        MainForm QuanLyForm = new MainForm(taiKhoan.Manhanvien, tenNhanVienHienThi);
                        QuanLyForm.Show();
                        this.Hide();
                    }
                    else if (taiKhoan.Vaitro.Trim().Equals("NhanVien", StringComparison.OrdinalIgnoreCase))
                    {
                        MainEmployer NhanVienForm = new MainEmployer(taiKhoan.Manhanvien, tenNhanVienHienThi);
                        NhanVienForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        // Vai trò không được hỗ trợ, ngay cả sau khi đã xử lý khoảng trắng và chữ hoa/thường
                        MessageBox.Show($"Vai trò '{taiKhoan.Vaitro}' không được hỗ trợ. Vui lòng liên hệ quản trị viên.", "Lỗi vai trò", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Tài khoản không tồn tại hoặc sai mật khẩu (BLL đã trả về null)
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentException ex) // Bắt lỗi từ BLL nếu người dùng nhập thiếu
            {
                MessageBox.Show(ex.Message, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi trong quá trình đăng nhập: {ex.Message}\nChi tiết: {ex.InnerException?.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            txtMatkhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '●';
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtTenTaiKhoan_TextChanged(object sender, EventArgs e)
        {

        }
    }
}