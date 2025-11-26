using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms; // Only used for MessageBox in error handling examples

namespace CoffeeManagementSystem.DAL
{
    public class KhachhangDAL : BaseDataAccess
    {
        public KhachhangDAL() : base() { }

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns>A list of Khachhang objects.</returns>
        public List<Khachhang> GetAllKhachhangs()
        {
            List<Khachhang> khachhangs = new List<Khachhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SELECT statement matching your provided structure
                    string selectSql = "SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy FROM Khachhang";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Khachhang khachhang = new Khachhang
                                {
                                    Makhachhang = reader["Makhachhang"].ToString(),
                                    Hoten = reader["Hoten"].ToString(),
                                    Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                    Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                    Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                                };
                                khachhangs.Add(khachhang);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy danh sách khách hàng: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw; // Optional: re-throw for higher-level error handling
                }
            }
            return khachhangs;
        }

        /// <summary>
        /// Retrieves customer information by ID.
        /// </summary>
        /// <param name="makhachhang">The ID of the customer to retrieve.</param>
        /// <returns>A Khachhang object if found, otherwise null.</returns>
        public Khachhang GetKhachhangById(string makhachhang)
        {
            Khachhang khachhang = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SELECT statement matching your provided structure
                    string selectSql = "SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy FROM Khachhang WHERE Makhachhang = @Makhachhang";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Makhachhang", makhachhang);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                khachhang = new Khachhang
                                {
                                    Makhachhang = reader["Makhachhang"].ToString(),
                                    Hoten = reader["Hoten"].ToString(),
                                    Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                    Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                    Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy khách hàng theo ID: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw;
                }
            }
            return khachhang;
        }

        /// <summary>
        /// Searches for a customer by their name (case-insensitive).
        /// This method was added for the auto-save customer feature.
        /// </summary>
        /// <param name="tenKhachhang">The name of the customer to search for.</param>
        /// <returns>A Khachhang object if found, otherwise null.</returns>
        public Khachhang GetKhachhangByName(string tenKhachhang)
        {
            Khachhang khachhang = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Compare case-insensitively using LOWER()
                    // SELECT statement matching your provided structure
                    string selectSql = "SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy FROM Khachhang WHERE LOWER(Hoten) = LOWER(@Tenkhachhang)";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Tenkhachhang", tenKhachhang);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                khachhang = new Khachhang
                                {
                                    Makhachhang = reader["Makhachhang"].ToString(),
                                    Hoten = reader["Hoten"].ToString(),
                                    Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                    Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                    Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm khách hàng theo tên: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw; // Re-throw to allow calling layer to handle
                }
            }
            return khachhang;
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="khachhang">The Khachhang object to add.</param>
        public void AddKhachhang(Khachhang khachhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // INSERT statement matching your provided structure
                    string insertSql = @"
                    INSERT INTO Khachhang (Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy)
                    VALUES (@Makhachhang, @Hoten, @Sodienthoai, @Email, @Ngaydangky, @Diemtichluy)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Makhachhang", khachhang.Makhachhang);
                        command.Parameters.AddWithValue("@Hoten", khachhang.Hoten);
                        command.Parameters.AddWithValue("@Sodienthoai", (object)khachhang.Sodienthoai ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object)khachhang.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Ngaydangky", khachhang.Ngaydangky.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Diemtichluy", khachhang.Diemtichluy);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi thêm khách hàng: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw;
                }
            }
        }

        /// <summary>
        /// Updates the information of a customer.
        /// </summary>
        /// <param name="khachhang">The Khachhang object containing updated information (Makhachhang is required).</param>
        public void UpdateKhachhang(Khachhang khachhang)
        {
            Logger.LogDebug($"Đang cập nhật khách hàng độc lập: Mã='{khachhang.Makhachhang}', Tên='{khachhang.Hoten}'.");
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // UPDATE statement matching your provided structure
                    string updateSql = @"
                    UPDATE Khachhang
                    SET Hoten = @Hoten,
                        Sodienthoai = @Sodienthoai,
                        Email = @Email,
                        Ngaydangky = @Ngaydangky,
                        Diemtichluy = @Diemtichluy
                    WHERE Makhachhang = @Makhachhang";

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Hoten", khachhang.Hoten);
                        command.Parameters.AddWithValue("@Sodienthoai", (object)khachhang.Sodienthoai ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object)khachhang.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Ngaydangky", khachhang.Ngaydangky.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Diemtichluy", khachhang.Diemtichluy);
                        command.Parameters.AddWithValue("@Makhachhang", khachhang.Makhachhang);

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Lỗi khi cập nhật khách hàng '{khachhang.Hoten}' (Mã: {khachhang.Makhachhang}) độc lập.", ex);
                    MessageBox.Show($"Lỗi khi cập nhật khách hàng: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw;
                }
            }
        }
        public void UpdateKhachhang(Khachhang khachhang, SQLiteConnection connection, SQLiteTransaction transaction) // <-- PHIÊN BẢN CẦN THÊM
        {
            // LOG: Bắt đầu cập nhật khách hàng trong transaction
            Logger.LogDebug($"Đang cập nhật khách hàng trong transaction: Mã='{khachhang.Makhachhang}', Tên='{khachhang.Hoten}', Điểm mới={khachhang.Diemtichluy}.");
            try
            {
                string query = "UPDATE Khachhang SET Hoten = @Hoten, Ngaydangky = @Ngaydangky, Diemtichluy = @Diemtichluy WHERE Makhachhang = @Makhachhang";
                using (SQLiteCommand command = new SQLiteCommand(query, connection, transaction)) // Truyền transaction vào command
                {
                    command.Parameters.AddWithValue("@Hoten", khachhang.Hoten);
                    command.Parameters.AddWithValue("@Ngaydangky", khachhang.Ngaydangky.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Diemtichluy", khachhang.Diemtichluy);
                    command.Parameters.AddWithValue("@Makhachhang", khachhang.Makhachhang);
                    command.ExecuteNonQuery();
                }
                Logger.LogInfo($"Đã cập nhật khách hàng '{khachhang.Hoten}' (Mã: {khachhang.Makhachhang}) trong transaction thành công.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Lỗi khi cập nhật khách hàng '{khachhang.Hoten}' (Mã: {khachhang.Makhachhang}) trong transaction.", ex);
                throw new Exception($"Lỗi khi cập nhật khách hàng trong transaction: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="makhachhang">The ID of the customer to delete.</param>
        public void DeleteKhachhang(string makhachhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Khachhang WHERE Makhachhang = @Makhachhang";

                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Makhachhang", makhachhang);

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa khách hàng: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw;
                }
            }
        }

        /// <summary>
        /// Searches for customers based on a keyword in Makhachhang, Hoten, Sodienthoai, Email columns.
        /// </summary>
        /// <param name="searchTerm">The search keyword.</param>
        /// <returns>A list of matching Khachhang objects.</returns>
        public List<Khachhang> SearchKhachhangs(string searchTerm)
        {
            List<Khachhang> khachhangs = new List<Khachhang>();

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return khachhangs;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // SELECT statement with search conditions matching your provided structure
                    string selectSql = @"
                    SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy
                    FROM Khachhang
                    WHERE LOWER(Makhachhang) LIKE @SearchTerm
                        OR LOWER(Hoten) LIKE @SearchTerm
                        OR LOWER(Sodienthoai) LIKE @SearchTerm
                        OR LOWER(Email) LIKE @SearchTerm";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Khachhang khachhang = new Khachhang
                                {
                                    Makhachhang = reader["Makhachhang"].ToString(),
                                    Hoten = reader["Hoten"].ToString(),
                                    Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                                    Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                    Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                    Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                                };
                                khachhangs.Add(khachhang);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tìm kiếm khách hàng: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // throw;
                }
            }
            return khachhangs;
        }

        public List<Khachhang> GetTop10HighestDiemTichLuyCustomers() // Đã đổi tên phương thức
        {
            List<Khachhang> customers = new List<Khachhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy
                        FROM Khachhang
                        ORDER BY DiemTichLuy DESC
                        LIMIT 10;"; // Chỉ sắp xếp và lấy TOP 10

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Đã loại bỏ: command.Parameters.AddWithValue("@MinDiemTichLuy", minDiemTichLuy);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                customers.Add(new Khachhang
                                {
                                    Makhachhang = reader["Makhachhang"].ToString(),
                                    Hoten = reader["Hoten"].ToString(),
                                    Sodienthoai = reader["Sodienthoai"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                    Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy TOP 10 khách hàng điểm cao nhất: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return customers;
        }
        public string GetLatestKhachhangId()
        {
            string latestId = null;
            string query = "SELECT Makhachhang FROM Khachhang ORDER BY Makhachhang DESC LIMIT 1";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            latestId = result.ToString();
                            Logger.LogDebug($"Đã lấy mã khách hàng lớn nhất: {latestId}");
                        }
                        else
                        {
                            Logger.LogDebug("Không có khách hàng nào trong CSDL.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Lỗi khi lấy mã khách hàng lớn nhất.", ex);
                throw new Exception("Lỗi khi lấy mã khách hàng lớn nhất: " + ex.Message, ex);
            }
            return latestId;
        }
    }
}
