using CoffeeManagementSystem.CoffeeManagementSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms; // Chỉ dùng cho một số MessageBox trong báo cáo (có thể thay bằng throw sau này)

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// DonhangDAL:
    /// - Tầng truy cập dữ liệu (Data Access Layer) cho bảng Donhang.
    /// - Chỉ chịu trách nhiệm làm việc với CSDL (SELECT, INSERT, UPDATE, DELETE).
    /// - Được gọi từ BLL (PaymentBLL, các BLL khác) để thao tác với bảng Donhang.
    /// </summary>
    public class DonhangDAL : BaseDataAccess
    {
        public DonhangDAL() : base() { }

        /// <summary>
        /// Thêm 1 đơn hàng mới (không dùng transaction).
        /// - Ít dùng trong flow thanh toán chính, vì thanh toán đang dùng bản có transaction.
        /// - Có thể dùng ở những chỗ đơn giản khác nếu cần.
        /// </summary>
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
                        // Nếu Makhachhang null → lưu DBNull.Value (cho phép hóa đơn không có khách).
                        command.Parameters.AddWithValue("@Makhachhang", (object)donhang.Makhachhang ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Thoigiandat", donhang.Thoigiandat.ToString("yyyy-MM-dd HH:mm:ss"));
                        command.Parameters.AddWithValue("@Trangthaidon", donhang.Trangthaidon);
                        command.Parameters.AddWithValue("@Tongtien", donhang.Tongtien);

                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // DAL không show MessageBox nữa, ném lỗi cho BLL/UI xử lý.
                    throw new Exception($"Lỗi DAL khi thêm đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Thêm đơn hàng trong ngữ cảnh transaction có sẵn:
        /// - Được PaymentBLL gọi trong ProcessPayment(...)
        /// - Dùng chung connection + transaction với Chitietdonhang & Thanhtoan để đảm bảo COMMIT/ROLLBACK đồng bộ.
        /// </summary>
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
                // Ném lỗi về cho BLL để BLL quyết định rollback transaction.
                throw new Exception($"Lỗi DAL khi thêm đơn hàng trong transaction: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách đơn hàng từ CSDL.
        /// - Thường dùng cho màn hình quản lý đơn hàng / lịch sử đơn.
        /// </summary>
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
                    throw new Exception($"Lỗi DAL khi lấy danh sách đơn hàng: {ex.Message}", ex);
                }
            }
            return donhangs;
        }

        /// <summary>
        /// Lấy 1 đơn hàng theo mã Madonhang.
        /// - Trả về Donhang nếu tìm thấy, ngược lại trả về null.
        /// </summary>
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
                    throw new Exception($"Lỗi DAL khi lấy đơn hàng theo ID: {ex.Message}", ex);
                }
            }
            return donhang;
        }

        /// <summary>
        /// Cập nhật thông tin 1 đơn hàng theo Madonhang.
        /// - Yêu cầu Madonhang không được thay đổi (khóa để WHERE).
        /// </summary>
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
                    throw new Exception($"Lỗi DAL khi cập nhật đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Xóa 1 đơn hàng theo Madonhang.
        /// - Lưu ý: nếu có khóa ngoại (chi tiết đơn hàng), cần xóa trước hoặc dùng cascade.
        /// </summary>
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
                    throw new Exception($"Lỗi DAL khi xóa đơn hàng: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Đếm tổng số đơn hàng trong khoảng thời gian (start → end).
        /// - Dùng cho label tổng số đơn / thống kê chung.
        /// - Sử dụng [fromDate, toDate + 1) để không mất phần giờ.
        /// </summary>
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
        /// Tìm kiếm đơn hàng theo keyword:
        /// - So khớp LIKE trên các cột: Madonhang, Manhanvien, Makhachhang, Trangthaidon.
        /// - Dùng LOWER(...) để tìm kiếm không phân biệt hoa/thường.
        /// </summary>
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
                    throw new Exception($"Lỗi DAL khi tìm kiếm đơn hàng: {ex.Message}", ex);
                }
            }
            return donhangs;
        }

        /// <summary>
        /// Lấy dữ liệu doanh thu theo ngày trong khoảng (startDate → endDate).
        /// - Trả về list RevenueReportItem để vẽ chart cột / line.
        /// - Gom nhóm theo ngày (STRFTIME('%Y-%m-%d', Thoigiandat)).
        /// </summary>
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
                    // Ở đây vẫn dùng MessageBox, có thể chuyển sang throw nếu muốn thống nhất với các hàm khác
                    MessageBox.Show($"Lỗi khi lấy báo cáo doanh thu: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return revenueData;
        }

        /// <summary>
        /// Lấy doanh thu theo loại đồ uống trong 1 ngày cụ thể (cho chart cột/bảng).
        /// - JOIN Loaidouong, Douong, Chitietdonhang, Donhang.
        /// - Gom nhóm theo tên loại (Tenloai).
        /// </summary>
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
        /// Lấy doanh thu theo loại đồ uống trong khoảng thời gian (start → end)
        /// - Dùng cho pie chart: mỗi lát là 1 loại đồ uống.
        /// </summary>
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
                // [start, end+1) để không mất phần giờ
                cmd.Parameters.AddWithValue("@fromDate", start.Date);
                cmd.Parameters.AddWithValue("@toDatePlus1", end.Date.AddDays(1));

                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }

        /// <summary>
        /// Lấy doanh thu theo giờ trong 1 ngày (dựa trên Tongtien của Donhang).
        /// - Dùng cho chart cột theo giờ: 08h, 09h, 10h,...
        /// </summary>
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

        /// <summary>
        /// Lấy danh sách lịch sử đơn hàng trong khoảng thời gian:
        /// - JOIN với Khachhang & Thanhtoan để lấy tên khách + hình thức thanh toán.
        /// - Dùng cho màn hình “Lịch sử đơn hàng”.
        /// </summary>
        public List<OrderHistoryItem> GetOrderHistory(DateTime fromDate, DateTime toDate)
        {
            List<OrderHistoryItem> list = new List<OrderHistoryItem>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string sql = @"
                SELECT 
                    dh.Madonhang,
                    dh.Thoigiandat,
                    COALESCE(kh.Hoten, 'Khách lẻ') AS TenKhachhang,
                    dh.Tongtien,
                    dh.Trangthaidon,
                    COALESCE(tt.Hinhthucthanhtoan, '') AS HinhThucThanhToan
                FROM Donhang dh
                LEFT JOIN Khachhang kh ON kh.Makhachhang = dh.Makhachhang
                LEFT JOIN Thanhtoan tt ON tt.Madonhang   = dh.Madonhang
                WHERE dh.Thoigiandat >= @fromDate 
                  AND dh.Thoigiandat <  @toDatePlus1
                ORDER BY dh.Thoigiandat DESC;";

                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@fromDate", fromDate.Date);
                        command.Parameters.AddWithValue("@toDatePlus1", toDate.Date.AddDays(1));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var item = new OrderHistoryItem
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Thoigiandat = DateTime.Parse(reader["Thoigiandat"].ToString()),
                                    TenKhachhang = reader["TenKhachhang"].ToString(),
                                    Tongtien = reader["Tongtien"] != DBNull.Value
                                                         ? Convert.ToDecimal(reader["Tongtien"]) : 0m,
                                    Trangthaidon = reader["Trangthaidon"].ToString(),
                                    HinhThucThanhToan = reader["HinhThucThanhToan"].ToString()
                                };

                                list.Add(item);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy lịch sử đơn hàng: {ex.Message}", ex);
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy chi tiết từng món của một đơn hàng (theo Madonhang)
        /// - JOIN Chitietdonhang với Douong để lấy tên đồ uống.
        /// - Dùng cho màn chi tiết đơn / popup xem chi tiết.
        /// </summary>
        public List<OrderDetailLine> GetOrderDetail(string madonhang)
        {
            var list = new List<OrderDetailLine>();

            string sql = @"
                SELECT 
                    ct.Madouong,
                    d.Tendouong,
                    ct.Soluong,
                    ct.Dongia,
                    ct.Thanhtien
                FROM Chitietdonhang ct
                JOIN Douong d ON d.Madouong = ct.Madouong
                WHERE ct.Madonhang = @Madonhang";

            using (var conn = new SQLiteConnection(ConnectionString))
            using (var cmd = new SQLiteCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@Madonhang", madonhang);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var line = new OrderDetailLine
                        {
                            Madouong = reader["Madouong"].ToString(),
                            Tendouong = reader["Tendouong"].ToString(),
                            Soluong = Convert.ToInt32(reader["Soluong"]),
                            Dongia = Convert.ToDecimal(reader["Dongia"]),
                            Thanhtien = Convert.ToDecimal(reader["Thanhtien"])
                        };
                        list.Add(line);
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Lấy Madonhang cuối cùng dạng DHxxx (DH001, DH002, ...)
        /// - Dùng để sinh mã đơn mới: đọc mã lớn nhất rồi +1.
        /// </summary>
        public string GetLastMadonhang()
        {
            using (var conn = new SQLiteConnection(ConnectionString))
            {
                conn.Open();

                // Giả sử Madonhang: DH001, DH002, ...
                string sql = @"
            SELECT Madonhang
            FROM Donhang
            WHERE Madonhang LIKE 'DH%'
            ORDER BY CAST(SUBSTR(Madonhang, 3) AS INTEGER) DESC
            LIMIT 1;";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    object result = cmd.ExecuteScalar();
                    return (result == null || result == DBNull.Value)
                        ? null
                        : result.ToString();
                }
            }
        }

    }

    /// <summary>
    /// Model dùng cho báo cáo doanh thu theo ngày (GetRevenueByDateRange).
    /// </summary>
    public class RevenueReportItem
    {
        public DateTime Ngay { get; set; }
        public decimal Tongtien { get; set; }
    }

}
