using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    public class KhuyenmaiDAL : BaseDataAccess
    {
        // Nếu muốn giống KhachhangDAL thì có thể thêm:
        // private readonly string _connectionString = @"DataSource=QuanLyCaPheDatabase.db;Version=3;";

        public KhuyenmaiDAL() : base() { }

        public List<Khuyenmai> GetAllKhuyenmai()
        {
            List<Khuyenmai> list = new List<Khuyenmai>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT MaKM, TenChuongTrinh, NgayBatDau, NgayKetThuc, PhanTramGiam, TrangThai, GhiChu FROM Khuyenmai";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new Khuyenmai
                            {
                                MaKM = reader["MaKM"].ToString(),
                                TenChuongTrinh = reader["TenChuongTrinh"].ToString(),
                                NgayBatDau = DateTime.Parse(reader["NgayBatDau"].ToString()),
                                NgayKetThuc = DateTime.Parse(reader["NgayKetThuc"].ToString()),
                                PhanTramGiam = Convert.ToInt32(reader["PhanTramGiam"]),
                                TrangThai = Convert.ToInt32(reader["TrangThai"]),
                                GhiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : null
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Lỗi khi lấy danh sách khuyến mãi", ex);
                    // có thể MessageBox.Show nếu bạn thích
                    throw;
                }
            }

            return list;
        }

        public void InsertKhuyenmai(Khuyenmai km)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO Khuyenmai
                        (MaKM, TenChuongTrinh, NgayBatDau, NgayKetThuc, PhanTramGiam, TrangThai, GhiChu)
                        VALUES (@MaKM, @TenChuongTrinh, @NgayBatDau, @NgayKetThuc, @PhanTramGiam, @TrangThai, @GhiChu);";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaKM", km.MaKM);
                        cmd.Parameters.AddWithValue("@TenChuongTrinh", km.TenChuongTrinh);
                        cmd.Parameters.AddWithValue("@NgayBatDau", km.NgayBatDau.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@NgayKetThuc", km.NgayKetThuc.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@PhanTramGiam", km.PhanTramGiam);
                        cmd.Parameters.AddWithValue("@TrangThai", km.TrangThai);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)km.GhiChu ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Lỗi khi thêm khuyến mãi", ex);
                    throw;
                }
            }
        }

        public void UpdateKhuyenmai(Khuyenmai km)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        UPDATE Khuyenmai
                        SET TenChuongTrinh = @TenChuongTrinh,
                            NgayBatDau = @NgayBatDau,
                            NgayKetThuc = @NgayKetThuc,
                            PhanTramGiam = @PhanTramGiam,
                            TrangThai = @TrangThai,
                            GhiChu = @GhiChu
                        WHERE MaKM = @MaKM;";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaKM", km.MaKM);
                        cmd.Parameters.AddWithValue("@TenChuongTrinh", km.TenChuongTrinh);
                        cmd.Parameters.AddWithValue("@NgayBatDau", km.NgayBatDau.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@NgayKetThuc", km.NgayKetThuc.ToString("yyyy-MM-dd HH:mm:ss"));
                        cmd.Parameters.AddWithValue("@PhanTramGiam", km.PhanTramGiam);
                        cmd.Parameters.AddWithValue("@TrangThai", km.TrangThai);
                        cmd.Parameters.AddWithValue("@GhiChu", (object)km.GhiChu ?? DBNull.Value);

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Lỗi khi cập nhật khuyến mãi", ex);
                    throw;
                }
            }
        }

        public void DeleteKhuyenmai(string maKM)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM Khuyenmai WHERE MaKM = @MaKM";

                    using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@MaKM", maKM);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogError("Lỗi khi xóa khuyến mãi", ex);
                    throw;
                }
            }
        }
        public Khuyenmai GetKhuyenmaiById(string maKM)
        {
            Khuyenmai km = null;

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT MaKM, TenChuongTrinh, NgayBatDau, NgayKetThuc, PhanTramGiam, TrangThai, GhiChu FROM Khuyenmai WHERE MaKM = @MaKM";

                using (SQLiteCommand cmd = new SQLiteCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@MaKM", maKM);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            km = new Khuyenmai
                            {
                                MaKM = reader["MaKM"].ToString(),
                                TenChuongTrinh = reader["TenChuongTrinh"].ToString(),
                                NgayBatDau = DateTime.Parse(reader["NgayBatDau"].ToString()),
                                NgayKetThuc = DateTime.Parse(reader["NgayKetThuc"].ToString()),
                                PhanTramGiam = Convert.ToInt32(reader["PhanTramGiam"]),
                                TrangThai = Convert.ToInt32(reader["TrangThai"]),
                                GhiChu = reader["GhiChu"] != DBNull.Value ? reader["GhiChu"].ToString() : null
                            };
                        }
                    }
                }
            }

            return km;
        }
    }
}
