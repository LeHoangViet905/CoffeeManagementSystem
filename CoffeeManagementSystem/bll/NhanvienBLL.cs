using CoffeeManagementSystem.DAL; // Để truy cập tầng DAL
using System;
using System.Collections.Generic;
using System.Linq; // Dùng cho LINQ nếu cần trong tương lai
using System.Text.RegularExpressions;

namespace CoffeeManagementSystem.BLL
{
    /// <summary>
    /// Lớp nghiệp vụ (BLL) cho bảng Nhanvien.
    /// Chịu trách nhiệm kiểm tra dữ liệu, áp dụng rule, rồi gọi DAL.
    /// </summary>
    public class NhanvienBLL
    {
        private NhanvienDAL _nhanvienDAL;

        public NhanvienBLL()
        {
            _nhanvienDAL = new NhanvienDAL();
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của đối tượng Nhanvien trước khi thêm/cập nhật.
        /// </summary>
        /// <param name="nhanvien">Đối tượng nhân viên cần kiểm tra.</param>
        /// <param name="isNew">true nếu là thêm mới (có kiểm tra mã trùng), false nếu là cập nhật.</param>
        private void ValidateNhanvien(Nhanvien nhanvien, bool isNew = true)
        {
            if (nhanvien == null)
            {
                throw new ArgumentNullException(nameof(nhanvien), "Thông tin nhân viên không được rỗng.");
            }

            if (string.IsNullOrWhiteSpace(nhanvien.Manhanvien))
            {
                throw new ArgumentException("Mã nhân viên không được để trống.", nameof(nhanvien.Manhanvien));
            }

            if (string.IsNullOrWhiteSpace(nhanvien.Hoten))
            {
                throw new ArgumentException("Họ tên nhân viên không được để trống.", nameof(nhanvien.Hoten));
            }

            if (nhanvien.Ngaysinh == DateTime.MinValue || nhanvien.Ngaysinh > DateTime.Now)
            {
                throw new ArgumentException("Ngày sinh không hợp lệ.", nameof(nhanvien.Ngaysinh));
            }

            if (string.IsNullOrWhiteSpace(nhanvien.Gioitinh))
            {
                throw new ArgumentException("Giới tính không được để trống.", nameof(nhanvien.Gioitinh));
            }

            if (string.IsNullOrWhiteSpace(nhanvien.Diachi))
            {
                throw new ArgumentException("Địa chỉ không được để trống.", nameof(nhanvien.Diachi));
            }

            // Kiểm tra số điện thoại (ở đây yêu cầu đúng 10 chữ số)
            if (!IsValidPhone(nhanvien.Sodienthoai))
            {
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.");
            }

            // Validate Email: nếu có thì phải chứa ký tự '@'
            if (!string.IsNullOrEmpty(nhanvien.Email) && !nhanvien.Email.Contains("@"))
            {
                throw new ArgumentException("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com");
            }

            if (nhanvien.Ngayvaolam == DateTime.MinValue || nhanvien.Ngayvaolam > DateTime.Now)
            {
                throw new ArgumentException("Ngày vào làm không hợp lệ.", nameof(nhanvien.Ngayvaolam));
            }

            // Rule nghiệp vụ: khi thêm mới, mã nhân viên phải là duy nhất
            if (isNew)
            {
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(nhanvien.Manhanvien);
                if (existingNhanvien != null)
                {
                    throw new InvalidOperationException($"Mã nhân viên '{nhanvien.Manhanvien}' đã tồn tại.");
                }
            }
        }

        /// <summary>
        /// Hàm hỗ trợ kiểm tra định dạng số điện thoại (10 chữ số).
        /// </summary>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{10}$";   // bắt buộc đúng 10 số
            return Regex.IsMatch(phone.Trim(), pattern);
        }

        /// <summary>
        /// Lấy tất cả nhân viên từ CSDL (không filter).
        /// </summary>
        public List<Nhanvien> GetAllNhanviens()
        {
            try
            {
                return _nhanvienDAL.GetAllNhanviens();
            }
            catch (Exception ex)
            {
                // Có thể log ra file/log system tùy thiết kế
                throw new Exception("Lỗi BLL khi lấy danh sách nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Thêm mới một nhân viên (có validate dữ liệu và check mã trùng).
        /// </summary>
        public bool AddNhanvien(Nhanvien nhanvien)
        {
            try
            {
                // Kiểm tra dữ liệu khi thêm mới
                ValidateNhanvien(nhanvien, true);
                return _nhanvienDAL.AddNhanvien(nhanvien);
            }
            catch (ArgumentException)
            {
                // Ném lại lỗi validate để UI (Form) hiển thị chi tiết cho người dùng
                throw;
            }
            catch (InvalidOperationException)
            {
                // Ném lại lỗi nghiệp vụ (vd: mã trùng) để UI xử lý
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL khi thêm nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Cập nhật thông tin một nhân viên hiện có.
        /// </summary>
        public bool UpdateNhanvien(Nhanvien nhanvien)
        {
            try
            {
                // Kiểm tra nhân viên có tồn tại trước khi cập nhật
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(nhanvien.Manhanvien);
                if (existingNhanvien == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã '{nhanvien.Manhanvien}' để cập nhật.");
                }

                // Validate khi cập nhật (không cần check trùng mã nữa)
                ValidateNhanvien(nhanvien, false);
                return _nhanvienDAL.UpdateNhanvien(nhanvien);
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
                throw new Exception("Lỗi BLL khi cập nhật nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Xóa một nhân viên theo mã.
        /// </summary>
        public bool DeleteNhanvien(string maNhanvien)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maNhanvien))
                {
                    throw new ArgumentException("Mã nhân viên không được để trống để xóa.", nameof(maNhanvien));
                }

                // Kiểm tra nhân viên có tồn tại hay không
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(maNhanvien);
                if (existingNhanvien == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã '{maNhanvien}' để xóa.");
                }

                // TODO: thêm rule ràng buộc nếu cần (ví dụ: NV đang được phân ca, liên quan hóa đơn,…)
                // if (_calamviecDAL.GetShiftsByNhanvien(maNhanvien).Any()) { ... }

                return _nhanvienDAL.DeleteNhanvien(maNhanvien);
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
                throw new Exception("Lỗi BLL khi xóa nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Lấy thông tin một nhân viên theo mã.
        /// </summary>
        public Nhanvien GetNhanvienById(string maNhanvien)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maNhanvien))
                {
                    throw new ArgumentException("Mã nhân viên không được để trống.", nameof(maNhanvien));
                }
                return _nhanvienDAL.GetNhanvienById(maNhanvien);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL khi lấy nhân viên theo ID: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nhân viên theo từ khóa (tùy DAL xử lý theo tên/mã/SDT,...).
        /// </summary>
        public List<Nhanvien> SearchNhanviens(string keyword)
        {
            try
            {
                return _nhanvienDAL.SearchNhanviens(keyword);
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL khi tìm kiếm nhân viên: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Sinh mã nhân viên tiếp theo dạng NV001, NV002,...
        /// Dựa trên danh sách mã hiện có trong CSDL.
        /// </summary>
        public string GenerateNextMaNV()
        {
            List<string> allIDs = _nhanvienDAL.GetAllMaNV(); // Lấy tất cả mã NV
            int nextNumber = 1;

            if (allIDs.Count > 0)
            {
                List<int> numbers = new List<int>();
                foreach (var id in allIDs)
                {
                    if (id.StartsWith("NV") && int.TryParse(id.Substring(2), out int n))
                        numbers.Add(n);
                }

                numbers.Sort();

                // Tìm số nhỏ nhất bị khuyết (1..n), giống như "fill gap"
                for (int i = 1; i <= numbers.Count + 1; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        nextNumber = i;
                        break;
                    }
                }
            }

            return "NV" + nextNumber.ToString("D3"); // NV001, NV002, ...
        }

        /// <summary>
        /// Sinh mã NV tiếp theo khi chỉ làm việc trên bộ nhớ (Import, không query DB lại).
        /// </summary>
        public string GenerateNextMaNVInMemory(HashSet<string> usedMa)
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
            return "NV" + (max + 1).ToString("D3"); // NV001, NV002,...
        }

        /// <summary>
        /// Import danh sách nhân viên:
        /// - Có thể DAL sẽ xử lý insert/update tùy logic bên dưới.
        /// </summary>
        public void ImportNhanviens(List<Nhanvien> nhanviens)
        {
            if (nhanviens == null || nhanviens.Count == 0)
                throw new ArgumentException("Danh sách nhân viên rỗng.", nameof(nhanviens));

            _nhanvienDAL.ImportNhanviens(nhanviens);
        }

        /// <summary>
        /// Lấy toàn bộ mã nhân viên (Manhanvien) từ CSDL.
        /// Thường dùng để sinh mã mới hoặc validate.
        /// </summary>
        public List<string> GetAllMaNV()
        {
            return _nhanvienDAL.GetAllMaNV(); // chỉ gọi 1 lần để tái sử dụng
        }
    }
}
