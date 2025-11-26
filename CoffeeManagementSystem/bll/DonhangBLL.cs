using CoffeeManagementSystem.DAL; // Tham chiếu đến tầng DAL
using System;
using System.Collections.Generic;
using System.Data;

namespace CoffeeManagementSystem.BLL
{
    public class DonhangBLL
    {
        private DonhangDAL _donhangDAL;

        public DonhangBLL()
        {
            _donhangDAL = new DonhangDAL();
        }

        /// <summary>
        /// Thêm một đơn hàng mới.
        /// BLL có thể thêm logic kiểm tra dữ liệu hoặc quy tắc nghiệp vụ tại đây.
        /// </summary>
        /// <param name="donhang">Đối tượng Donhang cần thêm.</param>
        public void AddDonhang(Donhang donhang)
        {
            // Logic nghiệp vụ có thể đặt ở đây (ví dụ: kiểm tra Madonhang không trùng, kiểm tra giá trị hợp lệ)
            if (string.IsNullOrWhiteSpace(donhang.Madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(donhang.Manhanvien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }
            if (donhang.Tongtien < 0)
            {
                throw new ArgumentOutOfRangeException("Tổng tiền không thể là số âm.");
            }

            try
            {
                _donhangDAL.AddDonhang(donhang);
            }
            catch (Exception ex)
            {
                // Bắt lỗi từ DAL và xử lý hoặc ném lại lỗi với thông báo thân thiện hơn
                throw new Exception($"Lỗi BLL khi thêm đơn hàng: {ex.Message}", ex);
            }
        }




        /// <summary>
        /// Lấy tất cả các đơn hàng.
        /// </summary>
        /// <returns>Danh sách các đối tượng Donhang.</returns>
        public List<Donhang> GetAllDonhangs()
        {
            try
            {
                return _donhangDAL.GetAllDonhangs();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy tất cả đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo mã.
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng.</param>
        /// <returns>Đối tượng Donhang nếu tìm thấy, ngược lại là null.</returns>
        public Donhang GetDonhangById(string madonhang)
        {
            if (string.IsNullOrWhiteSpace(madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống khi tìm kiếm.");
            }

            try
            {
                return _donhangDAL.GetDonhangById(madonhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi lấy đơn hàng theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin đơn hàng.
        /// </summary>
        /// <param name="donhang">Đối tượng Donhang với thông tin đã cập nhật.</param>
        public void UpdateDonhang(Donhang donhang)
        {
            // Logic nghiệp vụ có thể đặt ở đây (ví dụ: kiểm tra tồn tại của đơn hàng, kiểm tra giá trị mới)
            if (string.IsNullOrWhiteSpace(donhang.Madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống khi cập nhật.");
            }
            if (string.IsNullOrWhiteSpace(donhang.Manhanvien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.");
            }
            if (donhang.Tongtien < 0)
            {
                throw new ArgumentOutOfRangeException("Tổng tiền không thể là số âm.");
            }

            try
            {
                _donhangDAL.UpdateDonhang(donhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi cập nhật đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa một đơn hàng.
        /// </summary>
        /// <param name="madonhang">Mã đơn hàng cần xóa.</param>
        public void DeleteDonhang(string madonhang)
        {
            // Logic nghiệp vụ có thể đặt ở đây (ví dụ: kiểm tra xem đơn hàng có thể xóa không, có chi tiết liên quan không)
            if (string.IsNullOrWhiteSpace(madonhang))
            {
                throw new ArgumentException("Mã đơn hàng không được để trống khi xóa.");
            }

            try
            {
                _donhangDAL.DeleteDonhang(madonhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi xóa đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm đơn hàng dựa trên từ khóa.
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách các đơn hàng phù hợp.</returns>
        public List<Donhang> SearchDonhangs(string searchTerm)
        {
            // BLL có thể thêm logic kiểm tra searchTerm hoặc tối ưu hóa tìm kiếm
            try
            {
                return _donhangDAL.SearchDonhangs(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi BLL khi tìm kiếm đơn hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy báo cáo doanh thu theo khoảng thời gian.
        /// </summary>
        /// <param name="startDate">Ngày bắt đầu.</param>
        /// <param name="endDate">Ngày kết thúc.</param>
        /// <returns>Danh sách các mục báo cáo doanh thu.</returns>
        public List<RevenueReportItem> GetRevenueByDateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate > endDate)
            {
                throw new ArgumentException("Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
            }

            try
            {
                // Giữ lại MessageBox.Show trong DAL của bạn cho phương thức này vì nó là một phương thức tiện ích báo cáo,
                // nhưng trong một thiết kế chặt chẽ hơn, BLL sẽ bắt và xử lý ngoại lệ hoặc ném lại.
                return _donhangDAL.GetRevenueByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                // Ở đây, BLL bắt lỗi từ DAL và ném lại lỗi để tầng UI có thể hiển thị một thông báo phù hợp.
                throw new Exception($"Lỗi BLL khi lấy báo cáo doanh thu: {ex.Message}", ex);
            }
        }
        //thêm dashboard cho report

        public DataTable GetRevenueByHour(DateTime date)
        {
            return _donhangDAL.GetRevenueByHour(date);
        }

        public int GetOrderCount(DateTime start, DateTime end)
        {
            return _donhangDAL.GetOrderCount(start, end);
        }

    }
}