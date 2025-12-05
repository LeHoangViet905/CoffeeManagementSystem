using CoffeeManagementSystem.DAL;
using CoffeeManagementSystem.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// Lớp PaymentBLL:
    /// - Xử lý nghiệp vụ thanh toán của 1 hóa đơn:
    ///     + Tính tổng tiền
    ///     + Lưu đơn hàng (Donhang)
    ///     + Lưu chi tiết đơn hàng (Chitietdonhang)
    ///     + Lưu thông tin thanh toán (Thanhtoan)
    ///     + Xử lý khách hàng và điểm tích lũy
    /// - Được sử dụng bởi PaymentForm (UI).
    /// </summary>
    public class PaymentBLL
    {
        // Các lớp DAL tương ứng với bảng trong CSDL
        private DonhangDAL _donhangDAL;
        private ChitietdonhangDAL _chitietdonhangDAL;
        private KhachhangDAL _khachhangDAL;
        private ThanhtoanDAL _thanhtoanDAL; // DAL cho bảng Thanhtoan

        // Danh sách chi tiết đồ uống của hóa đơn hiện tại
        private List<Chitietdonhang> _dsChiTietHoaDon;

        // Thông tin nhân viên lập hóa đơn
        private string _manhanvienLapHoaDon;
        private string _tenNhanVienLapHoaDon;

        // Mã hóa đơn (Madonhang) hiện tại
        private string _maHoaDonHienTai;

        /// <summary>
        /// Sinh mã khách hàng mới dạng KH001, KH002, ...
        /// - Lấy mã lớn nhất hiện tại từ DB
        /// - Tăng số lên 1
        /// - Format lại thành KHxxx
        /// </summary>
        private string GenerateNewKhachhangId()
        {
            // Lấy mã khách hàng lớn nhất hiện tại từ DAL
            string lastId = _khachhangDAL.GetLatestKhachhangId();
            int nextNumber = 1;

            if (!string.IsNullOrEmpty(lastId) && lastId.StartsWith("KH") && lastId.Length >= 5)
            {
                string numberPart = lastId.Substring(2);   // VD: "010" trong "KH010"
                if (int.TryParse(numberPart, out int num))
                {
                    nextNumber = num + 1;
                }
            }

            string newId = $"KH{nextNumber:000}";          // KH001, KH002, KH010, ...
            Logger.LogDebug($"Đã tạo ID khách hàng mới: {newId}.");
            return newId;
        }

        /// <summary>
        /// Cho UI lấy mã nhân viên lập hóa đơn.
        /// </summary>
        public string GetManhanvienLapHoaDon()
        {
            return _manhanvienLapHoaDon;
        }

        /// <summary>
        /// Constructor PaymentBLL:
        /// - Được gọi từ PaymentForm khi mở màn hình thanh toán.
        /// - Nhận:
        ///     + dsChiTiet: danh sách chi tiết đơn hàng (món uống, SL, đơn giá, thành tiền)
        ///     + manhanvien: mã nhân viên lập hóa đơn
        ///     + tenNhanVien: tên nhân viên lập hóa đơn
        ///     + maHoaDon: mã hóa đơn đã được sinh từ OrderForm
        /// - Khởi tạo các DAL, lưu lại thông tin cần thiết.
        /// </summary>
        public PaymentBLL(List<Chitietdonhang> dsChiTiet,
                          string manhanvien,
                          string tenNhanVien,
                          string maHoaDon)
        {
            Logger.LogInfo("Bắt đầu khởi tạo PaymentBLL.");

            // Validate các tham số đầu vào (không cho null)
            _dsChiTietHoaDon = dsChiTiet ?? throw new ArgumentNullException(nameof(dsChiTiet), "Danh sách chi tiết hóa đơn không được null.");
            _manhanvienLapHoaDon = manhanvien ?? throw new ArgumentNullException(nameof(manhanvien), "Mã nhân viên không được null.");
            _tenNhanVienLapHoaDon = tenNhanVien ?? throw new ArgumentNullException(nameof(tenNhanVien), "Tên nhân viên không được null.");

            // Khởi tạo các lớp DAL tương tác DB
            _donhangDAL = new DonhangDAL();
            _chitietdonhangDAL = new ChitietdonhangDAL();
            _khachhangDAL = new KhachhangDAL();
            _thanhtoanDAL = new ThanhtoanDAL();

            // Mã hóa đơn được truyền từ OrderForm
            if (string.IsNullOrWhiteSpace(maHoaDon))
                throw new ArgumentException("Mã hóa đơn không được rỗng.", nameof(maHoaDon));

            _maHoaDonHienTai = maHoaDon;

            Logger.LogInfo($"PaymentBLL đã được khởi tạo. Mã hóa đơn: {_maHoaDonHienTai}, Nhân viên: {_tenNhanVienLapHoaDon} ({_manhanvienLapHoaDon}).");
            Logger.LogDebug($"Số lượng chi tiết đơn hàng ban đầu: {_dsChiTietHoaDon.Count}.");
        }

        // === Các phương thức cung cấp dữ liệu cho UI hiển thị ===

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

        /// <summary>
        /// Ngày lập hóa đơn: dùng thời điểm hiện tại.
        /// </summary>
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
        /// Tính tổng tiền hóa đơn từ danh sách chi tiết.
        /// </summary>
        public decimal CalculateTongTien()
        {
            decimal tongTien = _dsChiTietHoaDon.Sum(item => item.Thanhtien);
            Logger.LogDebug($"Đã tính tổng tiền hóa đơn: {tongTien:N0}.");
            return tongTien;
        }

        /// <summary>
        /// Sinh ID thanh toán duy nhất cho bảng Thanhtoan: TT + timestamp.
        /// </summary>
        private string GenerateUniqueThanhtoanId()
        {
            string newId = "TT" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            Logger.LogDebug($"Đã tạo ID thanh toán mới: {newId}.");
            return newId;
        }

        /// <summary>
        /// Hàm chính xử lý thanh toán (business logic):
        /// - Được gọi từ PaymentForm.XuLyThanhToan(...)
        /// - Các bước:
        ///   1) Kiểm tra có chi tiết đơn hàng hay không.
        ///   2) Xử lý khách hàng:
        ///       - Nếu có tên → tìm trong DB:
        ///           · Nếu không thấy → ném KhachhangNotFoundException (cho UI hỏi thêm mới).
        ///           · Nếu thấy → lấy Makhachhang.
        ///       - Nếu không nhập tên → xem như khách vãng lai (Makhachhang = null).
        ///   3) Tạo đối tượng Donhang (đơn hàng).
        ///   4) Tạo đối tượng Thanhtoan (thanh toán).
        ///   5) Mở transaction:
        ///       - Thêm Donhang
        ///       - Thêm các Chitietdonhang
        ///       - Thêm Thanhtoan
        ///       - Cập nhật điểm tích lũy khách hàng (nếu có)
        ///       - Commit nếu ok, Rollback nếu lỗi.
        /// </summary>
        public bool ProcessPayment(string customerPhoneNumber,
                           string hinhThucThanhToan,
                           string manhanvienThuNgan,
                           string ghiChu,
                           out Khachhang customerToUse)
        {
            Logger.LogInfo($"Bắt đầu ProcessPayment cho SĐT: '{customerPhoneNumber ?? "Khách vãng lai"}', Hình thức TT: {hinhThucThanhToan}");
            customerToUse = null;

            if (!_dsChiTietHoaDon.Any())
            {
                Logger.LogWarning("Không có đồ uống nào trong hóa đơn để thanh toán.");
                throw new InvalidOperationException("Không có đồ uống nào trong hóa đơn để thanh toán.");
            }

            decimal tongTienHoaDon = CalculateTongTien();
            string makhachhang = null;

            // === XỬ LÝ KHÁCH HÀNG THEO SỐ ĐIỆN THOẠI ===
            if (!string.IsNullOrWhiteSpace(customerPhoneNumber))
            {
                try
                {
                    customerToUse = _khachhangDAL.GetKhachhangByPhoneNumber(customerPhoneNumber);

                    if (customerToUse == null)
                    {
                        Logger.LogInfo($"Khách hàng với SĐT '{customerPhoneNumber}' không tìm thấy trong CSDL.");
                        throw new KhachhangNotFoundException($"Khách hàng với số điện thoại '{customerPhoneNumber}' chưa tồn tại.");
                    }
                    else
                    {
                        Logger.LogInfo($"Đã tìm thấy khách hàng: {customerToUse.Hoten} (Mã: {customerToUse.Makhachhang}, SĐT: {customerToUse.Sodienthoai}).");
                        makhachhang = customerToUse.Makhachhang;
                    }
                }
                catch (KhachhangNotFoundException)
                {
                    throw;
                }
                catch (Exception ex)
                {
                    Logger.LogError($"Lỗi khi tìm kiếm khách hàng theo SĐT '{customerPhoneNumber}'.", ex);
                    throw new Exception($"Lỗi khi xử lý thông tin khách hàng: {ex.Message}", ex);
                }
            }
            else
            {
                Logger.LogInfo("Không có số điện thoại khách hàng được cung cấp. Xử lý như khách vãng lai.");
            }

            // Tạo đối tượng ĐƠN HÀNG
            Donhang newDonhang = new Donhang
            {
                Madonhang = _maHoaDonHienTai,
                Manhanvien = _manhanvienLapHoaDon,
                Makhachhang = makhachhang, // có thể null
                Thoigiandat = DateTime.Now,
                Trangthaidon = "Hoàn thành",
                Tongtien = tongTienHoaDon
            };

            // Tạo đối tượng THANH TOÁN
            Thanhtoan newThanhtoan = new Thanhtoan
            {
                Mathanhtoan = GenerateUniqueThanhtoanId(), // ID thanh toán
                Madonhang = _maHoaDonHienTai,              // gắn với đơn hàng
                Thoigianthanhtoan = DateTime.Now,
                Hinhthucthanhtoan = hinhThucThanhToan,
                Sotienthanhtoan = tongTienHoaDon,
                Manhanvienthu = manhanvienThuNgan,
                Ghichu = ghiChu
            };

            // === TRANSACTION: đảm bảo lưu đồng bộ Donhang + ChiTiet + Thanhtoan ===
            using (SQLiteConnection connection = new SQLiteConnection(new BaseDataAccess().ConnectionString))
            {
                connection.Open();
                SQLiteTransaction transaction = connection.BeginTransaction();
                try
                {
                    // 1. Thêm đơn hàng
                    Logger.LogDebug($"Đang thêm đơn hàng vào CSDL: Mã='{newDonhang.Madonhang}', NV='{newDonhang.Manhanvien}', KH='{newDonhang.Makhachhang ?? "N/A"}', Tổng={newDonhang.Tongtien:N0}.");
                    _donhangDAL.AddDonhang(newDonhang, connection, transaction);
                    Logger.LogInfo($"Đã thêm đơn hàng thành công: Mã='{newDonhang.Madonhang}'.");

                    // 2. Thêm từng chi tiết đơn hàng
                    foreach (var item in _dsChiTietHoaDon)
                    {
                        item.Madonhang = _maHoaDonHienTai;
                        Logger.LogDebug($"Đang thêm chi tiết đơn hàng: HD='{item.Madonhang}', Đồ uống='{item.Madouong}', SL={item.Soluong}, TT={item.Thanhtien:N0}.");
                        _chitietdonhangDAL.AddChitietdonhang(item, connection, transaction);
                    }
                    Logger.LogInfo($"Đã thêm {_dsChiTietHoaDon.Count} chi tiết đơn hàng cho đơn hàng '{_maHoaDonHienTai}'.");

                    // 3. Thêm thông tin thanh toán
                    Logger.LogDebug($"Đang thêm thông tin thanh toán: Mã='{newThanhtoan.Mathanhtoan}', ĐH='{newThanhtoan.Madonhang}', HTTT='{newThanhtoan.Hinhthucthanhtoan}', ST={newThanhtoan.Sotienthanhtoan:N0}, NVThu='{newThanhtoan.Manhanvienthu ?? "N/A"}', Ghi chú='{newThanhtoan.Ghichu ?? ""}'.");
                    _thanhtoanDAL.AddThanhtoan(newThanhtoan, connection, transaction);
                    Logger.LogInfo($"Đã thêm thông tin thanh toán thành công: Mã='{newThanhtoan.Mathanhtoan}'.");

                    // 4. Nếu có khách hàng → cộng điểm tích lũy
                    if (customerToUse != null)
                    {
                        int diemTichLuyThem = (int)(tongTienHoaDon / 1000); // 1000 VND = 1 điểm
                        customerToUse.Diemtichluy += diemTichLuyThem;
                        _khachhangDAL.UpdateKhachhang(customerToUse, connection, transaction);
                        Logger.LogInfo($"Đã cập nhật điểm tích lũy cho khách hàng '{customerToUse.Hoten}' (Mã: {customerToUse.Makhachhang}): Thêm {diemTichLuyThem} điểm, Tổng hiện tại: {customerToUse.Diemtichluy}.");
                    }

                    // 5. Commit transaction
                    transaction.Commit();
                    Logger.LogInfo($"Giao dịch thanh toán cho đơn hàng '{_maHoaDonHienTai}' đã được COMMIT thành công.");
                    return true;
                }
                catch (Exception ex)
                {
                    // Nếu lỗi: rollback toàn bộ
                    transaction.Rollback();
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

        /// <summary>
        /// Tìm khách hàng theo tên (UI gọi khi user nhập/leave ô tên khách).
        /// </summary>
        public Khachhang GetKhachhangByName(string customerName)
        {
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
                Logger.LogError($"Lỗi khi tìm kiếm khách hàng theo tên '{customerName}'.", ex);
                throw new Exception($"Lỗi khi tìm khách hàng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm mới khách hàng với tên đơn giản (không có số điện thoại, email,...).
        /// - Được gọi khi PaymentForm hỏi: "KH chưa tồn tại, bạn có muốn thêm không?"
        /// </summary>
        public Khachhang AddNewKhachhang(string customerName, string phoneNumber)
        {
            Logger.LogInfo($"Bắt đầu thêm khách hàng mới: Tên='{customerName}', SĐT='{phoneNumber}'.");

            if (string.IsNullOrWhiteSpace(customerName))
                throw new ArgumentException("Tên khách hàng không được trống.", nameof(customerName));

            try
            {
                Khachhang newCustomer = new Khachhang
                {
                    Makhachhang = GenerateNewKhachhangId(),
                    Hoten = customerName,
                    Sodienthoai = string.IsNullOrWhiteSpace(phoneNumber) ? null : phoneNumber,
                    Ngaydangky = DateTime.Now,
                    Diemtichluy = 0
                };

                _khachhangDAL.AddKhachhang(newCustomer);
                Logger.LogInfo($"Đã thêm mới khách hàng thành công: '{newCustomer.Hoten}' (Mã: {newCustomer.Makhachhang}, SĐT: {newCustomer.Sodienthoai}).");

                return newCustomer;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Lỗi khi thêm mới khách hàng '{customerName}' với SĐT '{phoneNumber}'.", ex);
                throw new Exception($"Không thể thêm mới khách hàng: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Tìm khách hàng theo số điện thoại (UI dùng khi nhập SĐT).
        /// </summary>
        public Khachhang GetKhachhangByPhone(string phoneNumber)
        {
            Logger.LogInfo($"Bắt đầu tìm kiếm khách hàng với SĐT: '{phoneNumber}'.");
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                Logger.LogDebug("Số điện thoại rỗng khi tìm kiếm.");
                return null;
            }

            try
            {
                Khachhang foundCustomer = _khachhangDAL.GetKhachhangByPhoneNumber(phoneNumber);
                if (foundCustomer != null)
                {
                    Logger.LogInfo($"Đã tìm thấy khách hàng: '{foundCustomer.Hoten}' (Mã: {foundCustomer.Makhachhang}, SĐT: {foundCustomer.Sodienthoai}).");
                }
                else
                {
                    Logger.LogInfo($"Không tìm thấy khách hàng với SĐT: '{phoneNumber}'.");
                }
                return foundCustomer;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Lỗi khi tìm kiếm khách hàng theo SĐT '{phoneNumber}'.", ex);
                throw new Exception($"Lỗi khi tìm khách hàng: {ex.Message}", ex);
            }
        }

    }

    /// <summary>
    /// Exception tùy chỉnh để báo "không tìm thấy khách hàng":
    /// - BLL dùng exception này để báo cho UI biết cần xử lý đặc biệt
    ///   (hỏi có muốn thêm khách hàng mới hay không) thay vì tự MessageBox trong BLL.
    /// </summary>
    public class KhachhangNotFoundException : Exception
    {
        public KhachhangNotFoundException(string message) : base(message) { }
        public KhachhangNotFoundException(string message, Exception innerException) : base(message, innerException) { }
    }
}
