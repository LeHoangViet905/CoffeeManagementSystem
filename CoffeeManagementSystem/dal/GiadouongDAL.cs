using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms; // Only for MessageBox in error handling examples

// Ensure using namespace contains your BaseDataAccess class
// Ensure using namespace contains your Giadouong Model class

namespace CoffeeManagementSystem.DAL
{
    public class GiadouongDAL : BaseDataAccess
    {
        public GiadouongDAL() : base() { }

        /// <summary>
        /// Retrieves all price records for drinks from the database.
        /// </summary>
        /// <returns>A list of Giadouong objects.</returns>
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
                    {
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy danh sách giá đồ uống: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return giadouongs;
        }

        /// <summary>
        /// Retrieves the latest price for a specific drink.
        /// </summary>
        /// <param name="madouong">The ID of the drink.</param>
        /// <returns>The latest Giadouong object for the given drink, or null if not found.</returns>
        public Giadouong GetLatestGiaByMadouong(string madouong)
        {
            Giadouong latestGia = null; // <- Khai báo 1 đối tượng
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // 1. DÙNG CÂU SQL GỐC (LẤY 1 MÓN)
                    string selectSql = @"
                        SELECT Magia, Madouong, Giaban, Thoigianapdung
                        FROM Giadouong
                        WHERE Madouong = @Madouong
                        ORDER BY Thoigianapdung DESC
                        LIMIT 1"; // <-- Chỉ lấy 1

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // 2. THÊM PARAMETER (RẤT QUAN TRỌNG)
                        command.Parameters.AddWithValue("@Madouong", madouong);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // 3. DÙNG 'if' (VÌ CHỈ CÓ 1 KẾT QUẢ)
                            if (reader.Read())
                            {
                                // 4. GÁN VÀO 'latestGia'
                                latestGia = new Giadouong
                                {
                                    Magia = reader["Magia"].ToString(),
                                    Madouong = reader["Madouong"].ToString(),
                                    Giaban = Convert.ToDecimal(reader["Giaban"]),
                                    Thoigianapdung = DateTime.Parse(reader["Thoigianapdung"].ToString())
                                };
                                // (Không dùng 'giadouongs.Add' ở đây)
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Sửa lại câu thông báo lỗi cho đúng
                    MessageBox.Show($"Lỗi khi lấy giá mới nhất cho đồ uống '{madouong}': {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // 5. TRẢ VỀ 1 ĐỐI TƯỢNG
            return latestGia;
        }

        /// <summary>
        /// Adds a new price record for a drink to the database.
        /// </summary>
        /// <param name="giadouong">The Giadouong object to add.</param>
        public void AddGiadouong(Giadouong giadouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = "INSERT INTO Giadouong (Magia, Madouong, Giaban, Thoigianapdung) VALUES (@Magia, @Madouong, @Giaban, @Thoigianapdung)";
                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Magia", giadouong.Magia);
                        command.Parameters.AddWithValue("@Madouong", giadouong.Madouong);
                        command.Parameters.AddWithValue("@Giaban", giadouong.Giaban);
                        command.Parameters.AddWithValue("@Thoigianapdung", giadouong.Thoigianapdung.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm giá đồ uống: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        /// <summary>
        /// Retrieves the single, most current price for ALL drinks.
        /// (Lấy giá mới nhất của TẤT CẢ đồ uống)
        /// </summary>
        /// <returns>A list of Giadouong objects, each representing the latest price for a drink.</returns>
        public List<Giadouong> GetAllCurrentPrices()
        {
            // Di chuyển khai báo ra ngoài 'try'
            List<Giadouong> giadouongs = new List<Giadouong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Câu SQL này sẽ tìm giá có Thoigianapdung mới nhất cho mỗi Madouong
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
                        ) g2 ON g1.Madouong = g2.Madouong AND g1.Thoigianapdung = g2.MaxTime;
                    ";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
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
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy danh sách giá mới nhất: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return giadouongs;
        }
        /// <summary>
        /// Tìm kiếm Tên đồ uống (LIKE) và trả về giá mới nhất của chúng
        /// </summary>
        public List<Douong> SearchAllDouongs(string searchTerm)
        {
            // BƯỚC 2: Di chuyển khai báo ra ngoài 'try' VÀ sửa kiểu
            List<Douong> products = new List<Douong>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                SELECT 
                    d.Madouong, d.Tendouong, d.Maloai, d.Hinhanh, d.Mota,
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
                ) g2 ON g1.Madouong = g2.Madouong AND g1.Thoigianapdung = g2.MaxTime
                WHERE 
                    d.Tendouong LIKE @SearchTerm COLLATE NOCASE; 
            ";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Logic này của bạn đã đúng (Tạo đối tượng 'Douong')
                                Douong product = new Douong
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"].ToString(),
                                    Hinhanh = reader["Hinhanh"].ToString(),
                                    Mota = reader["Mota"].ToString(),
                                    Giaban = Convert.ToDecimal(reader["Giaban"])
                                };

                                // BƯỚC 3: Thêm vào list 'products' (hết lỗi typo)
                                products.Add(product);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm đồ uống: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            //Trả về list 'products'
            return products;
        }

        // You can add Update and Delete methods for Giadouong if needed.
        // For simplicity, we might only add new price records rather than updating old ones.
    }
}