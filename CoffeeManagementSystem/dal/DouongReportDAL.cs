using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms; // Chỉ dùng cho MessageBox ví dụ trong DAL

// Đảm bảo namespace này chứa các lớp model như Douong, Chitietdonhang, Donhang

namespace CoffeeManagementSystem.DAL
{
    public class DouongReportDAL : BaseDataAccess
    {
        public DouongReportDAL() : base() { }

        /// <summary>
        /// Lấy báo cáo bán hàng theo đồ uống trong một khoảng thời gian cụ thể.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu của khoảng thời gian báo cáo.</param>
        /// <param name="endDate">Ngày kết thúc của khoảng thời gian báo cáo.</param>
        /// <returns>Danh sách các đối tượng ProductSalesReportItem.</returns>
        public List<ProductSalesReportItem> GetProductSalesReport(DateTime startDate, DateTime endDate)
        {
            List<ProductSalesReportItem> reportData = new List<ProductSalesReportItem>();

            using (SQLiteConnection connection = new SQLiteConnection(ConnectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT
                            d.Madouong,
                            d.Tendouong,
                            d.Maloai, -- Giả sử có cột Maloai trong bảng Douong
                            SUM(ct.Soluong) AS SoLuongBan,
                            SUM(ct.Thanhtien) AS TongDoanhThuMon
                        FROM Chitietdonhang ct
                        JOIN Donhang dh ON ct.Madonhang = dh.Madonhang
                        JOIN Douong d ON ct.Madouong = d.Madouong
                        WHERE DATE(dh.Thoigiandat) >= DATE(@StartDate) AND DATE(dh.Thoigiandat) <= DATE(@EndDate)
                        GROUP BY d.Madouong, d.Tendouong, d.Maloai
                        ORDER BY TongDoanhThuMon DESC;"; // Sắp xếp theo tổng doanh thu giảm dần

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@StartDate", startDate.ToString("yyyy-MM-dd"));
                        command.Parameters.AddWithValue("@EndDate", endDate.ToString("yyyy-MM-dd"));

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                reportData.Add(new ProductSalesReportItem
                                {
                                    Madouong = reader["Madouong"].ToString(),
                                    Tendouong = reader["Tendouong"].ToString(),
                                    Maloai = reader["Maloai"] != DBNull.Value ? reader["Maloai"].ToString() : "Không xác định",
                                    SoLuongBan = Convert.ToInt32(reader["SoLuongBan"]),
                                    TongDoanhThuMon = Convert.ToDecimal(reader["TongDoanhThuMon"])
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Trong môi trường thực tế, bạn nên log lỗi thay vì hiển thị MessageBox ở tầng DAL
                    MessageBox.Show($"Lỗi DAL khi lấy báo cáo bán hàng theo đồ uống: {ex.Message}", "Lỗi CSDL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw; // Re-throw để tầng nghiệp vụ (BLL) hoặc UI có thể xử lý
                }
            }
            return reportData;
        }
    }

    // =====================================================
    // Lớp Model mới cho báo cáo bán hàng theo đồ uống
    // =====================================================
    public class ProductSalesReportItem
    {
        public string Madouong { get; set; }
        public string Tendouong { get; set; }
        public string Maloai { get; set; } // Loại đồ uống
        public int SoLuongBan { get; set; }
        public decimal TongDoanhThuMon { get; set; }
        // Thuộc tính này sẽ được tính toán ở tầng UI hoặc BLL
        public decimal TyLeDongGopDoanhThu { get; set; }
    }
}
