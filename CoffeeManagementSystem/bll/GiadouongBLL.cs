using CoffeeManagementSystem.DAL; // Để gọi các phương thức DAL
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
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
                // Logic nghiệp vụ (nếu có) có thể được thêm ở đây trước hoặc sau khi gọi DAL.
                return _giadouongDAL.GetAllGiadouongs();
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ DAL và ném lại một lỗi nghiệp vụ/tầng BLL cụ thể hơn
                throw new InvalidOperationException($"Lỗi BLL khi lấy tất cả giá đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy giá mới nhất cho một đồ uống cụ thể.
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
                throw new InvalidOperationException($"Lỗi BLL khi lấy giá mới nhất cho đồ uống '{madouong}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm một bản ghi giá đồ uống mới.
        /// </summary>
        /// <param name="giadouong">Đối tượng Giadouong cần thêm.</param>
        /// <exception cref="ArgumentException">Ném ra nếu đối tượng Giadouong không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra nếu có lỗi nghiệp vụ hoặc truy vấn.</exception>
        public void AddGiadouong(Giadouong giadouong)
        {
            // Logic nghiệp vụ: Kiểm tra tính hợp lệ của dữ liệu trước khi gọi DAL
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
            // Thêm các kiểm tra khác nếu cần (ví dụ: ngày áp dụng không trong tương lai xa, vv.)

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
        /// Phương thức này được AddDrinkForm gọi để hiển thị giá hiện tại.
        /// </summary>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Giá hiện tại của đồ uống, trả về 0 nếu không tìm thấy hoặc có lỗi.</returns>
        public decimal GetCurrentGia(string madouong)
        {
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống khi lấy giá hiện tại.", nameof(madouong));
            }

            try
            {
                // Gọi phương thức GetLatestGiaByMadouong đã có sẵn để lấy đối tượng giá mới nhất
                Giadouong latestGia = GetLatestGiaByMadouong(madouong);
                return latestGia?.Giaban ?? 0; // Trả về Giaban nếu có, ngược lại là 0
            }
            catch (Exception ex)
            {
                // Log lỗi và ném lại lỗi tầng BLL
                throw new InvalidOperationException($"Lỗi BLL khi lấy giá hiện tại của đồ uống '{madouong}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Helper method để tạo ID duy nhất cho Magia.
        /// Phương thức này được Form hoặc BLL khác gọi khi cần tạo Magia mới.
        /// </summary>
        public string GenerateNewGiadouongId()
        {
            // Logic tạo ID
            return "GIA" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        // Tùy chọn: Thêm các phương thức UpdateGiadouong và DeleteGiadouong nếu có nhu cầu nghiệp vụ.
        // Tuy nhiên, thường thì giá sẽ được thêm mới chứ ít khi cập nhật hoặc xóa trực tiếp.
    }
}