using System;
using System.Collections.Generic;
using System.Data.SQLite;

// Đảm bảo using namespace chứa BaseDataAccess và Chitietdonhang Model

namespace CoffeeManagementSystem.DAL
{
    public class ChitietdonhangDAL : BaseDataAccess
    {
        public ChitietdonhangDAL() : base() { }

        /// <summary>
        /// Thêm một chi tiết đơn hàng mới vào CSDL.
        /// </summary>
        /// <param name="chitiet">Đối tượng Chitietdonhang cần thêm.</param>
        /// <remarks>
        /// Để Madonhang và Madouong hoạt động như khóa chính kép,
        /// đảm bảo bảng Chitietdonhang trong CSDL SQLite của bạn
        /// được định nghĩa với PRIMARY KEY (Madonhang, Madouong).
        /// Ví dụ: CREATE TABLE Chitietdonhang (..., PRIMARY KEY (Madonhang, Madouong));
        /// </remarks>
        public void AddChitietdonhang(Chitietdonhang chitiet)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    // Câu lệnh INSERT INTO đã loại bỏ cột Machitiet
                    string insertSql = @"
                        INSERT INTO Chitietdonhang (Madonhang, Madouong, Soluong, Dongia, Thanhtien, Ghichu)
                        VALUES (@Madonhang, @Madouong, @Soluong, @Dongia, @Thanhtien, @Ghichu)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", chitiet.Madonhang);
                        command.Parameters.AddWithValue("@Madouong", chitiet.Madouong);
                        command.Parameters.AddWithValue("@Soluong", chitiet.Soluong);
                        command.Parameters.AddWithValue("@Dongia", chitiet.Dongia);
                        command.Parameters.AddWithValue("@Thanhtien", chitiet.Thanhtien);
                        // Xử lý cột Ghichu có thể NULL
                        command.Parameters.AddWithValue("@Ghichu", string.IsNullOrEmpty(chitiet.Ghichu) ? (object)DBNull.Value : chitiet.Ghichu);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi thêm chi tiết đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Thêm một chi tiết đơn hàng mới vào CSDL trong một transaction.
        /// Phương thức này được sử dụng bởi BLL để đảm bảo tính toàn vẹn dữ liệu.
        /// </summary>
        /// <param name="chitiet">Đối tượng Chitietdonhang cần thêm.</param>
        /// <param name="connection">Kết nối SQLite hiện có (để sử dụng transaction).</param>
        /// <param name="transaction">Transaction SQLite hiện có.</param>
        public void AddChitietdonhang(Chitietdonhang chitiet, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            try
            {
                string insertSql = @"
                    INSERT INTO Chitietdonhang (Madonhang, Madouong, Soluong, Dongia, Thanhtien, Ghichu)
                    VALUES (@Madonhang, @Madouong, @Soluong, @Dongia, @Thanhtien, @Ghichu)";

                using (SQLiteCommand command = new SQLiteCommand(insertSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@Madonhang", chitiet.Madonhang);
                    command.Parameters.AddWithValue("@Madouong", chitiet.Madouong);
                    command.Parameters.AddWithValue("@Soluong", chitiet.Soluong);
                    command.Parameters.AddWithValue("@Dongia", chitiet.Dongia);
                    command.Parameters.AddWithValue("@Thanhtien", chitiet.Thanhtien);
                    command.Parameters.AddWithValue("@Ghichu", string.IsNullOrEmpty(chitiet.Ghichu) ? (object)DBNull.Value : chitiet.Ghichu);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Ném lỗi để BLL xử lý rollback, không hiển thị MessageBox ở đây.
                throw new Exception($"Lỗi DAL khi thêm chi tiết đơn hàng trong transaction: {ex.Message}", ex);
            }
        }

        // Bạn cũng cần cập nhật các phương thức khác (GetAll, GetById, Update, Delete)
        // để đảm bảo chúng không truy cập cột Machitiet nữa.
        // Ví dụ cho GetAllChitietdonhangs:
        public List<Chitietdonhang> GetAllChitietdonhangs()
        {
            List<Chitietdonhang> chiTietDonHangs = new List<Chitietdonhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madonhang, Madouong, Soluong, Dongia, Thanhtien, Ghichu FROM Chitietdonhang"; // Loại bỏ Machitiet
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Chitietdonhang chiTiet = new Chitietdonhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Madouong = reader["Madouong"].ToString(),
                                    Soluong = Convert.ToInt32(reader["Soluong"]),
                                    Dongia = Convert.ToDecimal(reader["Dongia"]),
                                    Thanhtien = Convert.ToDecimal(reader["Thanhtien"]),
                                    Ghichu = reader["Ghichu"] == DBNull.Value ? null : reader["Ghichu"].ToString()
                                };
                                chiTietDonHangs.Add(chiTiet);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi lấy danh sách chi tiết đơn hàng: {ex.Message}", ex);
                }
            }
            return chiTietDonHangs;
        }

        /// <summary>
        /// Lấy một chi tiết đơn hàng cụ thể bằng khóa chính kép (Madonhang, Madouong).
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng.</param>
        /// <param name="madouong">Mã đồ uống.</param>
        /// <returns>Đối tượng Chitietdonhang nếu tìm thấy, ngược lại null.</returns>
        public Chitietdonhang GetChitietdonhangByIds(string madonhang, string madouong)
        {
            Chitietdonhang chiTiet = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madonhang, Madouong, Soluong, Dongia, Thanhtien, Ghichu FROM Chitietdonhang WHERE Madonhang = @Madonhang AND Madouong = @Madouong";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", madonhang);
                        command.Parameters.AddWithValue("@Madouong", madouong);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                chiTiet = new Chitietdonhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Madouong = reader["Madouong"].ToString(),
                                    Soluong = Convert.ToInt32(reader["Soluong"]),
                                    Dongia = Convert.ToDecimal(reader["Dongia"]),
                                    Thanhtien = Convert.ToDecimal(reader["Thanhtien"]),
                                    Ghichu = reader["Ghichu"] == DBNull.Value ? null : reader["Ghichu"].ToString()
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi lấy chi tiết đơn hàng theo khóa kép: {ex.Message}", ex);
                }
            }
            return chiTiet;
        }

        // Các phương thức khác như UpdateChitietdonhang, DeleteChitietdonhang
        // cũng cần được cập nhật để sử dụng cả Madonhang và Madouong làm điều kiện WHERE.

        /// <summary>
        /// Cập nhật thông tin của một chi tiết đơn hàng.
        /// </summary>
        /// <param name="chitiet">Đối tượng Chitietdonhang chứa thông tin cập nhật.</param>
        public void UpdateChitietdonhang(Chitietdonhang chitiet)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Chitietdonhang
                        SET Soluong = @Soluong,
                            Dongia = @Dongia,
                            Thanhtien = @Thanhtien,
                            Ghichu = @Ghichu
                        WHERE Madonhang = @Madonhang AND Madouong = @Madouong"; // Điều kiện WHERE dùng khóa chính kép

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Soluong", chitiet.Soluong);
                        command.Parameters.AddWithValue("@Dongia", chitiet.Dongia);
                        command.Parameters.AddWithValue("@Thanhtien", chitiet.Thanhtien);
                        command.Parameters.AddWithValue("@Ghichu", string.IsNullOrEmpty(chitiet.Ghichu) ? (object)DBNull.Value : chitiet.Ghichu);
                        command.Parameters.AddWithValue("@Madonhang", chitiet.Madonhang);
                        command.Parameters.AddWithValue("@Madouong", chitiet.Madouong);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi cập nhật chi tiết đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Xóa một chi tiết đơn hàng khỏi CSDL.
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng của chi tiết cần xóa.</param>
        /// <param name="madouong">Mã đồ uống của chi tiết cần xóa.</param>
        public void DeleteChitietdonhang(string madonhang, string madouong)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Chitietdonhang WHERE Madonhang = @Madonhang AND Madouong = @Madouong"; // Điều kiện WHERE dùng khóa chính kép

                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", madonhang);
                        command.Parameters.AddWithValue("@Madouong", madouong);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi xóa chi tiết đơn hàng: {ex.Message}", ex);
                }
            }
        }
    }
}
