using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng Douong (đồ uống).
    /// Kế thừa BaseDataAccess để dùng chung ConnectionString.
    /// </summary>
    public class DouongDAL : BaseDataAccess
    {
        // Chuỗi kết nối riêng (đang song song với ConnectionString của BaseDataAccess)
        private readonly string _connectionString = @"DataSource=QuanLyCaPheDatabase.db;Version=3;";

        // Đối tượng DAL cho bảng Giadouong để lấy giá mới nhất của từng đồ uống
        private GiadouongDAL giadouongDAL;

        public DouongDAL() : base()
        {
            // Khởi tạo GiadouongDAL trong constructor
            giadouongDAL = new GiadouongDAL();
        }

        // =====================================================
        // LẤY DANH SÁCH ĐỒ UỐNG
        // =====================================================

        /// <summary>
        /// Lấy tất cả đồ uống từ CSDL và gán luôn giá hiện tại (CurrentGia) cho từng đồ uống.
        /// </summary>
        public List<Douong> GetAllDouongs()
        {
            List<Douong> douongs = new List<Douong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madouong, Tendouong, Maloai, Mota, Hinhanh FROM Douong";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Douong douong = new Douong
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"].ToString(),
                                    Mota = reader["Mota"] != DBNull.Value ? reader["Mota"].ToString() : null,
                                    Hinhanh = reader["Hinhanh"] != DBNull.Value ? reader["Hinhanh"].ToString() : null
                                };

                                // Gọi GiadouongDAL để lấy giá mới nhất → gán vào thuộc tính CurrentGia
                                // Dùng toán tử ?./?? để xử lý trường hợp không có giá (trả về 0m)
                                douong.CurrentGia = giadouongDAL.GetLatestGiaByMadouong(douong.Madouong)?.Giaban ?? 0m;

                                douongs.Add(douong);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DAL không hiển thị MessageBox, ném lỗi lên BLL/UI xử lý
                    throw new Exception($"Lỗi DAL khi lấy tất cả đồ uống: {ex.Message}", ex);
                }
            }
            return douongs;
        }

        /// <summary>
        /// Tìm kiếm đồ uống theo tên hoặc mã (LIKE, không phân biệt hoa/thường).
        /// Đồng thời gán CurrentGia cho từng kết quả.
        /// </summary>
        public List<Douong> SearchDouongs(string searchTerm)
        {
            List<Douong> douongs = new List<Douong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Madouong, Tendouong, Maloai, Mota, Hinhanh 
                        FROM Douong 
                        WHERE LOWER(Tendouong) LIKE @SearchTerm OR LOWER(Madouong) LIKE @SearchTerm";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // Tìm kiếm chứa từ khóa ( %term% )
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Douong douong = new Douong
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"].ToString(),
                                    Mota = reader["Mota"] != DBNull.Value ? reader["Mota"].ToString() : null,
                                    Hinhanh = reader["Hinhanh"] != DBNull.Value ? reader["Hinhanh"].ToString() : null
                                };

                                // Gán giá mới nhất cho đồ uống
                                douong.CurrentGia = giadouongDAL.GetLatestGiaByMadouong(douong.Madouong)?.Giaban ?? 0m;
                                douongs.Add(douong);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi tìm kiếm đồ uống: {ex.Message}", ex);
                }
            }
            return douongs;
        }

        /// <summary>
        /// Lấy thông tin chi tiết một đồ uống theo mã.
        /// Gồm cả giá hiện tại (CurrentGia).
        /// </summary>
        public Douong GetDouongById(string madouong)
        {
            Douong douong = null;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madouong, Tendouong, Maloai, Mota, Hinhanh FROM Douong WHERE Madouong = @Madouong";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madouong", madouong);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                douong = new Douong
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"].ToString(),
                                    Mota = reader["Mota"] != DBNull.Value ? reader["Mota"].ToString() : null,
                                    Hinhanh = reader["Hinhanh"] != DBNull.Value ? reader["Hinhanh"].ToString() : null
                                };

                                // Gán giá hiện tại cho 1 đồ uống cụ thể
                                douong.CurrentGia = giadouongDAL.GetLatestGiaByMadouong(douong.Madouong)?.Giaban ?? 0m;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy đồ uống theo ID: {ex.Message}", ex);
                }
            }
            return douong;
        }

        // =====================================================
        // THÊM / CẬP NHẬT / XÓA ĐỒ UỐNG
        // =====================================================

        /// <summary>
        /// Thêm một đồ uống mới vào bảng Douong.
        /// </summary>
        public void AddDouong(Douong douong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Douong (Madouong, Tendouong, Maloai, Mota, Hinhanh) 
                        VALUES (@Madouong, @Tendouong, @Maloai, @Mota, @Hinhanh)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madouong", douong.Madouong);
                        command.Parameters.AddWithValue("@Tendouong", douong.Tendouong);
                        command.Parameters.AddWithValue("@Maloai", douong.Maloai);
                        command.Parameters.AddWithValue("@Mota", (object)douong.Mota ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Hinhanh", (object)douong.Hinhanh ?? DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi thêm đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin một đồ uống (không xử lý giá).
        /// </summary>
        public void UpdateDouong(Douong douong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Douong 
                        SET Tendouong = @Tendouong, 
                            Maloai    = @Maloai, 
                            Mota      = @Mota, 
                            Hinhanh   = @Hinhanh 
                        WHERE Madouong = @Madouong";

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Tendouong", douong.Tendouong);
                        command.Parameters.AddWithValue("@Maloai", douong.Maloai);
                        command.Parameters.AddWithValue("@Mota", (object)douong.Mota ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Hinhanh", (object)douong.Hinhanh ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Madouong", douong.Madouong);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi cập nhật đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Xóa một đồ uống theo mã.
        /// (Có thể cần xóa/cập nhật các bản ghi giá ở bảng Giadouong trước, tùy nghiệp vụ.)
        /// </summary>
        public void DeleteDouong(string madouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string deleteSql = "DELETE FROM Douong WHERE Madouong = @Madouong";
                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madouong", madouong);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi xóa đồ uống: {ex.Message}", ex);
                }
            }
        }

        // =====================================================
        // HỖ TRỢ SINH MÃ / IMPORT DANH SÁCH ĐỒ UỐNG
        // =====================================================

        /// <summary>
        /// Lấy toàn bộ mã đồ uống (Madouong) từ CSDL.
        /// Dùng cho sinh mã tự động / kiểm tra trùng / import.
        /// </summary>
        public List<string> GetAllMaDU()
        {
            List<string> maList = new List<string>();

            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Madouong FROM Douong", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maList.Add(reader["Madouong"].ToString());
                    }
                }
            }

            return maList;
        }

        /// <summary>
        /// Import nhiều đồ uống:
        /// - Nếu Madouong đã tồn tại → UPDATE
        /// - Nếu chưa tồn tại → INSERT
        /// Thực hiện trong transaction để đảm bảo toàn vẹn.
        /// </summary>
        public void ImportDouongs(List<Douong> douongs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var d in douongs)
                        {
                            // 1. Kiểm tra đồ uống đã tồn tại theo Madouong hay chưa
                            string checkSql = "SELECT COUNT(1) FROM Douong WHERE Madouong = @Madouong";
                            using (var cmdCheck = new SQLiteCommand(checkSql, connection, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Madouong", d.Madouong);
                                long count = (long)cmdCheck.ExecuteScalar();

                                if (count > 0)
                                {
                                    // 2. Nếu đã tồn tại → cập nhật thông tin
                                    string updateSql = @"
                                        UPDATE Douong 
                                        SET Tendouong = @Tendouong, 
                                            Maloai    = @Maloai, 
                                            Mota      = @Mota, 
                                            Hinhanh   = @Hinhanh 
                                        WHERE Madouong = @Madouong";

                                    using (var cmdUpdate = new SQLiteCommand(updateSql, connection, transaction))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@Tendouong", d.Tendouong);
                                        cmdUpdate.Parameters.AddWithValue("@Maloai", d.Maloai);
                                        cmdUpdate.Parameters.AddWithValue("@Mota", (object)d.Mota ?? DBNull.Value);
                                        cmdUpdate.Parameters.AddWithValue("@Hinhanh", (object)d.Hinhanh ?? DBNull.Value);
                                        cmdUpdate.Parameters.AddWithValue("@Madouong", d.Madouong);
                                        cmdUpdate.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // 3. Nếu chưa tồn tại → chèn mới
                                    string insertSql = @"
                                        INSERT INTO Douong (Madouong, Tendouong, Maloai, Mota, Hinhanh) 
                                        VALUES (@Madouong, @Tendouong, @Maloai, @Mota, @Hinhanh)";

                                    using (var cmdInsert = new SQLiteCommand(insertSql, connection, transaction))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@Madouong", d.Madouong);
                                        cmdInsert.Parameters.AddWithValue("@Tendouong", d.Tendouong);
                                        cmdInsert.Parameters.AddWithValue("@Maloai", d.Maloai);
                                        cmdInsert.Parameters.AddWithValue("@Mota", (object)d.Mota ?? DBNull.Value);
                                        cmdInsert.Parameters.AddWithValue("@Hinhanh", (object)d.Hinhanh ?? DBNull.Value);
                                        cmdInsert.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        // Commit nếu mọi thứ OK
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Có lỗi → rollback và ném ngoại lệ
                        transaction.Rollback();
                        throw new Exception("Lỗi DAL khi import nhiều đồ uống: " + ex.Message, ex);
                    }
                }
            }
        }
        // ============================================================
        // EXPORT CSV 
        // ============================================================
        public void ExportToCSV(List<Douong> list, string filePath)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            // Lấy property làm header
            var props = typeof(Douong).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var encoding = new UTF8Encoding(true); // BOM cho Excel

            using (var sw = new StreamWriter(filePath, false, encoding))
            {
                // HEADER
                sw.WriteLine(string.Join(",", props.Select(p => EscapeCsv(p.Name))));

                // ROWS
                foreach (var d in list)
                {
                    var values = props.Select(p => EscapeCsv(p.GetValue(d)?.ToString() ?? ""));
                    sw.WriteLine(string.Join(",", values));
                }
            }
        }

        private string EscapeCsv(string s)
        {
            if (s == null) return "";

            bool mustQuote = s.Contains(",") || s.Contains("\"") || s.Contains("\n");
            string escaped = s.Replace("\"", "\"\"");

            return mustQuote ? $"\"{escaped}\"" : escaped;
        }
    }
}

