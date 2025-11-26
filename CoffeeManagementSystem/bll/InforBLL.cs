// Đảm bảo using namespace chứa các Model và DAL của bạn
using CoffeeManagementSystem.DAL;
using System;

namespace CoffeeManagementSystem.BLL // Đặt BLL trong một namespace con để tổ chức code tốt hơn
{
    public class InforBLL
    {
        private NhanvienDAL _nhanvienDAL;
        private TaikhoanDAL _taikhoanDAL;
        private string _loggedInManhanvien;

        // Dữ liệu gốc được lưu trữ trong BLL để so sánh và thao tác
        private Nhanvien _originalNhanvien;
        private Taikhoan _originalTaikhoan;

        public InforBLL(string manhanvien)
        {
            _loggedInManhanvien = manhanvien;
            _nhanvienDAL = new NhanvienDAL();
            _taikhoanDAL = new TaikhoanDAL();
        }

        /// <summary>
        /// Tải thông tin cá nhân và tài khoản.
        /// </summary>
        /// <returns>Tuple chứa Nhanvien và Taikhoan nếu thành công, ngược lại trả về null.</returns>
        public Tuple<Nhanvien, Taikhoan> LoadInforData()
        {
            if (string.IsNullOrEmpty(_loggedInManhanvien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }

            try
            {
                _originalNhanvien = _nhanvienDAL.GetNhanvienById(_loggedInManhanvien);
                _originalTaikhoan = _taikhoanDAL.GetTaikhoanByManhanvien(_loggedInManhanvien);

                if (_originalNhanvien != null && _originalTaikhoan != null)
                {
                    return new Tuple<Nhanvien, Taikhoan>(_originalNhanvien, _originalTaikhoan);
                }
                else
                {
                    return null; // Không tìm thấy thông tin
                }
            }
            catch (Exception ex)
            {
                // BLL xử lý lỗi từ DAL hoặc ném lại lỗi để UI xử lý (ví dụ: hiển thị MessageBox)
                throw new Exception($"Lỗi BLL khi tải thông tin: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu thay đổi thông tin cá nhân và tài khoản.
        /// </summary>
        /// <param name="updatedNhanvien">Đối tượng Nhanvien với thông tin mới.</param>
        /// <param name="updatedTaikhoan">Đối tượng Taikhoan với thông tin mới.</param>
        /// <returns>True nếu có thay đổi và lưu thành công, False nếu không có thay đổi.</returns>
        public bool SaveInforData(Nhanvien updatedNhanvien, Taikhoan updatedTaikhoan)
        {
            if (_originalNhanvien == null || _originalTaikhoan == null)
            {
                throw new InvalidOperationException("Không có dữ liệu gốc để so sánh và lưu. Vui lòng tải thông tin trước.");
            }

            // --- BẮT ĐẦU LOGIC NGHIỆP VỤ VÀ XÁC THỰC DỮ LIỆU ---
            if (string.IsNullOrWhiteSpace(updatedNhanvien.Hoten) || string.IsNullOrWhiteSpace(updatedNhanvien.Gioitinh) || string.IsNullOrWhiteSpace(updatedNhanvien.Diachi))
            {
                throw new ArgumentException("Họ tên, Giới tính và Địa chỉ không được để trống.");
            }

            try
            {
                bool nhanvienChanged = false;
                bool taikhoanChanged = false;

                // So sánh dữ liệu mới với dữ liệu gốc để quyết định có cần cập nhật không
                if (_originalNhanvien.Hoten != updatedNhanvien.Hoten ||
                    _originalNhanvien.Ngaysinh != updatedNhanvien.Ngaysinh ||
                    _originalNhanvien.Gioitinh != updatedNhanvien.Gioitinh ||
                    _originalNhanvien.Diachi != updatedNhanvien.Diachi ||
                    _originalNhanvien.Sodienthoai != updatedNhanvien.Sodienthoai ||
                    _originalNhanvien.Email != updatedNhanvien.Email)
                {
                    // Cập nhật thông tin trên đối tượng _originalNhanvien để sau này có thể dùng để so sánh tiếp
                    _originalNhanvien.Hoten = updatedNhanvien.Hoten;
                    _originalNhanvien.Ngaysinh = updatedNhanvien.Ngaysinh;
                    _originalNhanvien.Gioitinh = updatedNhanvien.Gioitinh;
                    _originalNhanvien.Diachi = updatedNhanvien.Diachi;
                    _originalNhanvien.Sodienthoai = updatedNhanvien.Sodienthoai;
                    _originalNhanvien.Email = updatedNhanvien.Email;

                    _nhanvienDAL.UpdateNhanvien(_originalNhanvien);
                    nhanvienChanged = true;
                }

                if (_originalTaikhoan.Matkhau != updatedTaikhoan.Matkhau)
                {
                    _originalTaikhoan.Matkhau = updatedTaikhoan.Matkhau;
                    _taikhoanDAL.UpdateTaikhoan(_originalTaikhoan);
                    taikhoanChanged = true;
                }

                return nhanvienChanged || taikhoanChanged; // Trả về true nếu có bất kỳ thay đổi nào được lưu
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lưu thông tin: {ex.Message}", ex);
            }
        }
    }
}
