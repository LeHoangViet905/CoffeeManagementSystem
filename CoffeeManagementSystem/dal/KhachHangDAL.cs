using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    public class KhachhangDAL : BaseDataAccess
    {
        private readonly string _connectionString = @"DataSource=QuanLyCaPheDatabase.db;Version=3;";
        public KhachhangDAL() : base() { }

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        public List<Khachhang> GetAllKhachhangs()
        {
            List<Khachhang> khachhangs = new List<Khachhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy 
                        FROM Khachhang";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
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
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy danh sách khách hàng: {ex.Message}", ex);
                }
            }
            return khachhangs;
        }

        /// <summary>
        /// Retrieves customer information by ID.
        /// </summary>
        public Khachhang GetKhachhangById(string makhachhang)
        {
            Khachhang khachhang = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy 
                        FROM Khachhang 
                        WHERE Makhachhang = @Makhachhang";

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
                    throw new Exception($"Lỗi DAL khi lấy khách hàng theo ID: {ex.Message}", ex);
                }
            }
            return khachhang;
        }

        /// <summary>
        /// Searches for a customer by their name (case-insensitive).
        /// </summary>
        public Khachhang GetKhachhangByName(string tenKhachhang)
        {
            Khachhang khachhang = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy 
                        FROM Khachhang 
                        WHERE LOWER(Hoten) = LOWER(@Tenkhachhang)";

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
                    throw new Exception($"Lỗi DAL khi tìm khách hàng theo tên: {ex.Message}", ex);
                }
            }
            return khachhang;
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        public void AddKhachhang(Khachhang khachhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Khachhang (Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy)
                        VALUES (@Makhachhang, @Hoten, @Sodienthoai, @Email, @Ngaydangky, @Diemtichluy)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Makhachhang", khachhang.Makhachhang);
                        command.Parameters.AddWithValue("@Hoten", khachhang.Hoten);
                        command.Parameters.AddWithValue("@Sodienthoai", (object)khachhang.Sodienthoai ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Email", (object)khachhang.Email ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Ngaydangky", khachhang.Ngaydangky.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@Diemtichluy", khachhang.Diemtichluy);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi thêm khách hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Updates the information of a customer (standalone).
        /// </summary>
        public void UpdateKhachhang(Khachhang khachhang)
        {
            Logger.LogDebug($"Đang cập nhật khách hàng độc lập: Mã='{khachhang.Makhachhang}', Tên='{khachhang.Hoten}'.");

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Khachhang
                        SET Hoten       = @Hoten,
                            Sodienthoai = @Sodienthoai,
                            Email       = @Email,
                            Ngaydangky  = @Ngaydangky,
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

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Lỗi khi cập nhật khách hàng '{khachhang.Hoten}' (Mã: {khachhang.Makhachhang}) độc lập.", ex);
                    throw new Exception($"Lỗi DAL khi cập nhật khách hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Updates a customer within an existing transaction.
        /// </summary>
        public void UpdateKhachhang(Khachhang khachhang, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            Logger.LogDebug($"Đang cập nhật khách hàng trong transaction: Mã='{khachhang.Makhachhang}', Tên='{khachhang.Hoten}', Điểm mới={khachhang.Diemtichluy}.");
            try
            {
                string query = @"
                    UPDATE Khachhang 
                    SET Hoten       = @Hoten, 
                        Ngaydangky  = @Ngaydangky, 
                        Diemtichluy = @Diemtichluy 
                    WHERE Makhachhang = @Makhachhang";

                using (SQLiteCommand command = new SQLiteCommand(query, connection, transaction))
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
                throw new Exception($"Lỗi DAL khi cập nhật khách hàng trong transaction: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
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
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi xóa khách hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Searches for customers by keyword in ID / Name / Phone / Email.
        /// </summary>
        public List<Khachhang> SearchKhachhangs(string searchTerm)
        {
            List<Khachhang> khachhangs = new List<Khachhang>();

            if (string.IsNullOrWhiteSpace(searchTerm))
                return khachhangs;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy
                        FROM Khachhang
                        WHERE LOWER(Makhachhang) LIKE @SearchTerm
                           OR LOWER(Hoten)       LIKE @SearchTerm
                           OR LOWER(Sodienthoai) LIKE @SearchTerm
                           OR LOWER(Email)       LIKE @SearchTerm";

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
                    throw new Exception($"Lỗi DAL khi tìm kiếm khách hàng: {ex.Message}", ex);
                }
            }
            return khachhangs;
        }

        /// <summary>
        /// Gets TOP 10 customers with highest points.
        /// </summary>
        public List<Khachhang> GetTop10HighestDiemTichLuyCustomers()
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
                        LIMIT 10";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            customers.Add(new Khachhang
                            {
                                Makhachhang = reader["Makhachhang"].ToString(),
                                Hoten = reader["Hoten"].ToString(),
                                Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                                Ngaydangky = DateTime.Parse(reader["Ngaydangky"].ToString()),
                                Diemtichluy = Convert.ToInt32(reader["Diemtichluy"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy TOP 10 khách hàng điểm cao nhất: {ex.Message}", ex);
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
                throw new Exception("Lỗi DAL khi lấy mã khách hàng lớn nhất: " + ex.Message, ex);
            }
            return latestId;
        }

        public List<string> GetAllMaKhachhang()
        {
            List<string> maList = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Makhachhang FROM Khachhang", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maList.Add(reader["Makhachhang"].ToString());
                    }
                }
            }
            return maList;
        }

        public void ImportKhachhangs(List<Khachhang> khachhangs)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var k in khachhangs)
                        {
                            string checkSql = "SELECT COUNT(1) FROM Khachhang WHERE Makhachhang = @Makhachhang";
                            using (var cmdCheck = new SQLiteCommand(checkSql, connection, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Makhachhang", k.Makhachhang);
                                long count = (long)cmdCheck.ExecuteScalar();

                                if (count > 0)
                                {
                                    // ⚠️ BUG FIX: thêm WHERE Makhachhang = @Makhachhang
                                    string updateSql = @"
                                        UPDATE Khachhang 
                                        SET Hoten       = @Hoten,
                                            Sodienthoai = @Sodienthoai,
                                            Email       = @Email,
                                            Ngaydangky  = @Ngaydangky,
                                            Diemtichluy = @Diemtichluy
                                        WHERE Makhachhang = @Makhachhang";

                                    using (var cmdUpdate = new SQLiteCommand(updateSql, connection, transaction))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@Makhachhang", k.Makhachhang);
                                        cmdUpdate.Parameters.AddWithValue("@Hoten", k.Hoten);
                                        cmdUpdate.Parameters.AddWithValue("@Sodienthoai", (object)k.Sodienthoai ?? DBNull.Value);
                                        cmdUpdate.Parameters.AddWithValue("@Email", (object)k.Email ?? DBNull.Value);
                                        cmdUpdate.Parameters.AddWithValue("@Ngaydangky", k.Ngaydangky);
                                        cmdUpdate.Parameters.AddWithValue("@Diemtichluy", k.Diemtichluy);
                                        cmdUpdate.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    string insertSql = @"
                                        INSERT INTO Khachhang 
                                            (Makhachhang, Hoten, Sodienthoai, Email, Ngaydangky, Diemtichluy) 
                                        VALUES 
                                            (@Makhachhang, @Hoten, @Sodienthoai, @Email, @Ngaydangky, @Diemtichluy)";

                                    using (var cmdInsert = new SQLiteCommand(insertSql, connection, transaction))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@Makhachhang", k.Makhachhang);
                                        cmdInsert.Parameters.AddWithValue("@Hoten", k.Hoten);
                                        cmdInsert.Parameters.AddWithValue("@Sodienthoai", (object)k.Sodienthoai ?? DBNull.Value);
                                        cmdInsert.Parameters.AddWithValue("@Email", (object)k.Email ?? DBNull.Value);
                                        cmdInsert.Parameters.AddWithValue("@Ngaydangky", k.Ngaydangky);
                                        cmdInsert.Parameters.AddWithValue("@Diemtichluy", k.Diemtichluy);
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
                        throw new Exception("Lỗi DAL khi import nhiều khách hàng: " + ex.Message, ex);
                    }
                }
            }
        }
    }
}
