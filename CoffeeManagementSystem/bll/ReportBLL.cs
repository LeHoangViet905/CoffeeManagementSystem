using CoffeeManagementSystem.DAL; // Để sử dụng các lớp DAL
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// ReportBLL (Business Logic Layer cho báo cáo)
    /// -------------------------------------------------
    /// - Đứng giữa UI (ReportForm) và các lớp DAL.
    /// - Nhiệm vụ chính:
    ///   + Gọi các hàm DAL để lấy dữ liệu cho báo cáo.
    ///   + Kiểm tra/ràng buộc nghiệp vụ cơ bản (như: startDate <= endDate).
    ///   + Ẩn chi tiết truy vấn CSDL khỏi UI, giúp Form chỉ làm việc với object C#.
    ///
    /// Luồng chung:
    ///   ReportForm  →  ReportBLL  →  (KhachhangDAL / DonhangDAL / DouongReportDAL)  →  SQLite
    /// </summary>
    public class ReportBLL
    {
        // DAL làm việc với bảng Khachhang
        private KhachhangDAL _khachhangDAL;

        // DAL làm việc với bảng Donhang (và các view/report liên quan doanh thu)
        private DonhangDAL _donhangDAL;

        // DAL làm việc với báo cáo đồ uống (doanh số bán theo từng món)
        private DouongReportDAL _douongReportDAL;

        /// <summary>
        /// Constructor:
        /// - Khởi tạo các đối tượng DAL cần dùng cho báo cáo.
        /// - ReportForm chỉ cần dùng BLL, không cần biết chi tiết DAL bên dưới.
        /// </summary>
        public ReportBLL()
        {
            _khachhangDAL = new KhachhangDAL();
            _donhangDAL = new DonhangDAL();
            _douongReportDAL = new DouongReportDAL();
        }

        /// <summary>
        /// Lấy danh sách TOP 10 khách hàng có điểm tích lũy cao nhất.
        /// ---------------------------------------------------------
        /// - Không có logic tính toán phức tạp ở BLL, chỉ gọi thẳng DAL.
        /// - Dùng để hiển thị lên tab "Khách hàng tiềm năng" trong ReportForm.
        /// </summary>
        /// <returns>
        /// List&lt;Khachhang&gt; gồm tối đa 10 khách có DiemTichLuy cao nhất.
        /// </returns>
        public List<Khachhang> GetPotentialCustomersReport()
        {
            // Gọi xuống KhachhangDAL để lấy đúng 10 khách hàng có điểm cao nhất
            return _khachhangDAL.GetTop10HighestDiemTichLuyCustomers();
        }

        /// <summary>
        /// Lấy dữ liệu báo cáo DOANH THU trong một khoảng thời gian.
        /// ---------------------------------------------------------
        /// - Kiểm tra ràng buộc: startDate phải <= endDate.
        /// - Sau đó gọi DonhangDAL.GetRevenueByDateRange().
        /// - Kết quả trả về cho ReportForm để bind lên DataGridView & Dashboard.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu (tính từ 00:00:00).</param>
        /// <param name="endDate">Ngày kết thúc (tới 23:59:59).</param>
        /// <returns>
        /// List&lt;RevenueReportItem&gt;:
        ///   + Mỗi phần tử là doanh thu theo 1 ngày.
        ///   + Dùng cho: dgvRevenue, chartDashboard, dashboard tổng hợp.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ném ra nếu ngày bắt đầu lớn hơn ngày kết thúc,
        /// để UI (ReportForm) hiển thị MessageBox cảnh báo.
        /// </exception>
        public List<RevenueReportItem> GetRevenueReport(DateTime startDate, DateTime endDate)
        {
            // RÀNG BUỘC NGHIỆP VỤ: không cho phép khoảng ngày ngược
            if (startDate > endDate)
            {
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }

            // Gọi xuống DAL để lấy danh sách RevenueReportItem
            return _donhangDAL.GetRevenueByDateRange(startDate, endDate);
        }

        /// <summary>
        /// Lấy dữ liệu báo cáo BÁN HÀNG THEO ĐỒ UỐNG trong một khoảng thời gian.
        /// ---------------------------------------------------------------------
        /// - Dùng cho tab "Sản phẩm bán chạy" (dgvProductSales + pie chart nếu cần).
        /// - BLL kiểm tra hợp lệ ngày, còn logic tổng hợp theo món nằm ở DouongReportDAL.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>
        /// List&lt;ProductSalesReportItem&gt;:
        ///   + Mỗi phần tử là 1 món đồ uống (Madouong, Tendouong, Maloai,
        ///     SoLuongBan, TongDoanhThuMon, ...).
        ///   + ReportForm dùng list này để:
        ///       - Bind lên dgvProductSales
        ///       - Tính tổng doanh thu, tỷ lệ đóng góp, v.v.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Ném ra nếu startDate > endDate để Form xử lý hiển thị lỗi.
        /// </exception>
        public List<ProductSalesReportItem> GetProductSalesReport(DateTime startDate, DateTime endDate)
        {
            // RÀNG BUỘC NGHIỆP VỤ: không cho phép khoảng ngày ngược
            if (startDate > endDate)
            {
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }

            // Gọi xuống DAL chuyên cho báo cáo đồ uống
            return _douongReportDAL.GetProductSalesReport(startDate, endDate);
        }
    }
}
