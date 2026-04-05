using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    /// <summary>
    /// ReportDAL:
    /// - Tầng truy xuất dữ liệu (Data Access Layer) chuyên cho các báo cáo.
    /// - Chỉ làm việc với CSDL (SQLite) và trả về:
    ///   + Tổng doanh thu trong khoảng ngày.
    ///   + Danh sách đơn hàng trong khoảng ngày.
    /// - KHÔNG xử lý logic business, không MessageBox – chỉ ném Exception cho BLL xử lý.
    /// </summary>
    public class ReportDAL : BaseDataAccess
    {
        /// <summary>
        /// Constructor:
        /// - Gọi base() để nhận ConnectionString từ BaseDataAccess.
        /// </summary>
        public ReportDAL() : base() { }

        /// <summary>
        /// Lấy tổng doanh thu (SUM Tongtien) trong một khoảng thời gian.
        /// - Dùng bảng Donhang.
        /// - Thời gian được giới hạn từ đầu ngày (00:00:00) tới cuối ngày (23:59:59).
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu (chỉ lấy phần ngày, giờ sẽ set về 00:00:00).</param>
        /// <param name="endDate">Ngày kết thúc (chỉ lấy phần ngày, giờ sẽ set lên 23:59:59).</param>
        /// <returns>
        /// Tổng doanh thu dạng decimal.
        /// Nếu không có đơn nào trong khoảng, trả về 0.
        /// </returns>
        public decimal GetTotalRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            decimal totalRevenue = 0m;

            // Tạo kết nối SQLite dùng ConnectionString kế thừa từ BaseDataAccess
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    // Câu lệnh SQL tính tổng cột Tongtien trong khoảng thời gian
                    string selectSql = @"
                        SELECT SUM(Tongtien) 
                        FROM Donhang 
                        WHERE Thoigiandat BETWEEN @StartDate AND @EndDate";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        // Gán tham số thời gian với chuỗi định dạng yyyy-MM-dd HH:mm:ss
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd 00:00:00"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd 23:59:59"));

                        // ExecuteScalar -> trả về 1 giá trị (object)
                        object result = command.ExecuteScalar();

                        // Nếu không null và không phải DBNull -> convert sang decimal
                        if (result != DBNull.Value && result != null)
                        {
                            totalRevenue = Convert.ToDecimal(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ném lỗi ra cho BLL xử lý, không MessageBox trong DAL
                    throw new Exception($"Lỗi DAL khi lấy tổng doanh thu: {ex.Message}", ex);
                }
            }

            return totalRevenue;
        }

        /// <summary>
        /// Lấy danh sách các đơn hàng trong một khoảng thời gian.
        /// - Dùng để hiển thị chi tiết doanh thu, lịch sử đơn, hoặc xuất báo cáo chi tiết.
        /// - Dữ liệu đọc từ bảng Donhang.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu (00:00:00).</param>
        /// <param name="endDate">Ngày kết thúc (23:59:59).</param>
        /// <returns>
        /// List&lt;Donhang&gt; chứa các đơn có Thoigiandat nằm trong khoảng.
        /// </returns>
        public List<Donhang> GetDonhangsByDateRange(DateTime startDate, DateTime endDate)
        {
            // Danh sách kết quả trả về
            List<Donhang> donhangs = new List<Donhang>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();

                    string selectSql = @"
                        SELECT Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien 
                        FROM Donhang 
                        WHERE Thoigiandat BETWEEN @StartDate AND @EndDate";

                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd 00:00:00"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd 23:59:59"));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Đọc từng dòng dữ liệu và map sang đối tượng Donhang
                            while (reader.Read())
                            {
                                Donhang donhang = new Donhang
                                {
                                    Madonhang = reader["Madonhang"].ToString(),
                                    Manhanvien = reader["Manhanvien"].ToString(),
                                    // Makhachhang có thể NULL -> kiểm tra DBNull
                                    Makhachhang = reader["Makhachhang"] != DBNull.Value
                                                    ? reader["Makhachhang"].ToString()
                                                    : null,
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
                    throw new Exception($"Lỗi DAL khi lấy đơn hàng theo khoảng thời gian: {ex.Message}", ex);
                }
            }

            return donhangs;
        }
    }
}
