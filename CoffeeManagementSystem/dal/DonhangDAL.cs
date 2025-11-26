using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms; // Only for MessageBox in error handling examples (will be replaced by throw in DAL)

// Ensure using namespace contains your BaseDataAccess class
// Ensure using namespace contains your Donhang Model class

namespace CoffeeManagementSystem.DAL
{
    public class DonhangDAL : BaseDataAccess
    {
        public DonhangDAL() : base() { }

        /// <summary>
        /// Adds a new order to the database.
        /// </summary>
        /// <param name="donhang">The Donhang object to add.</param>
        public void AddDonhang(Donhang donhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string insertSql = @"
                        INSERT INTO Donhang (Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien)
                        VALUES (@Madonhang, @Manhanvien, @Makhachhang, @Thoigiandat, @Trangthaidon, @Tongtien)";

                    using (SQLiteCommand command = new SQLiteCommand(insertSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", donhang.Madonhang);
                        command.Parameters.AddWithValue("@Manhanvien", donhang.Manhanvien);
                        command.Parameters.AddWithValue("@Makhachhang", (object)donhang.Makhachhang ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Thoigiandat", donhang.Thoigiandat.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Trangthaidon", donhang.Trangthaidon);
                        command.Parameters.AddWithValue("@Tongtien", donhang.Tongtien);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi thêm đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Thêm một đơn hàng mới vào cơ sở dữ liệu trong một transaction.
        /// Phương thức này được sử dụng bởi BLL để đảm bảo tính toàn vẹn dữ liệu.
        /// </summary>
        /// <param name="donhang">Đối tượng Donhang cần thêm.</param>
        /// <param name="connection">Kết nối SQLite hiện có (để sử dụng transaction).</param>
        /// <param name="transaction">Transaction SQLite hiện có.</param>
        public void AddDonhang(Donhang donhang, SQLiteConnection connection, SQLiteTransaction transaction)
        {
            try
            {
                string insertSql = @"
                    INSERT INTO Donhang (Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien)
                    VALUES (@Madonhang, @Manhanvien, @Makhachhang, @Thoigiandat, @Trangthaidon, @Tongtien)";

                using (SQLiteCommand command = new SQLiteCommand(insertSql, connection, transaction))
                {
                    command.Parameters.AddWithValue("@Madonhang", donhang.Madonhang);
                    command.Parameters.AddWithValue("@Manhanvien", donhang.Manhanvien);
                    command.Parameters.AddWithValue("@Makhachhang", (object)donhang.Makhachhang ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Thoigiandat", donhang.Thoigiandat.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@Trangthaidon", donhang.Trangthaidon);
                    command.Parameters.AddWithValue("@Tongtien", donhang.Tongtien);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                // Ném lỗi để BLL xử lý rollback, không hiển thị MessageBox ở đây.
                throw new Exception($"Lỗi DAL khi thêm đơn hàng trong transaction: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves all orders from the database.
        /// </summary>
        /// <returns>A list of Donhang objects.</returns>
        public List<Donhang> GetAllDonhangs()
        {
            List<Donhang> donhangs = new List<Donhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien FROM Donhang";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Donhang donhang = new Donhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Manhanvien = reader["Manhanvien"].ToString(),
                                    Makhachhang = reader["Makhachhang"] != DBNull.Value ? reader["Makhachhang"].ToString() : null,
                                    Thoigiandat = DateTime.Parse(reader["Thoigiandat"].ToString()),
                                    Trangthaidon = reader["Trangthaidon"].ToString(),
                                    Tongtien = Convert.ToDecimal(reader["Tongtien"])
                                };
                                donhangs.Add(donhang);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi lấy danh sách đơn hàng: {ex.Message}", ex);
                }
            }
            return donhangs;
        }

        /// <summary>
        /// Retrieves order information by ID.
        /// </summary>
        /// <param name="madonhang">The ID of the order to retrieve.</param>
        /// <returns>A Donhang object if found, otherwise null.</returns>
        public Donhang GetDonhangById(string madonhang)
        {
            Donhang donhang = null;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien FROM Donhang WHERE Madonhang = @Madonhang";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", madonhang);
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                donhang = new Donhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Manhanvien = reader["Manhanvien"].ToString(),
                                    Makhachhang = reader["Makhachhang"] != DBNull.Value ? reader["Makhachhang"].ToString() : null,
                                    Thoigiandat = DateTime.Parse(reader["Thoigiandat"].ToString()),
                                    Trangthaidon = reader["Trangthaidon"].ToString(),
                                    Tongtien = Convert.ToDecimal(reader["Tongtien"])
                                };
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi lấy đơn hàng theo ID: {ex.Message}", ex);
                }
            }
            return donhang;
        }

        /// <summary>
        /// Updates the information of an order.
        /// </summary>
        /// <param name="donhang">The Donhang object containing updated information (Madonhang is required).</param>
        public void UpdateDonhang(Donhang donhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string updateSql = @"
                        UPDATE Donhang
                        SET Manhanvien = @Manhanvien,
                            Makhachhang = @Makhachhang,
                            Thoigiandat = @Thoigiandat,
                            Trangthaidon = @Trangthaidon,
                            Tongtien = @Tongtien
                        WHERE Madonhang = @Madonhang";

                    using (SQLiteCommand command = new SQLiteCommand(updateSql, connection))
                    {
                        command.Parameters.AddWithValue("@Manhanvien", donhang.Manhanvien);
                        command.Parameters.AddWithValue("@Makhachhang", (object)donhang.Makhachhang ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Thoigiandat", donhang.Thoigiandat.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Trangthaidon", donhang.Trangthaidon);
                        command.Parameters.AddWithValue("@Tongtien", donhang.Tongtien);
                        command.Parameters.AddWithValue("@Madonhang", donhang.Madonhang);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi cập nhật đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Deletes an order from the database.
        /// </summary>
        /// <param name="madonhang">The ID of the order to delete.</param>
        public void DeleteDonhang(string madonhang)
        {
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string deleteSql = "DELETE FROM Donhang WHERE Madonhang = @Madonhang";
                    using (SQLiteCommand command = new SQLiteCommand(deleteSql, connection))
                    {
                        command.Parameters.AddWithValue("@Madonhang", madonhang);
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi xóa đơn hàng: {ex.Message}", ex);
                }
            }
        }
        /// <summary>
        /// Hàm đếm tổng số đơn cho label
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public int GetOrderCount(DateTime start, DateTime end)
        {
            string query = @"
        SELECT COUNT(*) 
        FROM Donhang
        WHERE Thoigiandat >= @fromDate
          AND Thoigiandat <  @toDatePlus1";

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@fromDate", start.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", end.Date.AddDays(1)); // < end+1 ngày

                conn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Searches for orders based on a keyword in Madonhang, Manhanvien, Makhachhang, Trangthaidon columns.
        /// </summary>
        /// <param name="searchTerm">The search keyword.</param>
        /// <returns>A list of matching Donhang objects.</returns>
        public List<Donhang> SearchDonhangs(string searchTerm)
        {
            List<Donhang> donhangs = new List<Donhang>();
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return donhangs;
            }

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = @"
                        SELECT Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien
                        FROM Donhang
                        WHERE LOWER(Madonhang) LIKE @SearchTerm
                           OR LOWER(Manhanvien) LIKE @SearchTerm
                           OR LOWER(Makhachhang) LIKE @SearchTerm
                           OR LOWER(Trangthaidon) LIKE @SearchTerm";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm.ToLower() + "%");
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Donhang donhang = new Donhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Manhanvien = reader["Manhanvien"].ToString(),
                                    Makhachhang = reader["Makhachhang"] != DBNull.Value ? reader["Makhachhang"].ToString() : null,
                                    Thoigiandat = DateTime.Parse(reader["Thoigiandat"].ToString()),
                                    Trangthaidon = reader["Trangthaidon"].ToString(),
                                    Tongtien = Convert.ToDecimal(reader["Tongtien"])
                                };
                                donhangs.Add(donhang);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // ĐIỀU CHỈNH: Thay thế MessageBox.Show bằng cách ném lỗi để BLL xử lý
                    throw new Exception($"Lỗi DAL khi tìm kiếm đơn hàng: {ex.Message}", ex);
                }
            }
            return donhangs;
        }
        public List<RevenueReportItem> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            List<RevenueReportItem> revenueData = new List<RevenueReportItem>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT
                            STRFTIME('%Y-%m-%d', Thoigiandat) AS Ngay,
                            SUM(Tongtien) AS Tongtien
                        FROM Donhang
                        WHERE DATE(Thoigiandat) >= DATE(@StartDate) AND DATE(Thoigiandat) <= DATE(@EndDate)
                        GROUP BY Ngay
                        ORDER BY Ngay;";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                revenueData.Add(new RevenueReportItem
                                {
                                    Ngay = DateTime.Parse(reader["Ngay"].ToString()),
                                    Tongtien = Convert.ToDecimal(reader["Tongtien"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lấy báo cáo doanh thu: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return revenueData;
        }
        public DataTable GetDoanhThuTheoLoaiTrongNgay(DateTime day)
        {
            string sql = @"
                SELECT l.Tenloai, SUM(ct.Soluong * ct.Dongia) AS DoanhThu
                FROM Loaidouong l
                JOIN Douong d       ON d.Maloai     = l.Maloai
                JOIN Chitietdonhang ct ON ct.Madouong  = d.Madouong
                JOIN Donhang dh     ON dh.Madonhang = ct.Madonhang
                WHERE dh.Thoigiandat >= @fromDate
                  AND dh.Thoigiandat <  @toDatePlus1
                GROUP BY l.Tenloai;";

            DataTable dt = new DataTable();

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            using (var da = new SQLiteDataAdapter(cmd))
            {
                cmd.Parameters.AddWithValue("@fromDate", day.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", day.Date.AddDays(1));

                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }
        /// <summary>
        /// Hàm lấy doanh thu theo loại đồ uống cho pie chart
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public DataTable GetDoanhThuTheoLoai(DateTime start, DateTime end)
        {
            string query = @"
        SELECT l.Tenloai, SUM(ct.Soluong * ct.Dongia) AS Doanhthu
        FROM Loaidouong l
        JOIN Douong d ON d.Maloai = l.Maloai
        JOIN Chitietdonhang ct ON ct.Madouong = d.Madouong
        JOIN Donhang dh ON dh.Madonhang = ct.Madonhang
        WHERE dh.Thoigiandat >= @fromDate AND dh.Thoigiandat < @toDatePlus1
        GROUP BY l.Tenloai";

            var dt = new DataTable();

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(query, conn))
            using (var da = new SQLiteDataAdapter(cmd))
            {
                // dùng BETWEEN dạng [start, end+1) để không bị mất phần giờ
                cmd.Parameters.AddWithValue("@fromDate", start.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", end.Date.AddDays(1));

                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }
        /// <summary>
        /// Hàm lấy doanh thu theo giờ dựa trên đơn hàng
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public DataTable GetRevenueByHour(DateTime date)
        {
            string query = @"
        SELECT 
            strftime('%H', Thoigiandat) AS Gio,
            SUM(TongTien) AS DoanhThu
        FROM Donhang
        WHERE DATE(Thoigiandat) = DATE(@date)
        GROUP BY strftime('%H', Thoigiandat)
        ORDER BY Gio;
    ";

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@date", date);

                using (var da = new SQLiteDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }


    }

    public class RevenueReportItem
    {
        public DateTime Ngay { get; set; }
        public decimal Tongtien { get; set; }
    }
    //thêm cho report

}