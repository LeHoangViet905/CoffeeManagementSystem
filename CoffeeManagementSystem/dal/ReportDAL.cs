using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace CoffeeManagementSystem.DAL
{
    public class ReportDAL : BaseDataAccess
    {
        public ReportDAL() : base() { }

        /// <summary>
        /// Lấy tổng doanh thu trong một khoảng thời gian.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>Tổng doanh thu (decimal).</returns>
        public decimal GetTotalRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            decimal totalRevenue = 0m;
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT SUM(Tongtien) FROM Donhang WHERE Thoigiandat BETWEEN @StartDate AND @EndDate";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd 00:00:00"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd 23:59:59"));
                        object result = command.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            totalRevenue = Convert.ToDecimal(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi DAL khi lấy tổng doanh thu: {ex.Message}", ex);
                }
            }
            return totalRevenue;
        }

        /// <summary>
        /// Lấy danh sách các đơn hàng trong một khoảng thời gian.
        /// Có thể dùng để hiển thị chi tiết doanh thu.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>Danh sách các đối tượng Donhang.</returns>
        public List<Donhang> GetDonhangsByDateRange(DateTime startDate, DateTime endDate)
        {
            List<Donhang> donhangs = new List<Donhang>();
            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string selectSql = "SELECT Madonhang, Manhanvien, Makhachhang, Thoigiandat, Trangthaidon, Tongtien FROM Donhang WHERE Thoigiandat BETWEEN @StartDate AND @EndDate";
                    using (SQLiteCommand command = new SQLiteCommand(selectSql, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd 00:00:00"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd 23:59:59"));
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
                    throw new Exception($"Lỗi DAL khi lấy đơn hàng theo khoảng thời gian: {ex.Message}", ex);
                }
            }
            return donhangs;
        }
    }
}
