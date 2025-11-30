using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    public class KhuyenmaiBLL
    {
        private readonly KhuyenmaiDAL _khuyenmaiDAL;

        public KhuyenmaiBLL()
        {
            _khuyenmaiDAL = new KhuyenmaiDAL();
        }

        /// <summary>
        /// Thêm 1 chương trình khuyến mãi mới.
        /// </summary>
        public bool AddKhuyenmai(Khuyenmai km)
        {
            // 1. Kiểm tra ràng buộc nghiệp vụ
            if (km == null)
                throw new ArgumentNullException(nameof(km), "Đối tượng khuyến mãi không được null.");

            if (string.IsNullOrWhiteSpace(km.MaKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống.", nameof(km.MaKM));

            if (string.IsNullOrWhiteSpace(km.TenChuongTrinh))
                throw new ArgumentException("Tên chương trình không được để trống.", nameof(km.TenChuongTrinh));

            if (km.NgayKetThuc <= km.NgayBatDau)
                throw new ArgumentException("Ngày kết thúc phải lớn hơn ngày bắt đầu.");

            if (km.PhanTramGiam <= 0 || km.PhanTramGiam > 50)
                throw new ArgumentException("Phần trăm giảm phải > 0 và ≤ 50.", nameof(km.PhanTramGiam));

            // Tính trạng thái tự động nếu bạn muốn
            km.TrangThai = TinhTrangTuNgay(km.NgayBatDau, km.NgayKetThuc);

            try
            {
                // 2. Kiểm tra trùng mã
                var existing = _khuyenmaiDAL.GetKhuyenmaiById(km.MaKM);
                if (existing != null)
                    throw new InvalidOperationException($"Mã khuyến mãi '{km.MaKM}' đã tồn tại.");

                // 3. Gọi DAL thêm mới
                _khuyenmaiDAL.InsertKhuyenmai(km);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi thêm khuyến mãi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chương trình khuyến mãi.
        /// </summary>
        public List<Khuyenmai> GetAllKhuyenmai()
        {
            try
            {
                return _khuyenmaiDAL.GetAllKhuyenmai();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy danh sách khuyến mãi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy 1 chương trình khuyến mãi theo mã.
        /// </summary>
        public Khuyenmai GetKhuyenmaiById(string maKM)
        {
            if (string.IsNullOrWhiteSpace(maKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống.", nameof(maKM));

            try
            {
                return _khuyenmaiDAL.GetKhuyenmaiById(maKM);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy khuyến mãi '{maKM}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin 1 chương trình khuyến mãi.
        /// </summary>
        public bool UpdateKhuyenmai(Khuyenmai km)
        {
            // 1. Validate
            if (km == null)
                throw new ArgumentNullException(nameof(km), "Đối tượng khuyến mãi không được null.");

            if (string.IsNullOrWhiteSpace(km.MaKM))
                throw new ArgumentException("Mã khuyến mãi không được để trống khi cập nhật.", nameof(km.MaKM));

            if (string.IsNullOrWhiteSpace(km.TenChuongTrinh))
                throw new ArgumentException("Tên chương trình không được để trống.", nameof(km.TenChuongTrinh));

            if (km.NgayKetThuc <= km.NgayBatDau)
                throw new ArgumentException("Ngày kết thúc phải lớn hơn ngày bắt đầu.");

            if (km.PhanTramGiam <= 0 || km.PhanTramGiam > 50)
                throw new ArgumentException("Phần trăm giảm phải > 0 và ≤ 50.", nameof(km.PhanTramGiam));

            km.TrangThai = TinhTrangTuNgay(km.NgayBatDau, km.NgayKetThuc);

            try
            {
                // 2. Kiểm tra tồn tại
                var existing = _khuyenmaiDAL.GetKhuyenmaiById(km.MaKM);
                if (existing == null)
                    throw new InvalidOperationException($"Không tìm thấy khuyến mãi với mã '{km.MaKM}' để cập nhật.");

                // 3. Gọi DAL
                _khuyenmaiDAL.UpdateKhuyenmai(km);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi cập nhật khuyến mãi '{km.MaKM}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa 1 chương trình khuyến mãi.
        /// </summary>
        public bool DeleteKhuyenmai(string maKM)
        {
            if (string.IsNullOrWhiteSpace(maKM))
                throw new ArgumentException("Mã khuyến mãi không được trống.", nameof(maKM));

            try
            {
                var existing = _khuyenmaiDAL.GetKhuyenmaiById(maKM);
                if (existing == null)
                    throw new InvalidOperationException($"Không tìm thấy khuyến mãi với mã '{maKM}' để xóa.");

                _khuyenmaiDAL.DeleteKhuyenmai(maKM);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi xóa khuyến mãi '{maKM}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lọc theo trạng thái (0: Sắp diễn ra, 1: Đang diễn ra, 2: Đã kết thúc).
        /// </summary>
        public List<Khuyenmai> GetKhuyenmaiByTrangThai(int trangThai)
        {
            if (trangThai < 0 || trangThai > 2)
                throw new ArgumentException("Trạng thái không hợp lệ. (0,1,2).", nameof(trangThai));

            try
            {
                var all = _khuyenmaiDAL.GetAllKhuyenmai();
                return all.FindAll(k => k.TrangThai == trangThai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy khuyến mãi theo trạng thái {trangThai}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tự tính trạng thái dựa trên ngày bắt đầu và kết thúc.
        /// </summary>
        private int TinhTrangTuNgay(DateTime start, DateTime end)
        {
            var now = DateTime.Now;
            if (now < start) return 0; // Sắp diễn ra
            if (now > end) return 2; // Đã kết thúc
            return 1;                  // Đang diễn ra
        }
    }
}
