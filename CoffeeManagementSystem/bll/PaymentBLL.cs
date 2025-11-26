using CoffeeManagementSystem.DAL;
using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CoffeeManagementSystem.BLL
{
    public class PaymentBLL
    {
        private DonhangDAL _donhangDAL;
        private ChitietdonhangDAL _chitietdonhangDAL;
        private KhachhangDAL _khachhangDAL;
        private ThanhtoanDAL _thanhtoanDAL; // THÊM: DAL cho bảng Thanhtoan

        private List<Chitietdonhang> _dsChiTietHoaDon;
        private string _manhanvienLapHoaDon;
        private string _tenNhanVienLapHoaDon;
        private string _maHoaDonHienTai;

        private string GenerateNewKhachhangId()
        {
            // Lấy mã khách hàng lớn nhất hiện tại từ DAL
            string lastId = _khachhangDAL.GetLatestKhachhangId();  // dùng hàm bạn đã có trong DAL
            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith("KH") && lastId.Length >= 5)
            {
                string numberPart = lastId.Substring(2);   // VD: "010" trong "KH010"
                if (int.TryParse(numberPart, out int num))
                {
                    nextNumber = num + 1;
                }
            }

            string newId = $"KH{nextNumber:000}";          // KH001, KH002, KH010, KH011,...
            Logger.LogDebug($"Đã tạo ID khách hàng mới: {newId}.");
            return newId;
        }

        public string GetManhanvienLapHoaDon()
        {
            return _manhanvienLapHoaDon;
        }
        public PaymentBLL(List<Chitietdonhang> dsChiTiet, string manhanvien, string tenNhanVien)
        {
            Logger.LogInfo("Bắt đầu khởi tạo PaymentBLL.");

            _dsChiTietHoaDon = dsChiTiet ?? throw new ArgumentNullException(nameof(dsChiTiet), "Danh sách chi tiết hóa đơn không được null.");
            _manhanvienLapHoaDon = manhanvien ?? throw new ArgumentNullException(nameof(manhanvien), "Mã nhân viên không được null.");
            _tenNhanVienLapHoaDon = tenNhanVien ?? throw new ArgumentNullException(nameof(tenNhanVien), "Tên nhân viên không được null.");

            _donhangDAL = new DonhangDAL();
            _chitietdonhangDAL = new ChitietdonhangDAL();
            _khachhangDAL = new KhachhangDAL();
            _thanhtoanDAL = new ThanhtoanDAL(); // KHỞI TẠO: ThanhtoanDAL

            _maHoaDonHienTai = GenerateUniqueDonhangId();

            Logger.LogInfo($"PaymentBLL đã được khởi tạo. Mã hóa đơn tạm thời: {_maHoaDonHienTai}, Nhân viên: {_tenNhanVienLapHoaDon} ({_manhanvienLapHoaDon}).");
            Logger.LogDebug($"Số lượng chi tiết đơn hàng ban đầu: {_dsChiTietHoaDon.Count}.");
        }

        // --- Các phương thức để Form có thể truy xuất thông tin hiển thị ---
        public string GetMaHoaDonHienTai()
        {
            Logger.LogDebug($"Truy xuất mã hóa đơn hiện tại: {_maHoaDonHienTai}.");
            return _maHoaDonHienTai;
        }

        public string GetTenNhanVienLapHoaDon()
        {
            Logger.LogDebug($"Truy xuất tên nhân viên lập hóa đơn: {_tenNhanVienLapHoaDon}.");
            return _tenNhanVienLapHoaDon;
        }

        public DateTime GetNgayLapHoaDon()
        {
            DateTime now = DateTime.Now;
            Logger.LogDebug($"Truy xuất ngày lập hóa đơn: {now.ToShortDateString()}.");
            return now; // Ngày lập là thời điểm hiện tại
        }

        public List<Chitietdonhang> GetDsChiTietHoaDon()
        {
            Logger.LogDebug($"Truy xuất danh sách chi tiết hóa đơn, số lượng: {_dsChiTietHoaDon.Count}.");
            return _dsChiTietHoaDon;
        }

        /// <summary>
        /// Tính toán tổng thành tiền từ danh sách chi tiết đơn hàng.
        /// </summary>
        /// <returns>Tổng tiền của đơn hàng.</returns>
        public decimal CalculateTongTien()
        {
            decimal tongTien = _dsChiTietHoaDon.Sum(item => item.Thanhtien);
            Logger.LogDebug($"Đã tính tổng tiền hóa đơn: {tongTien:N0}.");
            return tongTien;
        }

        /// <summary>
        /// Phương thức tạo ID duy nhất cho Madonhang (DH + timestamp).
        /// </summary>
        private string GenerateUniqueDonhangId()
        {
            string newId = "DH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Logger.LogDebug($"Đã tạo ID đơn hàng mới: {newId}.");
            return newId;
        }


        private string GenerateUniqueKhachhangId()
        {
            string newId = "KH" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Logger.LogDebug($"Đã tạo ID khách hàng mới: {newId}.");
            return newId;
        }

        private string GenerateUniqueThanhtoanId()
        {
            string newId = "TT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Logger.LogDebug($"Đã tạo ID thanh toán mới: {newId}.");
            return newId;
        }


        public bool ProcessPayment(string customerName, string hinhThucThanhToan, string manhanvienThuNgan, string ghiChu, out Khachhang customerToUse)
        {
            Logger.LogInfo($"Bắt đầu ProcessPayment cho khách hàng: '{customerName ?? "Khách vãng lai"}', Hình thức TT: {hinhThucThanhToan}");
            customerToUse = null; // Khởi tạo giá trị mặc định cho tham số out

            if (!_dsChiTietHoaDon.Any())
            {
                Logger.LogWarning("Không có đồ uống nào trong hóa đơn để thanh toán.");
                throw new InvalidOperationException("Không có đồ uống nào trong hóa đơn để thanh toán.");
            }

            decimal tongTienHoaDon = CalculateTongTien();
            string makhachhang = null;

            // Xử lý khách hàng nếu tên không rỗng
            if (!string.IsNullOrWhiteSpace(customerName))
            {
                try
                {
                    customerToUse = _khachhangDAL.GetKhachhangByName(customerName);

                    if (customerToUse == null)
                    {
                        Logger.LogInfo($"Khách hàng '{customerName}' không tìm thấy trong CSDL.");
                        throw new KhachhangNotFoundException($"Khách hàng '{customerName}' chưa tồn tại.");
                    }
                    else
                    {
                        Logger.LogInfo($"Đã tìm thấy khách hàng: {customerToUse.Hoten} (Mã: {customerToUse.Makhachhang}).");
                        makhachhang = customerToUse.Makhachhang;
                    }
                }
                catch (KhachhangNotFoundException) // Chỉ bắt exception này để ném lại nguyên vẹn
                {
                    throw; // Ném lại để PaymentForm xử lý logic thêm khách hàng
                }
                catch (Exception ex)
                {
                    // LOG: Lỗi khi tìm kiếm khách hàng
                    Logger.LogError($"Lỗi khi tìm kiếm khách hàng '{customerName}'.", ex);
                    throw new Exception($"Lỗi khi xử lý thông tin khách hàng: {ex.Message}", ex);
                }
            }
            else
            {
                Logger.LogInfo("Không có tên khách hàng được cung cấp. Xử lý như khách vãng lai.");
            }

            Donhang newDonhang = new Donhang
            {
                Madonhang = _maHoaDonHienTai,
                Manhanvien = _manhanvienLapHoaDon,
                Makhachhang = makhachhang, // Có thể là null nếu không có khách hàng
                Thoigiandat = DateTime.Now,
                Trangthaidon = "Hoàn thành",
                Tongtien = tongTienHoaDon
            };

            // Tạo đối tượng Thanhtoan
            Thanhtoan newThanhtoan = new Thanhtoan
            {
                Mathanhtoan = GenerateUniqueThanhtoanId(), // Tạo ID duy nhất cho thanh toán
                Madonhang = _maHoaDonHienTai, // Liên kết với đơn hàng vừa tạo
                Thoigianthanhtoan = DateTime.Now,
                Hinhthucthanhtoan = hinhThucThanhToan,
                Sotienthanhtoan = tongTienHoaDon,
                Manhanvienthu = manhanvienThuNgan, // Mã nhân viên thu tiền
                Ghichu = ghiChu // Ghi chú thanh toán
            };

            // Sử dụng transaction để đảm bảo tính toàn vẹn dữ liệu
            using (SQLiteConnection connection = new SQLiteConnection(new BaseDataAccess().ConnectionString))
            {
                connection.Open();
                SQLiteTransaction transaction = connection.BeginTransaction();
                try
                {
                    // LOG: Bắt đầu thêm đơn hàng vào CSDL
                    Logger.LogDebug($"Đang thêm đơn hàng vào CSDL: Mã='{newDonhang.Madonhang}', NV='{newDonhang.Manhanvien}', KH='{newDonhang.Makhachhang ?? "N/A"}', Tổng={newDonhang.Tongtien:N0}.");
                    _donhangDAL.AddDonhang(newDonhang, connection, transaction);
                    Logger.LogInfo($"Đã thêm đơn hàng thành công: Mã='{newDonhang.Madonhang}'.");

                    foreach (var item in _dsChiTietHoaDon)
                    {
                        item.Madonhang = _maHoaDonHienTai;
                        // LOG: Thêm chi tiết đơn hàng
                        Logger.LogDebug($"Đang thêm chi tiết đơn hàng: HD='{item.Madonhang}', Đồ uống='{item.Madouong}', SL={item.Soluong}, TT={item.Thanhtien:N0}.");
                        _chitietdonhangDAL.AddChitietdonhang(item, connection, transaction);
                    }
                    Logger.LogInfo($"Đã thêm {_dsChiTietHoaDon.Count} chi tiết đơn hàng cho đơn hàng '{_maHoaDonHienTai}'.");

                    // THÊM: Lưu thông tin thanh toán vào CSDL
                    Logger.LogDebug($"Đang thêm thông tin thanh toán: Mã='{newThanhtoan.Mathanhtoan}', ĐH='{newThanhtoan.Madonhang}', HTTT='{newThanhtoan.Hinhthucthanhtoan}', ST={newThanhtoan.Sotienthanhtoan:N0}, NVThu='{newThanhtoan.Manhanvienthu ?? "N/A"}', Ghi chú='{newThanhtoan.Ghichu ?? ""}'.");
                    _thanhtoanDAL.AddThanhtoan(newThanhtoan, connection, transaction);
                    Logger.LogInfo($"Đã thêm thông tin thanh toán thành công: Mã='{newThanhtoan.Mathanhtoan}'.");

                    // Nếu có khách hàng, cập nhật điểm tích lũy (giả sử 1000đ = 1 điểm)
                    if (customerToUse != null)
                    {
                        int diemTichLuyThem = (int)(tongTienHoaDon / 1000); // 1000 VND = 1 point
                        customerToUse.Diemtichluy += diemTichLuyThem;
                        _khachhangDAL.UpdateKhachhang(customerToUse, connection, transaction);
                        Logger.LogInfo($"Đã cập nhật điểm tích lũy cho khách hàng '{customerToUse.Hoten}' (Mã: {customerToUse.Makhachhang}): Thêm {diemTichLuyThem} điểm, Tổng hiện tại: {customerToUse.Diemtichluy}.");
                    }

                    transaction.Commit();
                    // LOG: Giao dịch thành công
                    Logger.LogInfo($"Giao dịch thanh toán cho đơn hàng '{_maHoaDonHienTai}' đã được COMMIT thành công.");
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    // LOG: Giao dịch bị Rollback do lỗi
                    Logger.LogError($"Giao dịch thanh toán cho đơn hàng '{_maHoaDonHienTai}' đã bị ROLLBACK do lỗi: {ex.Message}", ex);
                    throw new Exception($"Lỗi khi lưu đơn hàng, chi tiết và thanh toán vào CSDL: {ex.Message}", ex);
                }
                finally
                {
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        connection.Close();
                        Logger.LogDebug("Đã đóng kết nối CSDL sau giao dịch.");
                    }
                }
            }
        }

        public Khachhang GetKhachhangByName(string customerName)
        {
            // LOG: Bắt đầu tìm kiếm khách hàng
            Logger.LogInfo($"Bắt đầu tìm kiếm khách hàng với tên: '{customerName}'.");
            if (string.IsNullOrWhiteSpace(customerName))
            {
                Logger.LogDebug("Tên khách hàng rỗng khi tìm kiếm.");
                return null;
            }
            try
            {
                Khachhang foundCustomer = _khachhangDAL.GetKhachhangByName(customerName);
                if (foundCustomer != null)
                {
                    Logger.LogInfo($"Đã tìm thấy khách hàng: '{foundCustomer.Hoten}' (Mã: {foundCustomer.Makhachhang}).");
                }
                else
                {
                    Logger.LogInfo($"Không tìm thấy khách hàng với tên: '{customerName}'.");
                }
                return foundCustomer;
            }
            catch (Exception ex)
            {
                // LOG: Lỗi khi tìm kiếm khách hàng
                Logger.LogError($"Lỗi khi tìm kiếm khách hàng theo tên '{customerName}'.", ex);
                throw new Exception($"Lỗi khi tìm khách hàng: {ex.Message}", ex);
            }
        }

        public Khachhang AddNewKhachhang(string customerName)
        {
            Logger.LogInfo($"Bắt đầu thêm khách hàng mới: '{customerName}'.");

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Tên khách hàng không được trống.", nameof(customerName));

            try
            {
                Khachhang newCustomer = new Khachhang
                {
                    Makhachhang = GenerateNewKhachhangId(),   // dùng hàm mới
                    Hoten = customerName,
                    Ngaydangky = DateTime.Now,
                    Diemtichluy = 0
                };

                _khachhangDAL.AddKhachhang(newCustomer);
                Logger.LogInfo($"Đã thêm mới khách hàng thành công: '{newCustomer.Hoten}' (Mã: {newCustomer.Makhachhang}).");

                return newCustomer;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Lỗi khi thêm mới khách hàng '{customerName}'.", ex);
                throw new Exception($"Không thể thêm mới khách hàng: {ex.Message}", ex);
            }


        }
    }

    // Định nghĩa một Exception tùy chỉnh để BLL báo hiệu "không tìm thấy khách hàng"
    // mà không cần dùng MessageBox trong BLL
    public class KhachhangNotFoundException : Exception
    {
        public KhachhangNotFoundException(string message) : base(message) { }
        public KhachhangNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}