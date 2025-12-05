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
    /// Lớp truy cập dữ liệu (DAL) cho bảng Loaidouong.
    /// Chỉ làm việc với CSDL, không liên quan đến UI.
    /// </summary>
    public class LoaidouongDAL : BaseDataAccess
    {
        public LoaidouongDAL() : base() { }

        /// <summary>
        /// Lấy tất cả loại đồ uống từ CSDL.
        /// </summary>
        /// <returns>Danh sách Loaidouong.</returns>
        public List<Loaidouong> GetAllLoaidouongs()
        {
            List<Loaidouong> loaidouongs = new List<Loaidouong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Maloai, Tenloai FROM Loaidouong";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Loaidouong loaidouong = new Loaidouong
                            {
                                Maloai = reader["Maloai"].ToString(),
                                Tenloai = reader["Tenloai"].ToString()
                            };
                            loaidouongs.Add(loaidouong);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DAL không hiển thị MessageBox, ném lỗi để BLL/UI xử lý
                    throw new Exception($"Lỗi DAL khi lấy danh sách loại đồ uống: {ex.Message}", ex);
                }
            }

            return loaidouongs;
        }

        /// <summary>
        /// Lấy thông tin một loại đồ uống theo mã.
        /// </summary>
        /// <param name="maloai">Mã loại đồ uống.</param>
        /// <returns>Đối tượng Loaidouong nếu tìm thấy, ngược lại null.</returns>
        public Loaidouong GetLoaidouongById(string maloai)
        {
            Loaidouong loaidouong = null;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Maloai, Tenloai FROM Loaidouong WHERE Maloai = @Maloai";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Maloai", maloai);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                loaidouong = new Loaidouong
                                {
                                    Maloai = reader["Maloai"].ToString(),
                                    Tenloai = reader["Tenloai"].ToString()
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy loại đồ uống theo mã: {ex.Message}", ex);
                }
            }

            return loaidouong;
        }

        /// <summary>
        /// Thêm một loại đồ uống mới vào CSDL.
        /// </summary>
        /// <param name="loaidouong">Đối tượng Loaidouong cần thêm.</param>
        public void AddLoaidouong(Loaidouong loaidouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Loaidouong (Maloai, Tenloai) 
                        VALUES (@Maloai, @Tenloai)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Maloai", loaidouong.Maloai);
                        command.Parameters.AddWithValue("@Tenloai", loaidouong.Tenloai);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi thêm loại đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Cập nhật thông tin một loại đồ uống.
        /// </summary>
        /// <param name="loaidouong">Đối tượng Loaidouong (Maloai bắt buộc, Tenloai là dữ liệu mới).</param>
        public void UpdateLoaidouong(Loaidouong loaidouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Loaidouong
                        SET Tenloai = @Tenloai
                        WHERE Maloai = @Maloai";

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Tenloai", loaidouong.Tenloai);
                        command.Parameters.AddWithValue("@Maloai", loaidouong.Maloai);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi cập nhật loại đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Xóa một loại đồ uống khỏi CSDL.
        /// </summary>
        /// <param name="maloai">Mã loại đồ uống cần xóa.</param>
        public void DeleteLoaidouong(string maloai)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Loaidouong WHERE Maloai = @Maloai";

                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Maloai", maloai);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi xóa loại đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Tìm kiếm loại đồ uống theo từ khóa (trên Maloai hoặc Tenloai).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách Loaidouong phù hợp.</returns>
        public List<Loaidouong> SearchLoaidouongs(string searchTerm)
        {
            List<Loaidouong> loaidouongs = new List<Loaidouong>();

            // Nếu chuỗi tìm kiếm trống → trả list rỗng (BLL có thể xử lý thành lấy all nếu muốn)
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return loaidouongs;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string selectSql = @"
                        SELECT Maloai, Tenloai
                        FROM Loaidouong
                        WHERE LOWER(Maloai) LIKE @SearchTerm
                           OR LOWER(Tenloai) LIKE @SearchTerm";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Loaidouong loaidouong = new Loaidouong
                                {
                                    Maloai = reader["Maloai"].ToString(),
                                    Tenloai = reader["Tenloai"].ToString()
                                };
                                loaidouongs.Add(loaidouong);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi tìm kiếm loại đồ uống: {ex.Message}", ex);
                }
            }

            return loaidouongs;
        }

        /// <summary>
        /// Import nhiều loại đồ uống:
        /// - Nếu Maloai đã tồn tại → cập nhật Tenloai.
        /// - Nếu chưa tồn tại → thêm mới.
        /// Thực hiện trong transaction để đảm bảo toàn vẹn dữ liệu.
        /// </summary>
        public void ImportLoaidouongs(List<Loaidouong> loaidouongs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var a in loaidouongs)
                        {
                            // Kiểm tra đã tồn tại Maloai hay chưa
                            string checkSql = "SELECT COUNT(1) FROM Loaidouong WHERE Maloai = @Maloai";
                            using (var cmdCheck = new SQLiteCommand(checkSql, connection, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Maloai", a.Maloai);
                                long count = (long)cmdCheck.ExecuteScalar();

                                if (count > 0)
                                {
                                    // Đã tồn tại → update Tenloai
                                    string updateSql = @"
                                        UPDATE Loaidouong 
                                        SET Tenloai = @Tenloai 
                                        WHERE Maloai = @Maloai";

                                    using (var cmdUpdate = new SQLiteCommand(updateSql, connection, transaction))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@Tenloai", a.Tenloai);
                                        cmdUpdate.Parameters.AddWithValue("@Maloai", a.Maloai);
                                        cmdUpdate.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Chưa tồn tại → insert mới
                                    string insertSql = @"
                                        INSERT INTO Loaidouong (Maloai, Tenloai) 
                                        VALUES (@Maloai, @Tenloai)";

                                    using (var cmdInsert = new SQLiteCommand(insertSql, connection, transaction))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@Maloai", a.Maloai);
                                        cmdInsert.Parameters.AddWithValue("@Tenloai", a.Tenloai);
                                        cmdInsert.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Lỗi DAL khi import nhiều loại đồ uống: " + ex.Message, ex);
                    }
                }
            }
        }

        /// <summary>
        /// Lấy tất cả mã loại đồ uống (Maloai) từ CSDL.
        /// Thường dùng để sinh mã mới trên BLL.
        /// </summary>
        /// <returns>Danh sách chuỗi Maloai.</returns>
        public List<string> GetAllMaLD()
        {
            List<string> maList = new List<string>();

            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Maloai FROM Loaidouong", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maList.Add(reader["Maloai"].ToString());
                    }
                }
            }

            return maList;
        }
        // ============================================================
        // EXPORT CSV 
        // ============================================================
        public void ExportToCSV(List<Loaidouong> list, string filePath)
        {
            if (list == null) throw new ArgumentNullException(nameof(list));

            // Lấy property làm header
            var props = typeof(Loaidouong).GetProperties(BindingFlags.Public | BindingFlags.Instance);

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
