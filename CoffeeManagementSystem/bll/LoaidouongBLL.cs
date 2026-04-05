// using System.Linq; // Không cần nếu không dùng LINQ
// using System.Windows.Forms; // KHÔNG dùng trong BLL (BLL không tham chiếu UI)

// Đảm bảo namespace này khớp với cấu trúc dự án
using CoffeeManagementSystem.DAL; // Tham chiếu đến tầng DAL
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// Lớp nghiệp vụ cho bảng Loaidouong.
    /// Đứng giữa UI và LoaidouongDAL.
    /// </summary>
    public class LoaidouongBLL
    {
        private LoaidouongDAL _loaidouongDAL;

        public LoaidouongBLL()
        {
            _loaidouongDAL = new LoaidouongDAL();
        }

        /// <summary>
        /// Sinh mã loại đồ uống mới dạng LD001, LD002,...
        /// Nếu có khoảng trống (thiếu LD003) sẽ lấp từ số nhỏ nhất chưa dùng.
        /// </summary>
        /// <returns>Mã loại đồ uống mới.</returns>
        public string GenerateNextMaloai()
        {
            try
            {
                // Lấy toàn bộ danh sách loại đồ uống hiện có
                List<Loaidouong> allLoais = _loaidouongDAL.GetAllLoaidouongs();

                int nextNumber = 1;

                if (allLoais != null && allLoais.Count > 0)
                {
                    List<int> numbers = new List<int>();

                    // Tách phần số từ Maloai (LD001 → 1)
                    foreach (var loai in allLoais)
                    {
                        if (!string.IsNullOrWhiteSpace(loai.Maloai) &&
                            loai.Maloai.StartsWith("LD") &&
                            int.TryParse(loai.Maloai.Substring(2), out int n))
                        {
                            numbers.Add(n);
                        }
                    }

                    if (numbers.Count > 0)
                    {
                        numbers.Sort();

                        // Tìm số nhỏ nhất còn trống: 1,2,3,...
                        int expected = 1;
                        foreach (var n in numbers)
                        {
                            if (n != expected)
                                break;

                            expected++;
                        }

                        nextNumber = expected;
                    }
                }

                // Ghép thành LD001, LD010,...
                return "LD" + nextNumber.ToString("D3");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi sinh mã loại đồ uống mới: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả loại đồ uống.
        /// Có thể chèn thêm quy tắc nghiệp vụ (phân quyền, cache, ...) nếu cần.
        /// </summary>
        /// <returns>Danh sách Loaidouong.</returns>
        public List<Loaidouong> GetAllLoaidouongs()
        {
            try
            {
                return _loaidouongDAL.GetAllLoaidouongs();
            }
            catch (Exception ex)
            {
                // Ném ngoại lệ để UI xử lý
                throw new Exception($"Lỗi BLL khi lấy danh sách loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy loại đồ uống theo mã.
        /// </summary>
        /// <param name="maloai">Mã loại đồ uống.</param>
        /// <returns>Đối tượng Loaidouong nếu tìm thấy, ngược lại null.</returns>
        public Loaidouong GetLoaidouongById(string maloai)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống.", nameof(maloai));
            }

            try
            {
                return _loaidouongDAL.GetLoaidouongById(maloai);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy loại đồ uống theo mã: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm một loại đồ uống mới.
        /// Kiểm tra Maloai và Tenloai trước khi gọi DAL.
        /// </summary>
        /// <param name="loaidouong">Đối tượng Loaidouong cần thêm.</param>
        /// <returns>True nếu thêm thành công.</returns>
        public bool AddLoaidouong(Loaidouong loaidouong)
        {
            // Kiểm tra nghiệp vụ: object, mã, tên không được trống
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

            // Kiểm tra trùng mã loại trước khi thêm
            if (_loaidouongDAL.GetLoaidouongById(loaidouong.Maloai) != null)
            {
                throw new InvalidOperationException(
                    $"Mã loại đồ uống '{loaidouong.Maloai}' đã tồn tại. Vui lòng chọn mã khác.");
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
        /// Cập nhật thông tin loại đồ uống.
        /// Kiểm tra tồn tại và dữ liệu trước khi cập nhật.
        /// </summary>
        /// <param name="loaidouong">Đối tượng Loaidouong đã chỉnh sửa.</param>
        /// <returns>True nếu cập nhật thành công.</returns>
        public bool UpdateLoaidouong(Loaidouong loaidouong)
        {
            // Kiểm tra nghiệp vụ
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

            // Đảm bảo loại đồ uống cần cập nhật có tồn tại
            if (_loaidouongDAL.GetLoaidouongById(loaidouong.Maloai) == null)
            {
                throw new InvalidOperationException(
                    $"Loại đồ uống có mã '{loaidouong.Maloai}' cần cập nhật không tồn tại.");
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
        /// Xóa một loại đồ uống theo mã.
        /// Có thể bổ sung kiểm tra phụ thuộc (đồ uống đang dùng loại này) trước khi xóa.
        /// </summary>
        /// <param name="maloai">Mã loại đồ uống cần xóa.</param>
        /// <returns>True nếu xóa thành công.</returns>
        public bool DeleteLoaidouong(string maloai)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrWhiteSpace(maloai))
            {
                throw new ArgumentException("Mã loại đồ uống không được để trống khi xóa.", nameof(maloai));
            }

            // Gợi ý: có thể kiểm tra ràng buộc (nếu loại đang được đồ uống sử dụng thì không cho xóa)
            // Ví dụ (nếu có DouongDAL và hàm kiểm tra):
            // if (_douongDAL.DoesCategoryHaveDrinks(maloai))
            // {
            //     throw new InvalidOperationException("Không thể xóa loại đồ uống này vì đang có đồ uống sử dụng.");
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
        /// Tìm kiếm loại đồ uống theo từ khóa (mã, tên,...).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách Loaidouong phù hợp.</returns>
        public List<Loaidouong> SearchLoaidouongs(string searchTerm)
        {
            try
            {
                // DAL có thể tự xử lý trường hợp searchTerm rỗng.
                return _loaidouongDAL.SearchLoaidouongs(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi tìm kiếm loại đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Import nhiều loại đồ uống từ danh sách (thường dùng khi đọc từ file).
        /// </summary>
        /// <param name="Loaidouongs">Danh sách loại đồ uống cần import.</param>
        public void ImportLoaidouongs(List<Loaidouong> Loaidouongs)
        {
            if (Loaidouongs == null || Loaidouongs.Count == 0)
                throw new ArgumentException("Danh sách loại đồ uống rỗng.", nameof(Loaidouongs));

            _loaidouongDAL.ImportLoaidouongs(Loaidouongs);
        }

        /// <summary>
        /// Tạo mã Maloai mới trên bộ nhớ, dựa vào tập mã đã dùng (không truy vấn DB).
        /// Thường dùng khi import hàng loạt.
        /// </summary>
        /// <param name="usedMa">Tập hợp các mã loại đã sử dụng.</param>
        /// <returns>Mã mới dạng LDxxx.</returns>
        public string GenerateNextMaLDInMemory(HashSet<string> usedMa)
        {
            int max = 0;

            foreach (var ma in usedMa)
            {
                // Giả định Maloai có định dạng LD001, LD002,...
                if (ma.Length <= 2) continue;
                if (int.TryParse(ma.Substring(2), out int n))
                {
                    if (n > max) max = n;
                }
            }

            return "LD" + (max + 1).ToString("D3"); // LD001, LD002, LD003,...
        }

        /// <summary>
        /// Lấy tất cả mã loại đồ uống (Maloai) từ CSDL.
        /// </summary>
        /// <returns>Danh sách mã LD...</returns>
        public List<string> GetAllMaLD()
        {
            return _loaidouongDAL.GetAllMaLD(); // chỉ gọi 1 lần xuống DAL
        }
        public void ExportLoaiDoUongToCSV(List<Loaidouong> list, string filePath)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentException("Đường dẫn file không hợp lệ.", nameof(filePath));

            // Đảm bảo .csv
            if (!filePath.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                filePath = Path.ChangeExtension(filePath, ".csv");

            // Lấy header từ tên property của class Douong (đồng bộ với DB)
            var props = typeof(Loaidouong).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
