using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// Lớp truy cập dữ liệu (DAL) cho bảng Giadouong.
    /// Chỉ làm việc với CSDL, không xử lý giao diện.
    /// </summary>
    public class GiadouongDAL : BaseDataAccess
    {
        public GiadouongDAL() : base() { }

        /// <summary>
        /// Lấy tất cả bản ghi giá đồ uống từ CSDL.
        /// </summary>
        /// <returns>Danh sách Giadouong.</returns>
        public List<Giadouong> GetAllGiadouongs()
        {
            List<Giadouong> giadouongs = new List<Giadouong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Magia, Madouong, Giaban, Thoigianapdung FROM Giadouong";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Giadouong giadouong = new Giadouong
                            {
                                Magia = reader["Magia"].ToString(),
                                Madouong = reader["Madouong"].ToString(),
                                Giaban = Convert.ToDecimal(reader["Giaban"]),
                                Thoigianapdung = DateTime.Parse(reader["Thoigianapdung"].ToString())
                            };
                            giadouongs.Add(giadouong);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // DAL không hiển thị MessageBox, ném lỗi cho BLL/UI xử lý
                    throw new Exception($"Lỗi DAL khi lấy danh sách giá đồ uống: {ex.Message}", ex);
                }
            }

            return giadouongs;
        }

        /// <summary>
        /// Lấy bản ghi giá mới nhất cho một đồ uống theo mã.
        /// </summary>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Đối tượng Giadouong mới nhất, hoặc null nếu không có.</returns>
        public Giadouong GetLatestGiaByMadouong(string madouong)
        {
            Giadouong latestGia = null;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string selectSql = @"
                        SELECT Magia, Madouong, Giaban, Thoigianapdung
                        FROM Giadouong
                        WHERE Madouong = @Madouong
                        ORDER BY Thoigianapdung DESC
                        LIMIT 1";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madouong", madouong);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Vì LIMIT 1 nên chỉ cần if
                            if (reader.Read())
                            {
                                latestGia = new Giadouong
                                {
                                    Magia = reader["Magia"].ToString(),
                                    Madouong = reader["Madouong"].ToString(),
                                    Giaban = Convert.ToDecimal(reader["Giaban"]),
                                    Thoigianapdung = DateTime.Parse(reader["Thoigianapdung"].ToString())
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(
                        $"Lỗi DAL khi lấy giá mới nhất cho đồ uống '{madouong}': {ex.Message}", ex);
                }
            }

            return latestGia;
        }

        /// <summary>
        /// Thêm một bản ghi giá mới cho đồ uống.
        /// </summary>
        /// <param name="giadouong">Đối tượng Giadouong cần thêm.</param>
        public void AddGiadouong(Giadouong giadouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Giadouong (Magia, Madouong, Giaban, Thoigianapdung) 
                        VALUES (@Magia, @Madouong, @Giaban, @Thoigianapdung)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Magia", giadouong.Magia);
                        command.Parameters.AddWithValue("@Madouong", giadouong.Madouong);
                        command.Parameters.AddWithValue("@Giaban", giadouong.Giaban);
                        command.Parameters.AddWithValue(
                            "@Thoigianapdung",
                            giadouong.Thoigianapdung.ToString("yyyy-MM-dd HH:mm:ss"));

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi thêm giá đồ uống: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Lấy giá hiện tại (mới nhất) của TẤT CẢ đồ uống.
        /// Mỗi Madouong chỉ trả về 1 bản ghi mới nhất.
        /// </summary>
        /// <returns>Danh sách Giadouong, mỗi phần tử là giá mới nhất của 1 đồ uống.</returns>
        public List<Giadouong> GetAllCurrentPrices()
        {
            List<Giadouong> giadouongs = new List<Giadouong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Câu SQL lấy giá có Thoigianapdung MAX cho mỗi Madouong
                    string selectSql = @"
                        SELECT 
                            g1.Magia, 
                            g1.Madouong, 
                            g1.Giaban, 
                            g1.Thoigianapdung
                        FROM 
                            Giadouong g1
                        INNER JOIN (
                            SELECT 
                                Madouong, 
                                MAX(Thoigianapdung) AS MaxTime
                            FROM 
                                Giadouong
                            GROUP BY 
                                Madouong
                        ) g2 ON g1.Madouong = g2.Madouong 
                           AND g1.Thoigianapdung = g2.MaxTime;";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Giadouong giadouong = new Giadouong
                            {
                                Magia = reader["Magia"].ToString(),
                                Madouong = reader["Madouong"].ToString(),
                                Giaban = Convert.ToDecimal(reader["Giaban"]),
                                Thoigianapdung = DateTime.Parse(reader["Thoigianapdung"].ToString())
                            };
                            giadouongs.Add(giadouong);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy danh sách giá mới nhất: {ex.Message}", ex);
                }
            }

            return giadouongs;
        }

        /// <summary>
        /// Tìm kiếm tên đồ uống (LIKE) và trả về danh sách đồ uống kèm giá mới nhất.
        /// Thường dùng cho màn hình tìm kiếm tổng hợp (join Douong + Giadouong).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm theo tên đồ uống.</param>
        /// <returns>Danh sách Douong đã gán giá bán hiện tại.</returns>
        public List<Douong> SearchAllDouongs(string searchTerm)
        {
            List<Douong> products = new List<Douong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string selectSql = @"
                        SELECT 
                            d.Madouong, 
                            d.Tendouong, 
                            d.Maloai, 
                            d.Hinhanh, 
                            d.Mota,
                            g1.Giaban 
                        FROM 
                            Douong d
                        INNER JOIN 
                            Giadouong g1 ON d.Madouong = g1.Madouong
                        INNER JOIN (
                            SELECT 
                                Madouong, 
                                MAX(Thoigianapdung) AS MaxTime
                            FROM 
                                Giadouong
                            GROUP BY 
                                Madouong
                        ) g2 ON g1.Madouong = g2.Madouong 
                           AND g1.Thoigianapdung = g2.MaxTime
                        WHERE 
                            d.Tendouong LIKE @SearchTerm COLLATE NOCASE;";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Douong product = new Douong
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"].ToString(),
                                    Hinhanh = reader["Hinhanh"].ToString(),
                                    Mota = reader["Mota"].ToString(),
                                    Giaban = Convert.ToDecimal(reader["Giaban"])
                                };

                                products.Add(product);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi tìm kiếm đồ uống (kèm giá): {ex.Message}", ex);
                }
            }

            return products;
        }

        // Ghi chú:
        // Có thể bổ sung Update/Delete cho bảng Giadouong nếu muốn sửa/xóa bản ghi giá.
        // Tuy nhiên thực tế thường chỉ thêm bản ghi mới để lưu lịch sử thay đổi giá.
    }
}
