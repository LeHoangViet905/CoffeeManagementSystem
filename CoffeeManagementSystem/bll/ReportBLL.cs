using CoffeeManagementSystem.DAL; // Để sử dụng các lớp DAL
using System;
using System.Collections.Generic;

namespace CoffeeManagementSystem.BLL
{
    public class ReportBLL
    {
        private KhachhangDAL _khachhangDAL;
        private DonhangDAL _donhangDAL;
        private DouongReportDAL _douongReportDAL;

        public ReportBLL()
        {
            _khachhangDAL = new KhachhangDAL();
            _donhangDAL = new DonhangDAL();
            _douongReportDAL = new DouongReportDAL();
        }

        /// <summary>
        /// Lấy danh sách 10 khách hàng có điểm tích lũy cao nhất.
        /// </summary>
        /// <returns>List Khachhang.</returns>
        public List<Khachhang> GetPotentialCustomersReport()
        {
            return _khachhangDAL.GetTop10HighestDiemTichLuyCustomers();
        }

        /// <summary>
        /// Lấy dữ liệu báo cáo doanh thu trong một khoảng thời gian.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>List RevenueReportItem.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu ngày bắt đầu lớn hơn ngày kết thúc.</exception>
        public List<RevenueReportItem> GetRevenueReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }
            return _donhangDAL.GetRevenueByDateRange(startDate, endDate);
        }

        /// <summary>
        /// Lấy dữ liệu báo cáo bán hàng theo đồ uống trong một khoảng thời gian.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>List ProductSalesReportItem.</returns>
        /// <exception cref="ArgumentException">Ném ra nếu ngày bắt đầu lớn hơn ngày kết thúc.</exception>
        public List<ProductSalesReportItem> GetProductSalesReport(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Ngày bắt đầu không được lớn hơn ngày kết thúc.");
            }
            return _douongReportDAL.GetProductSalesReport(startDate, endDate);
        }
    }
}