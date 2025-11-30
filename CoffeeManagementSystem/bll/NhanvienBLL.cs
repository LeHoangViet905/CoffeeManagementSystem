using CoffeeManagementSystem.DAL; // Để truy cập DAL
using System;
using System.Collections.Generic;
using System.Linq; // Cần cho các thao tác LINQ nếu có
using System.Text.RegularExpressions;

namespace CoffeeManagementSystem.BLL
{
    public class NhanvienBLL
    {
        private NhanvienDAL _nhanvienDAL;

        public NhanvienBLL()
        {
            _nhanvienDAL = new NhanvienDAL();
        }

        // Phương thức kiểm tra tính hợp lệ của dữ liệu Nhanvien
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

            // Validate Sdt (tùy chọn: có thể thêm regex cho số điện thoại)
            if (!IsValidPhone(nhanvien.Sodienthoai))
            {
                throw new ArgumentException("Số điện thoại phải gồm đúng 10 chữ số.");
            }

            // Validate Email (tùy chọn: có thể thêm regex cho email)
            if (!string.IsNullOrEmpty(nhanvien.Email) && !nhanvien.Email.Contains("@"))
            {
                throw new ArgumentException("Email không hợp lệ! Hãy nhập đúng dạng: xxx@mail.com");
            }

            if (nhanvien.Ngayvaolam == DateTime.MinValue || nhanvien.Ngayvaolam > DateTime.Now)
            {
                throw new ArgumentException("Ngày vào làm không hợp lệ.", nameof(nhanvien.Ngayvaolam));
            }

            // Logic nghiệp vụ: Mã nhân viên phải là duy nhất khi thêm mới
            if (isNew)
            {
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(nhanvien.Manhanvien);
                if (existingNhanvien != null)
                {
                    throw new InvalidOperationException($"Mã nhân viên '{nhanvien.Manhanvien}' đã tồn tại.");
                }
            }
        }

        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{10}$";   // bắt buộc 10 số
            return Regex.IsMatch(phone.Trim(), pattern);
        }
        public List<Nhanvien> GetAllNhanviens()
        {
            try
            {
                return _nhanvienDAL.GetAllNhanviens();
            }
            catch (Exception ex)
            {
                // Log lỗi (tùy chọn)
                throw new Exception("Lỗi BLL khi lấy danh sách nhân viên: " + ex.Message, ex);
            }
        }

        public bool AddNhanvien(Nhanvien nhanvien)
        {
            try
            {
                ValidateNhanvien(nhanvien, true); // Kiểm tra validation khi thêm mới
                return _nhanvienDAL.AddNhanvien(nhanvien);
            }
            catch (ArgumentException)
            {
                throw; // Ném lại lỗi validation để Form bắt
            }
            catch (InvalidOperationException)
            {
                throw; // Ném lại lỗi nghiệp vụ (mã trùng) để Form bắt
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi BLL khi thêm nhân viên: " + ex.Message, ex);
            }
        }

        public bool UpdateNhanvien(Nhanvien nhanvien)
        {
            try
            {
                // Kiểm tra xem nhân viên có tồn tại không trước khi cập nhật
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(nhanvien.Manhanvien);
                if (existingNhanvien == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã '{nhanvien.Manhanvien}' để cập nhật.");
                }

                ValidateNhanvien(nhanvien, false); // Kiểm tra validation khi cập nhật (không kiểm tra mã trùng)
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

        public bool DeleteNhanvien(string maNhanvien)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maNhanvien))
                {
                    throw new ArgumentException("Mã nhân viên không được để trống để xóa.", nameof(maNhanvien));
                }

                // Kiểm tra xem nhân viên có tồn tại không trước khi xóa
                Nhanvien existingNhanvien = _nhanvienDAL.GetNhanvienById(maNhanvien);
                if (existingNhanvien == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy nhân viên với mã '{maNhanvien}' để xóa.");
                }

                // Thêm logic kiểm tra ràng buộc nếu có (ví dụ: nhân viên này có đang quản lý ca làm việc nào không?)
                // If (_calamviecDAL.GetShiftsByNhanvien(maNhanvien).Any()) { throw new InvalidOperationException("Không thể xóa nhân viên này vì họ đang có ca làm việc được phân công."); }

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
                for (int i = 1; i <= numbers.Count + 1; i++)
                {
                    if (!numbers.Contains(i))
                    {
                        nextNumber = i;
                        break;
                    }
                }
            }

            return "NV" + nextNumber.ToString("D3"); // NV001, NV002 ...
        }
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

        public void ImportNhanviens(List<Nhanvien> nhanviens)
        {
            if (nhanviens == null || nhanviens.Count == 0)
                throw new ArgumentException("Danh sách nhân viên rỗng.", nameof(nhanviens));

            _nhanvienDAL.ImportNhanviens(nhanviens);
        }
        public List<string> GetAllMaNV()
        {
            return _nhanvienDAL.GetAllMaNV(); // chỉ gọi 1 lần
        }
    }
}