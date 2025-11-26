// using System.Linq; // Không cần thiết nếu không dùng LINQ mở rộng
// using System.Windows.Forms; // BỎ DÒNG NÀY ĐI - KHÔNG ĐƯỢC PHÉP DÙNG TRONG BLL

// Đảm bảo các namespace này khớp với cấu trúc dự án của bạn
using CoffeeManagementSystem.DAL; // Tham chiếu đến DAL của bạn
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    public class LoaidouongBLL
    {
        private LoaidouongDAL _loaidouongDAL;

        public LoaidouongBLL()
        {
            _loaidouongDAL = new LoaidouongDAL();
        }

        /// <summary>
        /// Retrieves all drink categories.
        /// Includes basic error handling and can incorporate business rules.
        /// </summary>
        /// <returns>A list of Loaidouong objects.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public List<Loaidouong> GetAllLoaidouongs()
        {
            try
            {
                // Bạn có thể thêm các quy tắc nghiệp vụ ở đây trước khi gọi DAL, ví dụ:
                // kiểm tra quyền người dùng, caching, v.v.
                return _loaidouongDAL.GetAllLoaidouongs();
            }
            catch (Exception ex)
            {
                // Ném ngoại lệ để tầng UI xử lý
                throw new Exception($"Lỗi BLL khi lấy danh sách loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves a drink category by its ID.
        /// </summary>
        /// <param name="maloai">The ID of the drink category.</param>
        /// <returns>A Loaidouong object if found, otherwise null.</returns>
        /// <exception cref="ArgumentException">Throws if maloai is null or whitespace.</exception>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public Loaidouong GetLoaidouongById(string maloai)
        {
            // Xác thực cơ bản
            if (string.IsNullOrWhiteSpace(maloai))
            {
                // Ném ngoại lệ ArgumentException để tầng UI bắt và hiển thị
                throw new ArgumentException("Mã loại đồ uống không được để trống.", nameof(maloai));
            }

            try
            {
                return _loaidouongDAL.GetLoaidouongById(maloai);
            }
            catch (Exception ex)
            {
                // Ném ngoại lệ để tầng UI xử lý
                throw new Exception($"Lỗi BLL khi lấy loại đồ uống theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new drink category to the system.
        /// Includes business validation for Maloai and Tenloai.
        /// </summary>
        /// <param name="loaidouong">The Loaidouong object to add.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentNullException">Throws if loaidouong object is null.</exception>
        /// <exception cref="ArgumentException">Throws if Maloai or Tenloai are null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws if a business rule is violated (e.g., Maloai already exists).</exception>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public bool AddLoaidouong(Loaidouong loaidouong)
        {
            // Xác thực nghiệp vụ: Kiểm tra xem Maloai và Tenloai có được cung cấp và không chỉ là khoảng trắng
            if (loaidouong == null)
            {
                throw new ArgumentNullException(nameof(loaidouong), "Đối tượng loại đồ uống không thể rỗng.");
            }
            if (string.IsNullOrWhiteSpace(loaidouong.Maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống.", nameof(loaidouong));
            }
            if (string.IsNullOrWhiteSpace(loaidouong.Tenloai))
            {
                throw new ArgumentException("Tên loại đồ uống không được để trống.", nameof(loaidouong));
            }

            // Bạn có thể thêm các quy tắc nghiệp vụ phức tạp hơn ở đây, ví dụ:
            // - Kiểm tra xem Maloai đã tồn tại trước khi thêm không
            if (_loaidouongDAL.GetLoaidouongById(loaidouong.Maloai) != null)
            {
                throw new InvalidOperationException($"Mã loại đồ uống '{loaidouong.Maloai}' đã tồn tại. Vui lòng chọn mã khác.");
            }

            try
            {
                _loaidouongDAL.AddLoaidouong(loaidouong);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi thêm loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates an existing drink category.
        /// Includes business validation.
        /// </summary>
        /// <param name="loaidouong">The Loaidouong object with updated information.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentNullException">Throws if loaidouong object is null.</exception>
        /// <exception cref="ArgumentException">Throws if Maloai or Tenloai are null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws if the category to update does not exist.</exception>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public bool UpdateLoaidouong(Loaidouong loaidouong)
        {
            // Xác thực nghiệp vụ
            if (loaidouong == null)
            {
                throw new ArgumentNullException(nameof(loaidouong), "Đối tượng loại đồ uống không thể rỗng.");
            }
            if (string.IsNullOrWhiteSpace(loaidouong.Maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống khi cập nhật.", nameof(loaidouong));
            }
            if (string.IsNullOrWhiteSpace(loaidouong.Tenloai))
            {
                throw new ArgumentException("Tên loại đồ uống không được để trống.", nameof(loaidouong));
            }

            // Bạn có thể thêm một kiểm tra ở đây để đảm bảo Maloai tồn tại trước khi cố gắng cập nhật
            if (_loaidouongDAL.GetLoaidouongById(loaidouong.Maloai) == null)
            {
                throw new InvalidOperationException($"Loại đồ uống có mã '{loaidouong.Maloai}' cần cập nhật không tồn tại.");
            }

            try
            {
                _loaidouongDAL.UpdateLoaidouong(loaidouong);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi cập nhật loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a drink category by its ID.
        /// </summary>
        /// <param name="maloai">The ID of the drink category to delete.</param>
        /// <returns>True if the operation was successful.</returns>
        /// <exception cref="ArgumentException">Throws if maloai is null or whitespace.</exception>
        /// <exception cref="InvalidOperationException">Throws if business rules prevent deletion (e.g., dependencies).</exception>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public bool DeleteLoaidouong(string maloai)
        {
            // Xác thực cơ bản
            if (string.IsNullOrWhiteSpace(maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống khi xóa.", nameof(maloai));
            }

            // Quy tắc nghiệp vụ: Kiểm tra các phụ thuộc trước khi xóa (ví dụ: nếu có đồ uống nào được liên kết với loại này)
            // Điều này sẽ yêu cầu một phương thức DAL để kiểm tra các phụ thuộc trong bảng 'Douong'.
            // Ví dụ:
            // if (_douongDAL.DoesCategoryHaveDrinks(maloai)) // Giả sử có một DouongDAL tồn tại
            // {
            //     throw new InvalidOperationException("Không thể xóa loại đồ uống này vì có đồ uống đang sử dụng.");
            // }

            try
            {
                _loaidouongDAL.DeleteLoaidouong(maloai);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi xóa loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches for drink categories based on a keyword.
        /// </summary>
        /// <param name="searchTerm">The keyword to search for.</param>
        /// <returns>A list of matching Loaidouong objects.</returns>
        /// <exception cref="Exception">Throws a general exception if an error occurs in DAL.</exception>
        public List<Loaidouong> SearchLoaidouongs(string searchTerm)
        {
            try
            {
                // DAL đã xử lý thuật ngữ tìm kiếm rỗng/khoảng trắng.
                return _loaidouongDAL.SearchLoaidouongs(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi tìm kiếm loại đồ uống: {ex.Message}", ex);
            }
        }
    }
}