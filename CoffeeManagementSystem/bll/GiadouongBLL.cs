using CoffeeManagementSystem.DAL; // Để gọi các phương thức làm việc với CSDL ở tầng DAL
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// Lớp nghiệp vụ cho bảng Giadouong.
    /// Trung gian giữa UI/BLL khác và GiadouongDAL.
    /// </summary>
    public class GiadouongBLL
    {
        private GiadouongDAL _giadouongDAL;

        public GiadouongBLL()
        {
            _giadouongDAL = new GiadouongDAL();
        }

        /// <summary>
        /// Lấy tất cả các bản ghi giá đồ uống.
        /// </summary>
        /// <returns>Danh sách các đối tượng Giadouong.</returns>
        /// <exception cref="InvalidOperationException">Ném ra nếu có lỗi nghiệp vụ hoặc truy vấn.</exception>
        public List<Giadouong> GetAllGiadouongs()
        {
            try
            {
                // Có thể bổ sung thêm logic nghiệp vụ tại đây nếu cần
                return _giadouongDAL.GetAllGiadouongs();
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ DAL và ném lại lỗi ở tầng BLL với thông điệp rõ ràng hơn
                throw new InvalidOperationException($"Lỗi BLL khi lấy tất cả giá đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy bản ghi giá mới nhất cho một đồ uống.
        /// </summary>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Đối tượng Giadouong có giá mới nhất, hoặc null nếu không tìm thấy.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu mã đồ uống không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra nếu có lỗi nghiệp vụ hoặc truy vấn.</exception>
        public Giadouong GetLatestGiaByMadouong(string madouong)
        {
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(madouong));
            }

            try
            {
                return _giadouongDAL.GetLatestGiaByMadouong(madouong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Lỗi BLL khi lấy giá mới nhất cho đồ uống '{madouong}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm một bản ghi giá đồ uống mới.
        /// Thực hiện kiểm tra dữ liệu trước khi gọi DAL.
        /// </summary>
        /// <param name="giadouong">Đối tượng Giadouong cần thêm.</param>
        /// <exception cref="ArgumentException">Ném ra nếu đối tượng Giadouong không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra nếu có lỗi nghiệp vụ hoặc truy vấn.</exception>
        public void AddGiadouong(Giadouong giadouong)
        {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (giadouong == null)
            {
                throw new ArgumentNullException(nameof(giadouong), "Đối tượng giá đồ uống không được null.");
            }
            if (string.IsNullOrWhiteSpace(giadouong.Magia))
            {
                throw new ArgumentException("Mã giá không được để trống.", nameof(giadouong.Magia));
            }
            if (string.IsNullOrWhiteSpace(giadouong.Madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(giadouong.Madouong));
            }
            if (giadouong.Giaban < 0)
            {
                throw new ArgumentException("Giá bán không được là số âm.", nameof(giadouong.Giaban));
            }
            // Có thể thêm các ràng buộc khác: ngày áp dụng, trùng Magia, v.v.

            try
            {
                _giadouongDAL.AddGiadouong(giadouong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi BLL khi thêm giá đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá hiện tại (giá bán) của một đồ uống.
        /// Thường dùng để hiển thị giá ở Form (dựa trên bản ghi giá mới nhất).
        /// </summary>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Giá hiện tại của đồ uống, trả về 0 nếu không tìm thấy.</returns>
        public decimal GetCurrentGia(string madouong)
        {
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống khi lấy giá hiện tại.", nameof(madouong));
            }

            try
            {
                // Dùng lại hàm lấy bản ghi giá mới nhất
                Giadouong latestGia = GetLatestGiaByMadouong(madouong);
                return latestGia?.Giaban ?? 0; // Nếu null thì trả 0
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Lỗi BLL khi lấy giá hiện tại của đồ uống '{madouong}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo ID mới cho bảng giá (Magia) theo timestamp.
        /// Được gọi từ Form hoặc BLL khác khi cần tạo Magia.
        /// </summary>
        public string GenerateNewGiadouongId()
        {
            // Ví dụ: GIA20251201123456789
            return "GIA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// Lấy danh sách giá hiện tại (mới nhất) của tất cả đồ uống.
        /// Thích hợp cho báo cáo hoặc màn hình tổng hợp giá.
        /// </summary>
        /// <returns>Danh sách Giadouong đại diện cho giá hiện tại.</returns>
        public List<Giadouong> GetAllCurrentPrices()
        {
            // BLL chỉ đơn giản là gọi hàm tương ứng ở DAL,
            // có thể bổ sung logic nghiệp vụ (lọc, sắp xếp, ...) nếu cần.
            return _giadouongDAL.GetAllCurrentPrices();
        }

        /// <summary>
        /// Tìm kiếm toàn bộ đồ uống kèm giá (nếu DAL join sẵn).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tên, mã,...).</param>
        /// <returns>Danh sách Douong đã qua xử lý search.</returns>
        public List<Douong> SearchAllDouongs(string searchTerm)
        {
            // Ở đây BLL ủy quyền hoàn toàn cho DAL xử lý truy vấn tìm kiếm.
            return _giadouongDAL.SearchAllDouongs(searchTerm);
        }
    }
}
