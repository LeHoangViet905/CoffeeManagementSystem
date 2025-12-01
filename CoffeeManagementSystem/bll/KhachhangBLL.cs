using CoffeeManagementSystem.DAL; // Reference to your DAL
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CoffeeManagementSystem.BLL
{
    public class KhachhangBLL
    {
        private readonly KhachhangDAL _khachhangDAL;

        public KhachhangBLL()
        {
            _khachhangDAL = new KhachhangDAL();
        }

        // ================== VALIDATION HELPER ==================

        /// <summary>
        /// Kiểm tra dữ liệu khách hàng hợp lệ (nghiệp vụ + format).
        /// </summary>
        /// <param name="khachhang">Đối tượng Khachhang.</param>
        /// <param name="isNew">
        /// true: đang thêm mới → kiểm tra trùng mã;
        /// false: đang cập nhật → không kiểm tra trùng mã.
        /// </param>
        private void ValidateKhachhang(Khachhang khachhang, bool isNew = true)
        {
            if (khachhang == null)
                throw new ArgumentNullException(nameof(khachhang), "Đối tượng khách hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(khachhang.Makhachhang))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(khachhang.Makhachhang));

            if (string.IsNullOrWhiteSpace(khachhang.Hoten))
                throw new ArgumentException("Họ tên khách hàng không được để trống.", nameof(khachhang.Hoten));

            // Email: có thì phải đúng format, không thì được null/rỗng
            if (!string.IsNullOrWhiteSpace(khachhang.Email) && !IsValidEmail(khachhang.Email))
                throw new ArgumentException("Địa chỉ email không hợp lệ.", nameof(khachhang.Email));

            // SĐT: có thì phải đúng pattern, không thì được null/rỗng
            if (!string.IsNullOrWhiteSpace(khachhang.Sodienthoai) && !IsValidPhone(khachhang.Sodienthoai))
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.", nameof(khachhang.Sodienthoai));

            // Ngày đăng ký: không để MinValue và không vượt quá ngày hiện tại
            if (khachhang.Ngaydangky == DateTime.MinValue || khachhang.Ngaydangky > DateTime.Now)
                throw new ArgumentException("Ngày đăng ký không hợp lệ.", nameof(khachhang.Ngaydangky));

            // Điểm tích lũy không âm
            if (khachhang.Diemtichluy < 0)
                throw new ArgumentException("Điểm tích lũy không được âm.", nameof(khachhang.Diemtichluy));

            // Kiểm tra trùng mã khi thêm mới
            if (isNew)
            {
                var existing = _khachhangDAL.GetKhachhangById(khachhang.Makhachhang);
                if (existing != null)
                    throw new InvalidOperationException($"Mã khách hàng '{khachhang.Makhachhang}' đã tồn tại.");
            }
        }

        /// <summary>
        /// Kiểm tra email hợp lệ.
        /// </summary>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Regex chuẩn RFC 5322 đơn giản
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email.Trim(), pattern, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra số điện thoại hợp lệ: chỉ chứa số, độ dài 10.
        /// </summary>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{10}$";
            return Regex.IsMatch(phone.Trim(), pattern);
        }

        // ================== READ / SEARCH ==================

        public List<Khachhang> GetAllKhachhangs()
        {
            try
            {
                return _khachhangDAL.GetAllKhachhangs();
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách khách hàng: " + ex.Message, ex);
            }
        }

        public Khachhang GetKhachhangById(string makhachhang)
        {
            if (string.IsNullOrWhiteSpace(makhachhang))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(makhachhang));

            try
            {
                return _khachhangDAL.GetKhachhangById(makhachhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi lấy khách hàng theo ID '{makhachhang}': {ex.Message}", ex);
            }
        }

        public Khachhang GetKhachhangByName(string tenKhachhang)
        {
            if (string.IsNullOrWhiteSpace(tenKhachhang))
                throw new ArgumentException("Tên khách hàng không được để trống.", nameof(tenKhachhang));

            try
            {
                return _khachhangDAL.GetKhachhangByName(tenKhachhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm khách hàng theo tên '{tenKhachhang}': {ex.Message}", ex);
            }
        }

        public List<Khachhang> SearchKhachhangs(string searchTerm)
        {
            try
            {
                return _khachhangDAL.SearchKhachhangs(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm kiếm khách hàng với từ khóa '{searchTerm}': {ex.Message}", ex);
            }
        }

        public List<Khachhang> GetTop10HighestDiemTichLuyCustomers()
        {
            try
            {
                return _khachhangDAL.GetTop10HighestDiemTichLuyCustomers();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi lấy TOP 10 khách hàng có điểm tích lũy cao nhất: {ex.Message}", ex);
            }
        }

        // ================== CREATE / UPDATE / DELETE ==================

        public void AddKhachhang(Khachhang khachhang)
        {
            try
            {
                // Validate đầy đủ (bao gồm check trùng mã)
                ValidateKhachhang(khachhang, isNew: true);

                _khachhangDAL.AddKhachhang(khachhang);

                // Nếu muốn cảnh báo trùng tên thì có thể làm ở UI: 
                // BLL không nên show MessageBox.
                // var sameName = _khachhangDAL.GetKhachhangByName(khachhang.Hoten);
                // if (sameName != null) { ... log warning ... }
            }
            catch (ArgumentException)
            {
                // ném lại để Form bắt và hiển thị
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi thêm khách hàng '{khachhang?.Hoten}': {ex.Message}", ex);
            }
        }

        public void UpdateKhachhang(Khachhang khachhang)
        {
            if (khachhang == null)
                throw new ArgumentNullException(nameof(khachhang), "Đối tượng khách hàng không được để trống.");

            if (string.IsNullOrWhiteSpace(khachhang.Makhachhang))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(khachhang.Makhachhang));

            try
            {
                // Kiểm tra tồn tại trước
                var existing = _khachhangDAL.GetKhachhangById(khachhang.Makhachhang);
                if (existing == null)
                    throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{khachhang.Makhachhang}' để cập nhật.");

                // Validate nhưng không check trùng mã nữa
                ValidateKhachhang(khachhang, isNew: false);

                _khachhangDAL.UpdateKhachhang(khachhang);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi cập nhật khách hàng '{khachhang.Hoten}': {ex.Message}", ex);
            }
        }

        public void DeleteKhachhang(string makhachhang)
        {
            if (string.IsNullOrWhiteSpace(makhachhang))
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(makhachhang));

            try
            {
                // Có thể kiểm tra tồn tại trước khi xóa nếu muốn
                var existing = _khachhangDAL.GetKhachhangById(makhachhang);
                if (existing == null)
                    throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{makhachhang}' để xóa.");

                // TODO: thêm check ràng buộc (nếu KH đã có hóa đơn, điểm, lịch sử…)
                _khachhangDAL.DeleteKhachhang(makhachhang);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi xóa khách hàng với mã '{makhachhang}': {ex.Message}", ex);
            }
        }

        // ================== IMPORT / SINH MÃ ==================

        public string GenerateNextMakhachhang()
        {
            List<string> allIDs = _khachhangDAL.GetAllMaKhachhang(); // Lấy tất cả mã KH
            int nextNumber = 1;

            if (allIDs.Count > 0)
            {
                List<int> numbers = new List<int>();
                foreach (var id in allIDs)
                {
                    if (id.StartsWith("KH") && int.TryParse(id.Substring(2), out int n))
                        numbers.Add(n);
                }
                numbers.Sort();
                for (int i = 1; i <= numbers.Count + 1; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        nextNumber = i;
                        break;
                    }
                }
            }

            return "KH" + nextNumber.ToString("D3"); // KH001, KH002,...
        }

        public string GenerateNextMaKHInMemory(HashSet<string> usedMa)
        {
            int max = 0;
            foreach (var ma in usedMa)
            {
                if (ma.Length <= 2) continue;
                if (int.TryParse(ma.Substring(2), out int n))
                {
                    if (n > max) max = n;
                }
            }
            return "KH" + (max + 1).ToString("D3"); // KH001, KH002,...
        }

        public void ImportKhachhangs(List<Khachhang> khachhangs)
        {
            if (khachhangs == null || khachhangs.Count == 0)
                throw new ArgumentException("Danh sách khách hàng rỗng.", nameof(khachhangs));

            _khachhangDAL.ImportKhachhangs(khachhangs);
        }

        public List<string> GetAllMaKH()
        {
            return _khachhangDAL.GetAllMaKhachhang(); // chỉ gọi 1 lần
        }
    }
}
