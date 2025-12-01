using System;
using System.Collections.Generic;
using System.Data.SQLite;
// KHÔNG dùng System.Windows.Forms trong DAL vì DAL không nên hiển thị MessageBox

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// Lớp truy xuất dữ liệu cho bảng Nhanvien.
    /// Kế thừa BaseDataAccess để dùng chung ConnectionString.
    /// </summary>
    public class NhanvienDAL : BaseDataAccess
    {
        // Chuỗi kết nối riêng (đang song song với ConnectionString của BaseDataAccess)
        private readonly string _connectionString = @"DataSource=QuanLyCaPheDatabase.db;Version=3;";

        public NhanvienDAL() : base()
        {
        }

        // =====================================================
        // LẤY DANH SÁCH NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Lấy toàn bộ nhân viên từ CSDL (không lọc).
        /// </summary>
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
                // DAL không show MessageBox, ném lỗi cho BLL xử lý
                throw new Exception("Lỗi DAL khi lấy danh sách nhân viên: " + ex.Message, ex);
            }
            return nhanviens;
        }

        // =====================================================
        // LẤY NHÂN VIÊN THEO MÃ
        // =====================================================

        /// <summary>
        /// Lấy thông tin một nhân viên theo mã (Manhanvien).
        /// </summary>
        public Nhanvien GetNhanvienById(string manhanvien)
        {
            Nhanvien nhanvien = null;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam 
                        FROM Nhanvien 
                        WHERE Manhanvien = @Manhanvien";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // Tránh SQL Injection bằng parameter
                        command.Parameters.AddWithValue("@Manhanvien", manhanvien);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                nhanvien = MapDataReaderToNhanvien(reader);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi DAL khi lấy nhân viên theo ID: " + ex.Message, ex);
                }
            }

            return nhanvien; // Có thể null nếu không tìm thấy
        }

        // =====================================================
        // THÊM NHÂN VIÊN MỚI
        // =====================================================

        /// <summary>
        /// Thêm một nhân viên mới vào CSDL.
        /// Trả về true nếu thêm thành công.
        /// </summary>
        public bool AddNhanvien(Nhanvien nhanvien)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Nhanvien 
                            (Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam)
                        VALUES 
                            (@Manhanvien, @Hoten, @Ngaysinh, @Gioitinh, @Diachi, @Sodienthoai, @Email, @Ngayvaolam)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        // Dùng hàm chung để add parameter
                        AddNhanvienParameters(command, nhanvien);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi DAL khi thêm nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // CẬP NHẬT NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Cập nhật thông tin một nhân viên (dựa trên Manhanvien).
        /// Trả về true nếu cập nhật thành công.
        /// </summary>
        public bool UpdateNhanvien(Nhanvien nhanvien)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Nhanvien
                        SET Hoten       = @Hoten,
                            Ngaysinh    = @Ngaysinh,
                            Gioitinh    = @Gioitinh,
                            Diachi      = @Diachi,
                            Sodienthoai = @Sodienthoai,
                            Email       = @Email,
                            Ngayvaolam  = @Ngayvaolam
                        WHERE Manhanvien = @Manhanvien";

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        // Dùng chung hàm set parameter
                        AddNhanvienParameters(command, nhanvien);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi DAL khi cập nhật nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // XÓA NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Xóa nhân viên theo mã.
        /// Trả về true nếu xóa thành công.
        /// </summary>
        public bool DeleteNhanvien(string manhanvien)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Nhanvien WHERE Manhanvien = @Manhanvien";

                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Manhanvien", manhanvien);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi DAL khi xóa nhân viên: " + ex.Message, ex);
                }
            }
        }

        // =====================================================
        // TÌM KIẾM NHÂN VIÊN
        // =====================================================

        /// <summary>
        /// Tìm kiếm nhân viên theo từ khóa (mã, họ tên, địa chỉ, SĐT, email).
        /// </summary>
        public List<Nhanvien> SearchNhanviens(string searchTerm)
        {
            List<Nhanvien> nhanviens = new List<Nhanvien>();

            // Nếu từ khóa rỗng → trả về list rỗng (BLL có thể quyết định gọi GetAll nếu cần)
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return nhanviens;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string selectSql = @"
                        SELECT Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam
                        FROM Nhanvien
                        WHERE LOWER(Manhanvien)  LIKE @SearchTerm
                           OR LOWER(Hoten)       LIKE @SearchTerm
                           OR LOWER(Diachi)      LIKE @SearchTerm
                           OR LOWER(Sodienthoai) LIKE @SearchTerm
                           OR LOWER(Email)       LIKE @SearchTerm";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // %term% để tìm bất kỳ chuỗi có chứa searchTerm
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
                    throw new Exception("Lỗi DAL khi tìm kiếm nhân viên: " + ex.Message, ex);
                }
            }

            return nhanviens;
        }

        // =====================================================
        // HÀM HỖ TRỢ: ÁNH XẠ DỮ LIỆU TỪ DATAREADER → MODEL NHANVIEN
        // =====================================================

        /// <summary>
        /// Chuyển 1 dòng dữ liệu từ SQLiteDataReader sang đối tượng Nhanvien.
        /// </summary>
        private Nhanvien MapDataReaderToNhanvien(SQLiteDataReader reader)
        {
            return new Nhanvien
            {
                Manhanvien = reader["Manhanvien"].ToString(),
                Hoten = reader["Hoten"].ToString(),
                Ngaysinh = DateTime.Parse(reader["Ngaysinh"].ToString()),
                Gioitinh = reader["Gioitinh"].ToString(),
                Diachi = reader["Diachi"].ToString(),
                Sodienthoai = reader["Sodienthoai"] != DBNull.Value
                              ? reader["Sodienthoai"].ToString()
                              : null,
                Email = reader["Email"] != DBNull.Value
                              ? reader["Email"].ToString()
                              : null,
                Ngayvaolam = DateTime.Parse(reader["Ngayvaolam"].ToString())
            };
        }

        // =====================================================
        // HÀM HỖ TRỢ: THÊM PARAMETER CHUNG CHO INSERT / UPDATE
        // =====================================================

        /// <summary>
        /// Thêm toàn bộ parameter tương ứng với các cột của Nhanvien vào SQLiteCommand.
        /// Dùng chung cho INSERT và UPDATE.
        /// </summary>
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

        // =====================================================
        // LẤY TOÀN BỘ MÃ NHÂN VIÊN (PHỤC VỤ SINH MÃ TỰ ĐỘNG / IMPORT)
        // =====================================================

        /// <summary>
        /// Lấy danh sách tất cả mã nhân viên (Manhanvien).
        /// </summary>
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

        // =====================================================
        // IMPORT DANH SÁCH NHÂN VIÊN (INSERT/UPDATE THEO MÃ)
        // =====================================================

        /// <summary>
        /// Import nhiều nhân viên:
        /// - Nếu mã đã tồn tại → UPDATE
        /// - Nếu mã chưa tồn tại → INSERT
        /// Thực thi trong transaction để đảm bảo tính toàn vẹn.
        /// </summary>
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
                            // 1. Kiểm tra nhân viên đã tồn tại hay chưa
                            string checkSql = "SELECT COUNT(1) FROM Nhanvien WHERE Manhanvien = @Manhanvien";
                            using (var cmdCheck = new SQLiteCommand(checkSql, connection, transaction))
                            {
                                cmdCheck.Parameters.AddWithValue("@Manhanvien", n.Manhanvien);
                                long count = (long)cmdCheck.ExecuteScalar();

                                if (count > 0)
                                {
                                    // 2. Update nếu đã tồn tại
                                    // LƯU Ý: câu SQL dưới đây hiện tại CHƯA CÓ WHERE → sẽ update toàn bảng
                                    // TODO: thêm 'WHERE Manhanvien = @Manhanvien' nếu muốn update đúng 1 nhân viên
                                    string updateSql = @"
                                        UPDATE Nhanvien 
                                        SET Hoten = @Hoten,
                                            Ngaysinh = @Ngaysinh,
                                            Gioitinh = @Gioitinh,
                                            Diachi = @Diachi,
                                            Sodienthoai = @Sodienthoai,
                                            Email = @Email,
                                            Ngayvaolam = @Ngayvaolam";

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
                                    // 3. Insert nếu chưa tồn tại
                                    string insertSql = @"
                                        INSERT INTO Nhanvien 
                                            (Manhanvien, Hoten, Ngaysinh, Gioitinh, Diachi, Sodienthoai, Email, Ngayvaolam) 
                                        VALUES 
                                            (@Manhanvien, @Hoten, @Ngaysinh, @Gioitinh, @Diachi, @Sodienthoai, @Email, @Ngayvaolam)";

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
