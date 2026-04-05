using CoffeeManagementSystem.DAL; // Lấy dữ liệu từ lớp DAL
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// Lớp nghiệp vụ (BLL) cho đồ uống.
    /// Đóng vai trò trung gian giữa UI và DouongDAL.
    /// </summary>
    public class DouongBLL
    {
        private DouongDAL _douongDAL;

        public DouongBLL()
        {
            _douongDAL = new DouongDAL();
        }

        /// <summary>
        /// Lấy tất cả đồ uống từ CSDL.
        /// Gọi xuống DAL và chuẩn hóa lỗi ném ra tầng trên.
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
                // Có thể log lỗi chi tiết hơn nếu có hệ thống logging
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi lấy danh sách đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm đồ uống theo tên hoặc mã.
        /// Nếu từ khóa trống → trả về toàn bộ danh sách.
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
        /// Lấy một đồ uống theo mã.
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
        /// Thực hiện kiểm tra ràng buộc nghiệp vụ trước khi gọi DAL.
        /// </summary>
        /// <param name="douong">Đối tượng Douong cần thêm.</param>
        public void AddDouong(Douong douong)
        {
            // Kiểm tra dữ liệu đầu vào (ràng buộc nghiệp vụ cơ bản)
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

            // Có thể thêm kiểm tra trùng mã đồ uống nếu Madouong là khóa chính và không tự tăng
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
        /// Cập nhật thông tin một đồ uống trong CSDL.
        /// Thực hiện kiểm tra dữ liệu và tồn tại trước khi cập nhật.
        /// </summary>
        /// <param name="douong">Đối tượng Douong cần cập nhật.</param>
        public void UpdateDouong(Douong douong)
        {
            // Kiểm tra dữ liệu đầu vào
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
        /// Có thể mở rộng thêm kiểm tra ràng buộc toàn vẹn dữ liệu (đơn hàng, hóa đơn...).
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

            // TODO: Thêm logic kiểm tra toàn vẹn dữ liệu
            // Ví dụ: kiểm tra xem đồ uống này có xuất hiện trong chi tiết đơn hàng chưa.
            // Nếu có, có thể:
            // - Ném InvalidOperationException
            // - Hoặc chỉ đánh dấu "ngưng kinh doanh" thay vì xóa cứng.

            try
            {
                _douongDAL.DeleteDouong(madouong);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Lỗi nghiệp vụ khi xóa đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Sinh mã đồ uống tiếp theo dạng DUxxx dựa trên danh sách mã hiện có trong DB.
        /// Có thể lấp "lỗ hổng" nếu có mã bị thiếu (tìm số nhỏ nhất chưa được dùng).
        /// </summary>
        public string GenerateNextMaDU()
        {
            // Lấy tất cả mã DU hiện có
            List<string> allIDs = _douongDAL.GetAllMaDU();
            int nextNumber = 1;

            if (allIDs.Count > 0)
            {
                List<int> numbers = new List<int>();

                // Tách phần số từ mỗi mã DU và đưa vào danh sách numbers
                foreach (var id in allIDs)
                {
                    if (id.StartsWith("DU") && int.TryParse(id.Substring(2), out int n))
                        numbers.Add(n);
                }

                numbers.Sort();

                // Tìm số nhỏ nhất chưa được dùng (1, 2, 3, ...)
                for (int i = 1; i <= numbers.Count + 1; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        nextNumber = i;
                        break;
                    }
                }
            }

            // Format thành DU001, DU002, ...
            return "DU" + nextNumber.ToString("D3");
        }

        /// <summary>
        /// Sinh mã DU tiếp theo nhưng làm việc trên bộ nhớ (HashSet mã đã dùng),
        /// </summary>
        /// <param name="usedMa">Tập các mã đã sử dụng.</param>
        /// <returns>Mã mới dạng DUxxx.</returns>
        public string GenerateNextMaDUInMemory(HashSet<string> usedMa)
        {
            int max = 0;

            // Lấy phần số lớn nhất trong các mã hiện có
            foreach (var ma in usedMa)
            {
                if (ma.Length <= 2) continue;
                if (int.TryParse(ma.Substring(2), out int n))
                {
                    if (n > max) max = n;
                }
            }

            // Tăng 1 số so với max hiện tại
            return "DU" + (max + 1).ToString("D3"); // DU001, DU002,...
        }

        /// <summary>
        /// Import nhiều đồ uống cùng lúc vào CSDL.
        /// Thường dùng khi đọc từ file Excel/CSV.
        /// </summary>
        /// <param name="douongs">Danh sách đồ uống cần import.</param>
        public void ImportDouongs(List<Douong> douongs)
        {
            if (douongs == null || douongs.Count == 0)
                throw new ArgumentException("Danh sách đồ uống rỗng.", nameof(douongs));

            _douongDAL.ImportDouongs(douongs);
        }

        /// <summary>
        /// Lấy tất cả mã đồ uống (Madouong) từ CSDL.
        /// </summary>
        /// <returns>Danh sách mã DU.</returns>
        public List<string> GetAllMaDU()
        {
            return _douongDAL.GetAllMaDU(); // chỉ gọi 1 lần từ DAL
        }
        // ======================================================
        // THÊ/// <summary>
        /// Xuất danh sách đồ uống ra file CSV.
        /// Gọi DAL lấy DataTable rồi ghi ra CSV.
        /// </summary>
        public void ExportDoUongToCSV(List<Douong> list, string filePath)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Đường dẫn file không hợp lệ.", nameof(filePath));

            // Đảm bảo .csv
            if (!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                filePath = Path.ChangeExtension(filePath, ".csv");

            // Lấy header từ tên property của class Douong (đồng bộ với DB)
            var props = typeof(Douong).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Ghi file với UTF8 (không BOM) hoặc nếu muốn Excel dễ đọc có thể dùng Encoding.UTF8 preamble:
            var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true); // có BOM để Excel nhận UTF-8
            using (var sw = new StreamWriter(filePath, false, encoding))
            {
                // Header
                sw.WriteLine(string.Join(",", props.Select(p => EscapeCsv(p.Name))));

                // Rows
                foreach (var item in list)
                {
                    var values = props.Select(p =>
                    {
                        var val = p.GetValue(item);
                        return EscapeCsv(val?.ToString() ?? "");
                    });
                    sw.WriteLine(string.Join(",", values));
                }
            }
        }

        /// <summary>
        /// Escape theo CSV: 
        /// - nếu chứa dấu " hoặc , hoặc newline -> bao bằng " và doubled quotes bên trong
        /// </summary>
        private string EscapeCsv(string s)
        {
            if (s == null) return "";
            bool mustQuote = s.Contains(",") || s.Contains("\"") || s.Contains("\n") || s.Contains("\r");
            string escaped = s.Replace("\"", "\"\""); // double quotes
            return mustQuote ? $"\"{escaped}\"" : escaped;
        }
    }

}

