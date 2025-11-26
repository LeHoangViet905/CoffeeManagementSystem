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
            Giadouong latestGia = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Select the price with the latest 'Thoigianapdung' for the given drink
                    string selectSql = @"
                        SELECT Magia, Madouong, Giaban, Thoigianapdung
                        FROM Giadouong
                        WHERE Madouong = @Madouong
                        ORDER BY Thoigianapdung DESC
                        LIMIT 1"; // LIMIT 1 to get only the latest one

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madouong", madouong);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
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
                    MessageBox.Show($"Lỗi khi lấy giá mới nhất cho đồ uống '{madouong}': {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
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

        // You can add Update and Delete methods for Giadouong if needed.
        // For simplicity, we might only add new price records rather than updating old ones.
    }
}
