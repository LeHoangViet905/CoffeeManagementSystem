using System;
using System.Collections.Generic;
using System.Data.SQLite;
// BỎ using System.Windows.Forms; ở DAL vì DAL không nên hiển thị MessageBox

// Đảm bảo using namespace chứa lớp BaseDataAccess của bạn
// Ví dụ: using CoffeeManagementSystem.DAL;
// Giả định BaseDataAccess nằm trong cùng namespace hoặc đã được using ở nơi khác.

// Đảm bảo using namespace chứa lớp Model Nhanvien của bạn

namespace CoffeeManagementSystem.DAL // Đặt DAL trong một namespace con để tổ chức code tốt hơn
{
    public class NhanvienDAL : BaseDataAccess // Kế thừa từ lớp BaseDataAccess
    {
        private readonly string _connectionString = @"DataSource=QuanLyCaPheDatabase.db;Version=3;";
        public NhanvienDAL() : base() // Gọi constructor của lớp base để lấy ConnectionString
        {
        }

        // =====================================================
        // PHƯƠNG THỨC LẤY DANH SÁCH NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Lấy tất cả nhân viên từ CSDL.
        /// </summary>
        /// <returns>Danh sách các đối tượng Nhanvien.</returns>
        public List<Nhanvien> GetAllNhanviens()
        {
            List<Nhanvien> nhanviens = new List<Nhanvien>();
            string query = "SELECT Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam FROM Nhanvien";

            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
                {
                    connection.Open();
                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        using (SQLiteDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nhanviens.Add(MapDataReaderToNhanvien(reader));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                throw new Exception("Lỗi DAL khi lấy danh sách nhân viên: " + ex.Message, ex);
            }
            return nhanviens; // Trả về danh sách nhân viên
        }

        // =====================================================
        // PHƯƠNG THỨC LẤY NHÂN VIÊN THEO ID
        // =====================================================

        /// <summary>
        /// Lấy thông tin nhân viên theo Mã nhân viên.
        /// </summary>
        /// <param name="manhanvien">Mã nhân viên cần lấy.</param>
        /// <returns>Đối tượng Nhanvien nếu tìm thấy, ngược lại trả về null.</returns>
        public Nhanvien GetNhanvienById(string manhanvien)
        {
            Nhanvien nhanvien = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam FROM Nhanvien WHERE Manhanvien = @Manhanvien";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // Sử dụng Parameter cho điều kiện WHERE
                        command.Parameters.AddWithValue("@Manhanvien", manhanvien);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read()) // Đọc hàng đầu tiên (và duy nhất) nếu có
                            {
                                nhanvien = MapDataReaderToNhanvien(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                    throw new Exception("Lỗi DAL khi lấy nhân viên theo ID: " + ex.Message, ex);
                }
            }
            return nhanvien; // Trả về object Nhanvien hoặc null
        }

        // =====================================================
        // PHƯƠNG THỨC THÊM NHÂN VIÊN MỚI
        // =====================================================

        /// <summary>
        /// Thêm một nhân viên mới vào CSDL.
        /// </summary>
        /// <param name="nhanvien">Đối tượng Nhanvien cần thêm.</param>
        /// <returns>True nếu thêm thành công, ngược lại False.</returns>
        public bool AddNhanvien(Nhanvien nhanvien) // THAY ĐỔI TỪ VOID SANG BOOL
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Nhanvien (Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam)
                        VALUES (@Manhanvien, @Hoten, @Ngaysinh, @Gioitinh, @Diachi, @Sodienthoai, @Email, @Ngayvaolam)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        // Sử dụng Parameters để chèn dữ liệu
                        AddNhanvienParameters(command, nhanvien); // Dùng phương thức hỗ trợ chung

                        int rowsAffected = command.ExecuteNonQuery(); // Thực thi lệnh INSERT
                        return rowsAffected > 0; // Trả về true nếu có ít nhất 1 hàng bị ảnh hưởng
                    }
                }
                catch (Exception ex)
                {
                    // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                    throw new Exception("Lỗi DAL khi thêm nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // PHƯƠNG THỨC CẬP NHẬT THÔNG TIN NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Cập nhật thông tin của một nhân viên.
        /// </summary>
        /// <param name="nhanvien">Đối tượng Nhanvien chứa thông tin cập nhật (cần có Manhanvien).</param>
        /// <returns>True nếu cập nhật thành công, ngược lại False.</returns>
        public bool UpdateNhanvien(Nhanvien nhanvien) // THAY ĐỔI TỪ VOID SANG BOOL
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Nhanvien
                        SET Hoten = @Hoten,
                            Ngaysinh = @Ngaysinh,
                            Gioitinh = @Gioitinh,
                            Diachi = @Diachi,
                            Sodienthoai = @Sodienthoai,
                            Email = @Email,
                            Ngayvaolam = @Ngayvaolam
                        WHERE Manhanvien = @Manhanvien"; // Cập nhật dựa trên khóa chính

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        // Sử dụng Parameters để cập nhật dữ liệu
                        AddNhanvienParameters(command, nhanvien); // Dùng phương thức hỗ trợ chung

                        int rowsAffected = command.ExecuteNonQuery(); // Thực thi lệnh UPDATE
                        return rowsAffected > 0; // Trả về true nếu có ít nhất 1 hàng bị ảnh hưởng
                    }
                }
                catch (Exception ex)
                {
                    // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                    throw new Exception("Lỗi DAL khi cập nhật nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // PHƯƠNG THỨC XÓA NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Xóa một nhân viên khỏi CSDL.
        /// </summary>
        /// <param name="manhanvien">Mã nhân viên cần xóa.</param>
        /// <returns>True nếu xóa thành công, ngược lại False.</returns>
        public bool DeleteNhanvien(string manhanvien) // THAY ĐỔI TỪ VOID SANG BOOL
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Nhanvien WHERE Manhanvien = @Manhanvien";

                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        // Sử dụng Parameter cho điều kiện WHERE
                        command.Parameters.AddWithValue("@Manhanvien", manhanvien);

                        int rowsAffected = command.ExecuteNonQuery(); // Thực thi lệnh DELETE
                        return rowsAffected > 0; // Trả về true nếu có ít nhất 1 hàng bị ảnh hưởng
                    }
                }
                catch (Exception ex)
                {
                    // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                    throw new Exception("Lỗi DAL khi xóa nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // PHƯƠNG THỨC TÌM KIẾM NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Tìm kiếm nhân viên dựa trên từ khóa trong các cột Manhanvien, Hoten, Sodienthoai, Email, Diachi.
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách các đối tượng Nhanvien phù hợp.</returns>
        public List<Nhanvien> SearchNhanviens(string searchTerm)
        {
            List<Nhanvien> nhanviens = new List<Nhanvien>();

            // Kiểm tra nếu từ khóa rỗng hoặc null, trả về danh sách rỗng
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return nhanviens;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Câu lệnh SELECT với điều kiện WHERE sử dụng LIKE để tìm kiếm theo mẫu
                    // Tìm kiếm trong Manhanvien, Hoten, Sodienthoai, Email, Diachi
                    // Sử dụng LOWER() để tìm kiếm không phân biệt chữ hoa/thường
                    string selectSql = @"
                        SELECT Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam
                        FROM Nhanvien
                        WHERE LOWER(Manhanvien) LIKE @SearchTerm
                            OR LOWER(Hoten) LIKE @SearchTerm
                            OR LOWER(Diachi) LIKE @SearchTerm
                            OR LOWER(Sodienthoai) LIKE @SearchTerm
                            OR LOWER(Email) LIKE @SearchTerm";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // Sử dụng Parameter với ký tự wildcard %
                        // % ở đầu và cuối cho phép tìm kiếm bất kỳ chuỗi nào chứa searchTerm
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                nhanviens.Add(MapDataReaderToNhanvien(reader));
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Thay vì MessageBox, ném lại ngoại lệ để BLL xử lý
                    throw new Exception("Lỗi DAL khi tìm kiếm nhân viên: " + ex.Message, ex);
                }
            }
            return nhanviens; // Trả về danh sách nhân viên phù hợp
        }

        // =====================================================
        // PHƯƠNG THỨC HỖ TRỢ: ÁNH XẠ DATAREADER SANG NHANVIEN OBJECT
        // =====================================================
        private Nhanvien MapDataReaderToNhanvien(SQLiteDataReader reader)
        {
            return new Nhanvien
            {
                Manhanvien = reader["Manhanvien"].ToString(),
                Hoten = reader["Hoten"].ToString(),
                Ngaysinh = DateTime.Parse(reader["Ngaysinh"].ToString()),
                Gioitinh = reader["Gioitinh"].ToString(),
                Diachi = reader["Diachi"].ToString(),
                Sodienthoai = reader["Sodienthoai"] != DBNull.Value ? reader["Sodienthoai"].ToString() : null,
                Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : null,
                Ngayvaolam = DateTime.Parse(reader["Ngayvaolam"].ToString())
            };
        }

        // =====================================================
        // PHƯƠNG THỨC HỖ TRỢ: THÊM THAM SỐ CHO LỆNH COMMAND
        // =====================================================
        // Sử dụng chung cho Insert và Update
        private void AddNhanvienParameters(SQLiteCommand cmd, Nhanvien nhanvien)
        {
            cmd.Parameters.AddWithValue("@Manhanvien", nhanvien.Manhanvien);
            cmd.Parameters.AddWithValue("@Hoten", nhanvien.Hoten);
            cmd.Parameters.AddWithValue("@Ngaysinh", nhanvien.Ngaysinh.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@Gioitinh", nhanvien.Gioitinh);
            cmd.Parameters.AddWithValue("@Diachi", nhanvien.Diachi);
            cmd.Parameters.AddWithValue("@Sodienthoai", (object)nhanvien.Sodienthoai ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", (object)nhanvien.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Ngayvaolam", nhanvien.Ngayvaolam.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        public List<string> GetAllMaNV()
        {
            List<string> maList = new List<string>();
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                var cmd = new SQLiteCommand("SELECT Manhanvien FROM Nhanvien", conn);
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maList.Add(reader["Manhanvien"].ToString());
                    }
                }
            }
            return maList;
        }
        public void ImportNhanviens(List<Nhanvien> nhanviens)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        foreach (var n in nhanviens)
                        {
                            // Kiểm tra xem đã tồn tại chưa
                            string checkSql = "SELECT COUNT(1) FROM Nhanvien WHERE Manhanvien = @Manhanvien";
                            using (var cmdCheck = new SQLiteCommand(checkSql, connection, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Manhanvien", n.Manhanvien);
                                long count = (long)cmdCheck.ExecuteScalar();

                                if (count > 0)
                                {
                                    // Update nếu tồn tại
                                    string updateSql = "UPDATE Nhanvien SET Hoten = @Hoten,Ngaysinh = @Ngaysinh,Gioitinh = @Gioitinh,Diachi = @Diachi,Sodienthoai = @Sodienthoai,Email = @Email,Ngayvaolam = @Ngayvaolam";
                                    using (var cmdUpdate = new SQLiteCommand(updateSql, connection, transaction))
                                    {
                                        cmdUpdate.Parameters.AddWithValue("@Manhanvien", n.Manhanvien);
                                        cmdUpdate.Parameters.AddWithValue("@Hoten", n.Hoten);
                                        cmdUpdate.Parameters.AddWithValue("@Ngaysinh", n.Ngaysinh);
                                        cmdUpdate.Parameters.AddWithValue("@Gioitinh", n.Gioitinh);
                                        cmdUpdate.Parameters.AddWithValue("@Diachi", n.Diachi);
                                        cmdUpdate.Parameters.AddWithValue("@Sodienthoai", n.Sodienthoai);
                                        cmdUpdate.Parameters.AddWithValue("@Email", n.Email);
                                        cmdUpdate.Parameters.AddWithValue("@Ngayvaolam", n.Ngayvaolam);
                                        cmdUpdate.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    // Insert nếu chưa tồn tại
                                    string insertSql = "INSERT INTO Douong (Madouong, Tendouong, Maloai, Mota, Hinhanh) VALUES (@Madouong, @Tendouong, @Maloai, @Mota, @Hinhanh)";
                                    using (var cmdInsert = new SQLiteCommand(insertSql, connection, transaction))
                                    {
                                        cmdInsert.Parameters.AddWithValue("@Manhanvien", n.Manhanvien);
                                        cmdInsert.Parameters.AddWithValue("@Hoten", n.Hoten);
                                        cmdInsert.Parameters.AddWithValue("@Ngaysinh", n.Ngaysinh);
                                        cmdInsert.Parameters.AddWithValue("@Gioitinh", n.Gioitinh);
                                        cmdInsert.Parameters.AddWithValue("@Diachi", n.Diachi);
                                        cmdInsert.Parameters.AddWithValue("@Sodienthoai", n.Sodienthoai);
                                        cmdInsert.Parameters.AddWithValue("@Email", n.Email);
                                        cmdInsert.Parameters.AddWithValue("@Ngayvaolam", n.Ngayvaolam);
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
                        throw new Exception("Lỗi DAL khi import nhiều nhân viên: " + ex.Message, ex);
                    }
                }
            }
        }
    }
}