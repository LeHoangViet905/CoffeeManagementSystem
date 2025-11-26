using CoffeeManagementSystem.DAL; // Vẫn cần để khởi tạo các DAL
using System;
using System.Collections.Generic;
using System.Data.SQLite; // Cần để quản lý transaction trong BLL
using System.Linq;

namespace CoffeeManagementSystem.BLL
{
    public class OrderBLL
    {
        private DouongBLL _douongBLL;
        private DonhangDAL _donhangDAL;
        private ChitietdonhangDAL _chitietdonhangDAL;
        private string _loggedInManhanvien; // Mã nhân viên lập hóa đơn

        // BỔ SUNG: Danh sách chi tiết hóa đơn tạm thời được CHUYỂN từ Form sang BLL
        // Đây chính là biến danhSachChiTietHoaDonTamThoi đã có trong OrderForm
        private List<Chitietdonhang> _danhSachChiTietHoaDonTamThoi;

        public OrderBLL(string manhanvien)
        {
            _douongBLL = new DouongBLL(); // Khởi tạo DouongBLL
            _donhangDAL = new DonhangDAL();
            _chitietdonhangDAL = new ChitietdonhangDAL();
            _loggedInManhanvien = manhanvien;

            // BỔ SUNG: Khởi tạo danh sách chi tiết hóa đơn tạm thời
            _danhSachChiTietHoaDonTamThoi = new List<Chitietdonhang>();
        }

        /// <summary>
        /// Tải tất cả danh sách đồ uống từ DouongBLL (đã bao gồm giá mới nhất).
        /// </summary>
        /// <returns>Danh sách Douong.</returns>
        public List<Douong> LoadAllDouongs()
        {
            try
            {
                // Gọi DouongBLL để lấy dữ liệu đồ uống
                return _douongBLL.GetAllDouongs();
            }
            catch (Exception ex)
            {
                // Ném lại lỗi để Form xử lý và hiển thị thông báo
                throw new Exception($"Lỗi BLL khi tải danh sách đồ uống: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm đồ uống theo từ khóa từ DouongBLL (đã bao gồm giá mới nhất).
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm.</param>
        /// <returns>Danh sách Douong phù hợp.</returns>
        public List<Douong> SearchDouongs(string searchTerm)
        {
            try
            {
                // Bổ sung logic tìm kiếm cụ thể từ DouongBLL nếu có
                // Hiện tại đang gọi GetAllDouongs, có thể cần phương thức Search của DouongBLL
                return _douongBLL.GetAllDouongs().Where(d =>
                    d.Tendouong.ToLower().Contains(searchTerm.ToLower()) ||
                    d.Madouong.ToLower().Contains(searchTerm.ToLower())
                ).ToList();
            }
            catch (Exception ex)
            {
                // Ném lại lỗi để Form xử lý
                throw new Exception($"Lỗi BLL khi tìm kiếm đồ uống: {ex.Message}", ex);
            }
        }

        // --- CÁC PHƯƠNG THỨC ĐƯỢC CHUYỂN HOÀN TOÀN TỪ OrderForm SANG OrderBLL ---

        /// <summary>
        /// **CHUYỂN TỪ FORM**: Thêm một đồ uống vào danh sách chi tiết hóa đơn tạm thời hoặc tăng số lượng nếu đã có.
        /// Logic này tương ứng với phần thêm vào `danhSachChiTietHoaDonTamThoi` trong sự kiện `dgvDouong_CellDoubleClick` của bạn.
        /// </summary>
        /// <param name="selectedDouong">Đồ uống được chọn.</param>
        /// <returns>Tổng số lượng món đã chọn trong hóa đơn tạm thời.</returns>
        public int AddOrUpdateChiTietHoaDonTamThoi(Douong selectedDouong)
        {
            if (selectedDouong == null)
            {
                throw new ArgumentNullException(nameof(selectedDouong), "Đồ uống được chọn không thể null.");
            }

            // Tìm kiếm xem món đồ uống đã có trong danh sách tạm thời chưa
            Chitietdonhang existingItem = _danhSachChiTietHoaDonTamThoi.FirstOrDefault(item => item.Madouong == selectedDouong.Madouong);

            if (existingItem != null)
            {
                // Nếu đã có, tăng số lượng và cập nhật thành tiền
                existingItem.Soluong++;
                // Sử dụng CurrentGia của Douong, như đã có trong Form
                existingItem.Thanhtien = existingItem.Dongia * existingItem.Soluong;
            }
            else
            {
                // Nếu chưa có, thêm món mới vào danh sách
                if (selectedDouong.CurrentGia <= 0)
                {
                    // Giữ nguyên thông báo lỗi từ form gốc
                    throw new InvalidOperationException($"Không thể thêm đồ uống '{selectedDouong.Tendouong}' vì giá bán không hợp lệ.");
                }

                _danhSachChiTietHoaDonTamThoi.Add(new Chitietdonhang
                {
                    // Madonhang sẽ được gán khi lưu vào DB (trong CreateNewOrder)
                    Madouong = selectedDouong.Madouong,
                    Tendouong = selectedDouong.Tendouong,
                    Dongia = selectedDouong.CurrentGia, // Lấy giá hiện tại từ đối tượng Douong
                    Soluong = 1,
                    Thanhtien = selectedDouong.CurrentGia * 1
                });
            }
            return _danhSachChiTietHoaDonTamThoi.Sum(item => item.Soluong);
        }

        /// <summary>
        /// **CHUYỂN TỪ FORM**: Phương thức tạo mã đơn hàng duy nhất.
        /// (Lưu ý: Logic này đã tồn tại trong phương thức GenerateUniqueMadonhangId của bạn,
        /// tôi giữ nguyên tên và logic bạn đã có).
        /// </summary>
        /// <returns>Mã đơn hàng duy nhất.</returns>
        private string GenerateUniqueMadonhangId()
        {
            return "HD" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }

        /// <summary>
        /// **CHUYỂN TỪ FORM & TÍCH HỢP**: Tạo hóa đơn mới và lưu chi tiết đơn hàng vào cơ sở dữ liệu.
        /// Phương thức này sử dụng Transaction để đảm bảo tính toàn vẹn dữ liệu.
        /// Giờ đây nó sẽ sử dụng danh sách tạm thời (_danhSachChiTietHoaDonTamThoi) nội bộ của BLL.
        /// </summary>
        /// <returns>True nếu tạo hóa đơn thành công, False nếu không.</returns>
        public bool CreateNewOrder()
        {
            // Kiểm tra ràng buộc nghiệp vụ
            if (_danhSachChiTietHoaDonTamThoi == null || !_danhSachChiTietHoaDonTamThoi.Any())
            {
                throw new ArgumentException("Danh sách chi tiết đơn hàng tạm thời trống. Không thể tạo hóa đơn.");
            }
            if (string.IsNullOrEmpty(_loggedInManhanvien))
            {
                throw new InvalidOperationException("Mã nhân viên lập hóa đơn không được trống. Vui lòng đăng nhập.");
            }

            // Tính tổng tiền của hóa đơn từ danh sách tạm thời của BLL
            decimal tongTien = _danhSachChiTietHoaDonTamThoi.Sum(item => item.Thanhtien);

            // Tạo đối tượng Donhang mới
            Donhang newDonhang = new Donhang
            {
                Madonhang = GenerateUniqueMadonhangId(), // Tạo mã hóa đơn duy nhất
                Manhanvien = _loggedInManhanvien,
                Thoigiandat = DateTime.Now,
                Tongtien = tongTien,
                Trangthaidon = "Chưa thanh toán" // Trạng thái mặc định khi mới tạo
            };

            // Sử dụng transaction để đảm bảo rằng việc thêm hóa đơn và tất cả chi tiết hóa đơn
            // hoặc thành công hoàn toàn, hoặc thất bại hoàn toàn (Rollback).
            // Lấy ConnectionString từ BaseDataAccess để khởi tạo kết nối.
            using (SQLiteConnection connection = new SQLiteConnection(new BaseDataAccess().ConnectionString))
            {
                connection.Open();
                SQLiteTransaction transaction = connection.BeginTransaction();
                try
                {
                    // 1. Thêm hóa đơn chính vào CSDL
                    _donhangDAL.AddDonhang(newDonhang, connection, transaction);

                    // 2. Thêm từng chi tiết hóa đơn vào CSDL
                    foreach (var chiTiet in _danhSachChiTietHoaDonTamThoi) // Sử dụng danh sách tạm thời của BLL
                    {
                        chiTiet.Madonhang = newDonhang.Madonhang; // Gán mã hóa đơn vừa tạo cho chi tiết
                        // Không gán Machitietdonhang vì DB schema sử dụng khóa chính kép (Madonhang, Madouong)
                        _chitietdonhangDAL.AddChitietdonhang(chiTiet, connection, transaction);
                    }

                    transaction.Commit(); // Commit transaction nếu mọi thao tác thành công
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback(); // Rollback nếu có bất kỳ lỗi nào xảy ra
                    // Ném lại lỗi để Form xử lý và hiển thị thông báo
                    throw new Exception($"Lỗi BLL khi tạo hóa đơn: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// BỔ SUNG: Xóa danh sách chi tiết hóa đơn tạm thời.
        /// Logic này tương ứng với `danhSachChiTietHoaDonTamThoi.Clear()` trong Form của bạn.
        /// </summary>
        public void ClearTemporaryOrderDetails()
        {
            _danhSachChiTietHoaDonTamThoi.Clear();
        }

        /// <summary>
        /// BỔ SUNG: Trả về danh sách chi tiết hóa đơn tạm thời cho Form hoặc PaymentForm.
        /// Logic này tương ứng với việc truyền `danhSachChiTietHoaDonTamThoi` trong Form của bạn.
        /// </summary>
        /// <returns>Danh sách các Chitietdonhang tạm thời.</returns>
        public List<Chitietdonhang> GetTemporaryOrderDetails()
        {
            return _danhSachChiTietHoaDonTamThoi;
        }
        public string GetCurrentMaNhanVien()
        {
            return _loggedInManhanvien;
        }
    }
}