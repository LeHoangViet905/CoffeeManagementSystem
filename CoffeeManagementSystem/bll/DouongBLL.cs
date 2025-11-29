using CoffeeManagementSystem.DAL; // Import DAL namespace
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    public class DouongBLL
    {
        private DouongDAL _douongDAL;

        public DouongBLL()
        {
            _douongDAL = new DouongDAL();
        }

        /// <summary>
        /// Lấy tất cả đồ uống từ CSDL và điền giá mới nhất cho từng đồ uống.
        /// Bao gồm xử lý lỗi nghiệp vụ và lỗi hệ thống.
        /// </summary>
        /// <returns>Danh sách các đối tượng Douong.</returns>
        public List<Douong> GetAllDouongs()
        {
            try
            {
                return _douongDAL.GetAllDouongs();
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết hơn nếu có hệ thống logging
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi lấy danh sách đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm đồ uống theo tên hoặc mã.
        /// Bao gồm xử lý lỗi nghiệp vụ và lỗi hệ thống.
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách các đối tượng Douong phù hợp.</returns>
        public List<Douong> SearchDouongs(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                // Nếu searchTerm rỗng, trả về tất cả đồ uống thay vì tìm kiếm rỗng
                return GetAllDouongs();
            }

            try
            {
                return _douongDAL.SearchDouongs(searchTerm.Trim());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi tìm kiếm đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy một đồ uống theo mã đồ uống.
        /// Bao gồm xử lý lỗi nghiệp vụ và lỗi hệ thống.
        /// </summary>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Đối tượng Douong hoặc null nếu không tìm thấy.</returns>
        public Douong GetDouongById(string madouong)
        {
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(madouong));
            }

            try
            {
                return _douongDAL.GetDouongById(madouong.Trim());
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi lấy đồ uống theo mã: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm một đồ uống mới vào CSDL.
        /// Bao gồm kiểm tra ràng buộc nghiệp vụ.
        /// </summary>
        /// <param name="douong">Đối tượng Douong cần thêm.</param>
        public void AddDouong(Douong douong)
        {
            // Kiểm tra ràng buộc nghiệp vụ
            if (douong == null)
            {
                throw new ArgumentNullException(nameof(douong), "Đối tượng đồ uống không được null.");
            }
            if (string.IsNullOrWhiteSpace(douong.Madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(douong.Madouong));
            }
            if (string.IsNullOrWhiteSpace(douong.Tendouong))
            {
                throw new ArgumentException("Tên đồ uống không được để trống.", nameof(douong.Tendouong));
            }
            if (string.IsNullOrWhiteSpace(douong.Maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống.", nameof(douong.Maloai));
            }

            // Có thể thêm kiểm tra trùng mã đồ uống nếu Madouong là khóa chính và không tự động tăng
            // Ví dụ:
            // if (_douongDAL.GetDouongById(douong.Madouong) != null)
            // {
            //     throw new InvalidOperationException($"Mã đồ uống '{douong.Madouong}' đã tồn tại.");
            // }

            try
            {
                _douongDAL.AddDouong(douong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi thêm đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin đồ uống trong CSDL.
        /// Bao gồm kiểm tra ràng buộc nghiệp vụ.
        /// </summary>
        /// <param name="douong">Đối tượng Douong cần cập nhật.</param>
        public void UpdateDouong(Douong douong)
        {
            // Kiểm tra ràng buộc nghiệp vụ
            if (douong == null)
            {
                throw new ArgumentNullException(nameof(douong), "Đối tượng đồ uống không được null.");
            }
            if (string.IsNullOrWhiteSpace(douong.Madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(douong.Madouong));
            }
            if (string.IsNullOrWhiteSpace(douong.Tendouong))
            {
                throw new ArgumentException("Tên đồ uống không được để trống.", nameof(douong.Tendouong));
            }
            if (string.IsNullOrWhiteSpace(douong.Maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống.", nameof(douong.Maloai));
            }

            // Kiểm tra xem đồ uống có tồn tại trước khi cập nhật không
            if (_douongDAL.GetDouongById(douong.Madouong) == null)
            {
                throw new InvalidOperationException($"Không tìm thấy đồ uống với mã '{douong.Madouong}' để cập nhật.");
            }

            try
            {
                _douongDAL.UpdateDouong(douong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi cập nhật đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa một đồ uống khỏi CSDL.
        /// Bao gồm kiểm tra ràng buộc nghiệp vụ.
        /// </summary>
        /// <param name="madouong">Mã đồ uống cần xóa.</param>
        public void DeleteDouong(string madouong)
        {
            if (string.IsNullOrWhiteSpace(madouong))
            {
                throw new ArgumentException("Mã đồ uống không được để trống.", nameof(madouong));
            }

            // Kiểm tra xem đồ uống có tồn tại trước khi xóa không
            if (_douongDAL.GetDouongById(madouong) == null)
            {
                throw new InvalidOperationException($"Không tìm thấy đồ uống với mã '{madouong}' để xóa.");
            }

            // TODO: Thêm logic kiểm tra ràng buộc toàn vẹn dữ liệu
            // Ví dụ: Kiểm tra xem đồ uống này có đang được sử dụng trong bất kỳ đơn hàng nào không.
            // Nếu có, bạn có thể ném một InvalidOperationException hoặc xử lý nó theo yêu cầu nghiệp vụ (ví dụ: đánh dấu là không hoạt động thay vì xóa cứng).
            // For example:
            // var chiTietDonHangDAL = new ChitietdonhangDAL(); // Need to instantiate if not already available
            // if (chiTietDonHangDAL.GetChiTietByMadouong(madouong).Any()) // Assuming a method exists in ChitietdonhangDAL
            // {
            //     throw new InvalidOperationException($"Không thể xóa đồ uống '{madouong}' vì nó đang có trong các đơn hàng.");
            // }

            try
            {
                _douongDAL.DeleteDouong(madouong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi xóa đồ uống: {ex.Message}", ex);
            }
        }
        public string GenerateNextMaDU()
        {
            List<string> allIDs = _douongDAL.GetAllMaDU(); // Lấy tất cả mã DU
            int nextNumber = 1;

            if (allIDs.Count > 0)
            {
                List<int> numbers = new List<int>();
                foreach (var id in allIDs)
                {
                    if (id.StartsWith("DU") && int.TryParse(id.Substring(2), out int n))
                        numbers.Add(n);
                }
                numbers.Sort();
                for (int i = 1; i <= numbers.Count + 1; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        nextNumber = i;
                        break;
                    }
                }
            }

            return "DU" + nextNumber.ToString("D3"); // DU001, DU002 ...
        }
        public  string GenerateNextMaDUInMemory(HashSet<string> usedMa)
        {
            int max = 0;
            foreach (var ma in usedMa)
            {
                if (ma.Length <= 2) continue;
                if (int.TryParse(ma.Substring(2), out int n))
                {
                    if (n > max) max = n;
                }
            }
            return "DU" + (max + 1).ToString("D3"); // DU001, DU002,...
        }

        public void ImportDouongs(List<Douong> douongs)
        {
            if (douongs == null || douongs.Count == 0)
                throw new ArgumentException("Danh sách đồ uống rỗng.", nameof(douongs));

            _douongDAL.ImportDouongs(douongs);
        }
        public List<string> GetAllMaDU()
        {
            return _douongDAL.GetAllMaDU(); // chỉ gọi 1 lần
        }


    }
}