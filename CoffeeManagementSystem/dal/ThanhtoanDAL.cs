// Trong ThanhtoanDAL.cs
using CoffeeManagementSystem.Utilities; // Đảm bảo có Logger
using System;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    public class ThanhtoanDAL : BaseDataAccess
    {
        public ThanhtoanDAL() : base() { }
        public void AddThanhtoan(Thanhtoan thanhtoan, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            Logger.LogInfo($"Bắt đầu thêm thanh toán cho đơn hàng: {thanhtoan.Madonhang}");
            string query = "INSERT INTO Thanhtoan (Mathanhtoan, Madonhang, Thoigianthanhtoan, Hinhthucthanhtoan, Sotienthanhtoan, Manhanvienthu, Ghichu) " +
                           "VALUES (@Mathanhtoan, @Madonhang, @Thoigianthanhtoan, @Hinhthucthanhtoan, @Sotienthanhtoan, @Manhanvienthu, @Ghichu)";

            try
            {
                using (SQLiteCommand command = new SQLiteCommand(query, connection, transaction)) // Sử dụng connection và transaction
                {
                    command.Parameters.AddWithValue("@Mathanhtoan", thanhtoan.Mathanhtoan);
                    command.Parameters.AddWithValue("@Madonhang", thanhtoan.Madonhang);
                    command.Parameters.AddWithValue("@Thoigianthanhtoan", thanhtoan.Thoigianthanhtoan);
                    command.Parameters.AddWithValue("@Hinhthucthanhtoan", thanhtoan.Hinhthucthanhtoan);
                    command.Parameters.AddWithValue("@Sotienthanhtoan", thanhtoan.Sotienthanhtoan);
                    command.Parameters.AddWithValue("@Manhanvienthu", thanhtoan.Manhanvienthu);
                    command.Parameters.AddWithValue("@Ghichu", thanhtoan.Ghichu ?? (object)DBNull.Value); // Xử lý Ghichu có thể null

                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Logger.LogInfo($"Đã thêm thanh toán thành công với mã: {thanhtoan.Mathanhtoan}");
                    }
                    else
                    {
                        Logger.LogWarning($"Không có dòng nào bị ảnh hưởng khi thêm thanh toán {thanhtoan.Mathanhtoan}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Lỗi khi thêm thanh toán vào CSDL: {thanhtoan.Mathanhtoan}", ex);
                throw new Exception($"Lỗi khi thêm thanh toán: {ex.Message}", ex);
            }
        }

        // --- Cần thêm phương thức GetLatestThanhtoanId() để tạo ID tự tăng TTxxx ---
        public string GetLatestThanhtoanId()
        {
            string latestId = null;
            string query = "SELECT Mathanhtoan FROM Thanhtoan ORDER BY Mathanhtoan DESC LIMIT 1";

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
                            Logger.LogDebug($"Đã lấy mã thanh toán lớn nhất: {latestId}");
                        }
                        else
                        {
                            Logger.LogDebug("Không có bản ghi thanh toán nào trong CSDL.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError("Lỗi khi lấy mã thanh toán lớn nhất.", ex);
                throw new Exception("Lỗi khi lấy mã thanh toán lớn nhất: " + ex.Message, ex);
            }
            return latestId;
        }
        public Thanhtoan GetLatestByMadonhang(string madonhang)
        {
            Thanhtoan result = null;

            string sql = @"
                SELECT Mathanhtoan, Madonhang, Thoigianthanhtoan,
                       Hinhthucthanhtoan, Sotienthanhtoan, Manhanvienthu, Ghichu
                FROM Thanhtoan
                WHERE Madonhang = @Madonhang
                ORDER BY Thoigianthanhtoan DESC
                LIMIT 1";

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Madonhang", madonhang);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = new Thanhtoan
                        {
                            Mathanhtoan = reader["Mathanhtoan"].ToString(),
                            Madonhang = reader["Madonhang"].ToString(),
                            Thoigianthanhtoan = DateTime.Parse(reader["Thoigianthanhtoan"].ToString()),
                            Hinhthucthanhtoan = reader["Hinhthucthanhtoan"].ToString(),
                            Sotienthanhtoan = Convert.ToDecimal(reader["Sotienthanhtoan"]),
                            Manhanvienthu = reader["Manhanvienthu"].ToString(),
                            Ghichu = reader["Ghichu"] == DBNull.Value ? null : reader["Ghichu"].ToString()
                        };
                    }
                }
            }

            return result;
        }
        // ... (các phương thức khác của ThanhtoanDAL nếu có) ...
    }
}