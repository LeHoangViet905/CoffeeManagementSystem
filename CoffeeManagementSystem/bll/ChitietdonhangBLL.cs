using CoffeeManagementSystem.DAL;
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    public class ChitietdonhangBLL
    {
        private ChitietdonhangDAL _chitietdonhangDAL;

        public ChitietdonhangBLL()
        {
            _chitietdonhangDAL = new ChitietdonhangDAL();
        }

        /// <summary>
        /// Thêm một chi tiết đơn hàng mới.
        /// </summary>
        /// <param name="chitiet">Đối tượng Chitietdonhang cần thêm.</param>
        /// <returns>True nếu thêm thành công.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu dữ liệu chi tiết đơn hàng không hợp lệ.</exception>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public bool AddChitietdonhang(Chitietdonhang chitiet)
        {
            // 1. Kiểm tra ràng buộc nghiệp vụ
            if (chitiet == null)
            {
                throw new ArgumentNullException(nameof(chitiet), "Đối tượng chi tiết đơn hàng không được null.");
            }
            if (string.IsNullOrWhiteSpace(chitiet.Madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống.", nameof(chitiet.Madonhang));
            }
            if (string.IsNullOrWhiteSpace(chitiet.Madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(chitiet.Madouong));
            }
            if (chitiet.Soluong <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.", nameof(chitiet.Soluong));
            }
            if (chitiet.Dongia < 0)
            {
                throw new ArgumentException("Đơn giá không thể là số âm.", nameof(chitiet.Dongia));
            }
            if (chitiet.Thanhtien < 0)
            {
                throw new ArgumentException("Thành tiền không thể là số âm.", nameof(chitiet.Thanhtien));
            }
            // Có thể thêm kiểm tra tính nhất quán giữa Soluong * Dongia và Thanhtien

            try
            {
                // 2. Gọi DAL để thực hiện thao tác
                _chitietdonhangDAL.AddChitietdonhang(chitiet);
                return true;
            }
            catch (Exception ex)
            {
                // 3. Xử lý lỗi (log lỗi, ném lại lỗi với thông tin rõ ràng hơn)
                throw new Exception($"Lỗi BLL khi thêm chi tiết đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả các chi tiết đơn hàng.
        /// </summary>
        /// <returns>Danh sách các đối tượng Chitietdonhang.</returns>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public List<Chitietdonhang> GetAllChitietdonhangs()
        {
            try
            {
                return _chitietdonhangDAL.GetAllChitietdonhangs();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy danh sách chi tiết đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy một chi tiết đơn hàng cụ thể bằng khóa chính kép (Madonhang, Madouong).
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng.</param>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Đối tượng Chitietdonhang nếu tìm thấy, ngược lại null.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu mã đơn hàng hoặc mã đồ uống không hợp lệ.</exception>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public Chitietdonhang GetChitietdonhangByIds(string madonhang, string madouong)
        {
            // 1. Kiểm tra ràng buộc nghiệp vụ
            if (string.IsNullOrWhiteSpace(madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống.", nameof(madonhang));
            }
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(madouong));
            }

            try
            {
                // 2. Gọi DAL để thực hiện thao tác
                return _chitietdonhangDAL.GetChitietdonhangByIds(madonhang, madouong);
            }
            catch (Exception ex)
            {
                // 3. Xử lý lỗi
                throw new Exception($"Lỗi BLL khi lấy chi tiết đơn hàng (ĐH: {madonhang}, ĐU: {madouong}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin của một chi tiết đơn hàng.
        /// </summary>
        /// <param name="chitiet">Đối tượng Chitietdonhang chứa thông tin cập nhật.</param>
        /// <returns>True nếu cập nhật thành công.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu dữ liệu chi tiết đơn hàng không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra nếu chi tiết đơn hàng không tồn tại để cập nhật.</exception>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public bool UpdateChitietdonhang(Chitietdonhang chitiet)
        {
            // 1. Kiểm tra ràng buộc nghiệp vụ
            if (chitiet == null)
            {
                throw new ArgumentNullException(nameof(chitiet), "Đối tượng chi tiết đơn hàng không được null.");
            }
            if (string.IsNullOrWhiteSpace(chitiet.Madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống khi cập nhật.", nameof(chitiet.Madonhang));
            }
            if (string.IsNullOrWhiteSpace(chitiet.Madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống khi cập nhật.", nameof(chitiet.Madouong));
            }
            if (chitiet.Soluong <= 0)
            {
                throw new ArgumentException("Số lượng phải lớn hơn 0.", nameof(chitiet.Soluong));
            }
            if (chitiet.Dongia < 0)
            {
                throw new ArgumentException("Đơn giá không thể là số âm.", nameof(chitiet.Dongia));
            }
            if (chitiet.Thanhtien < 0)
            {
                throw new ArgumentException("Thành tiền không thể là số âm.", nameof(chitiet.Thanhtien));
            }

            try
            {
                // 2. Kiểm tra sự tồn tại của chi tiết đơn hàng trước khi cập nhật
                Chitietdonhang existingChitiet = _chitietdonhangDAL.GetChitietdonhangByIds(chitiet.Madonhang, chitiet.Madouong);
                if (existingChitiet == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy chi tiết đơn hàng với Mã đơn hàng '{chitiet.Madonhang}' và Mã đồ uống '{chitiet.Madouong}' để cập nhật.");
                }

                // 3. Gọi DAL để thực hiện thao tác
                _chitietdonhangDAL.UpdateChitietdonhang(chitiet);
                return true;
            }
            catch (Exception ex)
            {
                // 4. Xử lý lỗi
                throw new Exception($"Lỗi BLL khi cập nhật chi tiết đơn hàng (ĐH: {chitiet.Madonhang}, ĐU: {chitiet.Madouong}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa một chi tiết đơn hàng khỏi cơ sở dữ liệu.
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng của chi tiết cần xóa.</param>
        /// <param name="madouong">Mã đồ uống của chi tiết cần xóa.</param>
        /// <returns>True nếu xóa thành công.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu mã đơn hàng hoặc mã đồ uống không hợp lệ.</exception>
        /// <exception cref="InvalidOperationException">Ném ra nếu chi tiết đơn hàng không tồn tại để xóa.</exception>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public bool DeleteChitietdonhang(string madonhang, string madouong)
        {
            // 1. Kiểm tra ràng buộc nghiệp vụ
            if (string.IsNullOrWhiteSpace(madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được trống.", nameof(madonhang));
            }
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được trống.", nameof(madouong));
            }

            try
            {
                // 2. Kiểm tra sự tồn tại của chi tiết đơn hàng trước khi xóa
                Chitietdonhang existingChitiet = _chitietdonhangDAL.GetChitietdonhangByIds(madonhang, madouong);
                if (existingChitiet == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy chi tiết đơn hàng với Mã đơn hàng '{madonhang}' và Mã đồ uống '{madouong}' để xóa.");
                }

                // 3. Gọi DAL để thực hiện thao tác
                _chitietdonhangDAL.DeleteChitietdonhang(madonhang, madouong);
                return true;
            }
            catch (Exception ex)
            {
                // 4. Xử lý lỗi
                throw new Exception($"Lỗi BLL khi xóa chi tiết đơn hàng (ĐH: {madonhang}, ĐU: {madouong}): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi tiết đơn hàng theo Mã đơn hàng.
        /// Thêm phương thức này nếu bạn cần lấy tất cả các mặt hàng cho một đơn hàng cụ thể.
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng cần lấy chi tiết.</param>
        /// <returns>Danh sách các đối tượng Chitietdonhang thuộc đơn hàng đó.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu mã đơn hàng không hợp lệ.</exception>
        /// <exception cref="Exception">Ném ra nếu có lỗi trong quá trình DAL.</exception>
        public List<Chitietdonhang> GetChitietdonhangsByMadonhang(string madonhang)
        {
            if (string.IsNullOrWhiteSpace(madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống.", nameof(madonhang));
            }

            try
            {
                return _chitietdonhangDAL.GetAllChitietdonhangs()
                                       .FindAll(c => c.Madonhang == madonhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy chi tiết đơn hàng theo Mã đơn hàng '{madonhang}': {ex.Message}", ex);
            }
        }
    }
}