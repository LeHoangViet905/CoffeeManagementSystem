using CoffeeManagementSystem.BLL;
using System;
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

            // Gán sự kiện cho checkbox Hiện mật khẩu
            this.chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
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
                    txtMatKhau.Text = taikhoan.Matkhau;

                    // Vô hiệu hóa các trường không cho phép chỉnh sửa (Logic UI)
                    txtMaNhanVien.Enabled = false;
                    dtpNgayVaoLam.Enabled = false;
                    txtTenDangNhap.Enabled = false;

                    // Mặc định ẩn mật khẩu khi load form
                    chkHienMatKhau.Checked = false;
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

        // Xử lý checkbox ẩn/hiện mật khẩu
        private void chkHienMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            // Nếu được tick thì hiện mật khẩu, bỏ tick thì ẩn
            txtMatKhau.PasswordChar = chkHienMatKhau.Checked ? '\0' : '●';
        }

        // Xử lý sự kiện click nút "Lưu thay đổi".
        private void btnLuuThayDoi_Click(object sender, EventArgs e)
        {
            // VALIDATION TẠI FORM
            if (!IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com",
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!IsValidPhone(txtSoDienThoai.Text.Trim()))
            {
                MessageBox.Show("Số điện thoại chỉ chứa số và phải từ 8-12 ký tự!",
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
        private bool IsValidEmail(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$"
            );
        }

        private bool IsValidPhone(string phone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(
                phone,
                @"^[0-9]{8,12}$"
            );
        }
    }
    }
