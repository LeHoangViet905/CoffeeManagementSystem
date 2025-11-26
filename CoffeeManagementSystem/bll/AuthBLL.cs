// Trong file: BLL/AuthBLL.cs

using CoffeeManagementSystem.DAL;
using System;

namespace CoffeeManagementSystem.BLL
{
    public class AuthBLL
    {
        //Đây là 2 biến private của class AuthBLL, kiểu dữ liệu là TaikhoanDAL và NhanvienDAL
        //dùng để gọi tới DAL của 2 bảng
        private TaikhoanDAL _taikhoanDAL;
        private NhanvienDAL _nhanvienDAL; // Cần để lấy thông tin nhân viên
        // **LƯU Ý: Tầng BLL không trực tiếp viết truy vấn SQL, mà phải gọi DAL để làm chuyện đó. **

        // Khi AuthBLL được tạo ra, hai DAL này cũng được khởi tạo luôn → sẵn sàng để gọi dùng
        public AuthBLL()
        {
            _taikhoanDAL = new TaikhoanDAL();
            _nhanvienDAL = new NhanvienDAL();
        }
        //Tài khoản là bảng riêng → xử lý xác thực
        //Nhân viên là bảng khác → lấy thông tin hiển thị

        /// <summary>
        /// Xác thực người dùng và trả về thông tin tài khoản nếu đăng nhập thành công.
        /// </summary>
        /// <param name="tenDangNhap">Tên đăng nhập.</param>
        /// <param name="matKhau">Mật khẩu.</param>
        /// <returns>Đối tượng Taikhoan nếu đăng nhập thành công, ngược lại là null.</returns>
        /// <exception cref="ArgumentException">Ném ngoại lệ nếu tên đăng nhập hoặc mật khẩu rỗng.</exception>

        // --- xử lý đăng nhập ---
        public Taikhoan AuthenticateUser(string tenDangNhap, string matKhau) //hàm xác thực tài khoản người dùng
        {
            //Nếu người dùng để trống
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                throw new ArgumentException("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.");
            }
            // Lấy tài khoản từ DB (gồm mật khẩu đã được hash)
            var taiKhoan = _taikhoanDAL.GetTaikhoanByTendangnhap(tenDangNhap);
            if (taiKhoan != null && BCrypt.Net.BCrypt.Verify(matKhau, taiKhoan.Matkhau))
            {
                // So sánh mật khẩu người dùng nhập với hash đã lưu
                bool isValid = BCrypt.Net.BCrypt.Verify(matKhau, taiKhoan.Matkhau);
                if (isValid)
                {
                    return taiKhoan; // Xác thực thành công
                }
            }

            return null; // Sai mật khẩu hoặc không tồn tại
        }

        /// <summary>
        /// Lấy tên hiển thị của nhân viên dựa trên mã nhân viên trong tài khoản.
        /// </summary>
        /// <param name="maNhanVien">Mã nhân viên.</param>
        /// <param name="defaultName">Tên mặc định nếu không tìm thấy nhân viên.</param>
        /// <returns>Tên nhân viên hoặc tên mặc định.</returns>

        // Y như cái xử lý đăng nhập
        public string GetEmployeeDisplayName(string maNhanVien, string defaultName)
        {
            if (!string.IsNullOrEmpty(maNhanVien))
            {
                Nhanvien nhanVien = _nhanvienDAL.GetNhanvienById(maNhanVien);
                if (nhanVien != null && !string.IsNullOrEmpty(nhanVien.Hoten))
                {
                    return nhanVien.Hoten;
                }
            }
            return defaultName;
        }
    }
}