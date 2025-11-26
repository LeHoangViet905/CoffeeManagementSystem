// Ensure using namespace contains your Khachhang Model class
using CoffeeManagementSystem.DAL; // Reference to your DAL
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CoffeeManagementSystem.BLL
{
    public class KhachhangBLL
    {
        /// <summary>
        /// Kiểm tra email hợp lệ
        /// </summary>
        /// <param name="email"></param>
        /// <returns>true nếu hợp lệ</returns>
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
        /// Kiểm tra số điện thoại hợp lệ: chỉ chứa số, độ dài 8-12
        /// </summary>
        /// <param name="phone"></param>
        /// <returns>true nếu hợp lệ</returns>
        public bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false;

            string pattern = @"^\d{8,12}$";
            return Regex.IsMatch(phone.Trim(), pattern);
        }
        private KhachhangDAL _khachhangDAL;

        public KhachhangBLL()
        {
            _khachhangDAL = new KhachhangDAL();
        }

        /// <summary>
        /// Retrieves all customers from the database.
        /// </summary>
        /// <returns>A list of Khachhang objects.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while retrieving customers.</exception>
        public List<Khachhang> GetAllKhachhangs()
        {
            try
            {
                return _khachhangDAL.GetAllKhachhangs();
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework)
                throw new Exception("Lỗi nghiệp vụ khi lấy danh sách khách hàng: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Retrieves customer information by ID.
        /// </summary>
        /// <param name="makhachhang">The ID of the customer to retrieve.</param>
        /// <returns>A Khachhang object if found, otherwise null.</returns>
        /// <exception cref="ArgumentException">Thrown if the customer ID is invalid.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while retrieving the customer.</exception>
        public Khachhang GetKhachhangById(string makhachhang)
        {
            if (string.IsNullOrWhiteSpace(makhachhang))
            {
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(makhachhang));
            }

            try
            {
                return _khachhangDAL.GetKhachhangById(makhachhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi lấy khách hàng theo ID '{makhachhang}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches for a customer by their name (case-insensitive).
        /// </summary>
        /// <param name="tenKhachhang">The name of the customer to search for.</param>
        /// <returns>A Khachhang object if found, otherwise null.</returns>
        /// <exception cref="ArgumentException">Thrown if the customer name is invalid.</exception>
        /// <exception cref="Exception">Thrown if an error occurs while searching for the customer.</exception>
        public Khachhang GetKhachhangByName(string tenKhachhang)
        {
            if (string.IsNullOrWhiteSpace(tenKhachhang))
            {
                throw new ArgumentException("Tên khách hàng không được để trống.", nameof(tenKhachhang));
            }

            try
            {
                return _khachhangDAL.GetKhachhangByName(tenKhachhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm khách hàng theo tên '{tenKhachhang}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Adds a new customer to the database.
        /// </summary>
        /// <param name="khachhang">The Khachhang object to add.</param>
        /// <exception cref="ArgumentException">Thrown if the customer object or its properties are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if a customer with the same ID already exists.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during addition.</exception>
        public void AddKhachhang(Khachhang khachhang)
        {
            // Basic validation
            if (khachhang == null)
            {
                throw new ArgumentNullException(nameof(khachhang), "Đối tượng khách hàng không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachhang.Makhachhang))
            {
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(khachhang.Makhachhang));
            }
            if (string.IsNullOrWhiteSpace(khachhang.Hoten))
            {
                throw new ArgumentException("Họ tên khách hàng không được để trống.", nameof(khachhang.Hoten));
            }
            // Add more specific validations if needed, e.g., phone number format, email format
            // if (!string.IsNullOrEmpty(khachhang.Email) && !IsValidEmail(khachhang.Email))
            // {
            //     throw new ArgumentException("Địa chỉ email không hợp lệ.", nameof(khachhang.Email));
            // }

            try
            {
                // Check if customer with the same ID already exists (business rule)
                if (_khachhangDAL.GetKhachhangById(khachhang.Makhachhang) != null)
                {
                    throw new InvalidOperationException($"Khách hàng với mã '{khachhang.Makhachhang}' đã tồn tại.");
                }

                _khachhangDAL.AddKhachhang(khachhang);
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw specific business rule exception
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi thêm khách hàng '{khachhang.Hoten}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Updates the information of a customer.
        /// </summary>
        /// <param name="khachhang">The Khachhang object containing updated information (Makhachhang is required).</param>
        /// <exception cref="ArgumentException">Thrown if the customer object or its properties are invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the customer to update is not found.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during update.</exception>
        public void UpdateKhachhang(Khachhang khachhang)
        {
            // Basic validation
            if (khachhang == null)
            {
                throw new ArgumentNullException(nameof(khachhang), "Đối tượng khách hàng không được để trống.");
            }
            if (string.IsNullOrWhiteSpace(khachhang.Makhachhang))
            {
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(khachhang.Makhachhang));
            }
            if (string.IsNullOrWhiteSpace(khachhang.Hoten))
            {
                throw new ArgumentException("Họ tên khách hàng không được để trống.", nameof(khachhang.Hoten));
            }
            // Add more specific validations if needed

            try
            {
                // Check if customer exists before attempting to update
                if (_khachhangDAL.GetKhachhangById(khachhang.Makhachhang) == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{khachhang.Makhachhang}' để cập nhật.");
                }

                _khachhangDAL.UpdateKhachhang(khachhang);
            }
            catch (InvalidOperationException)
            {
                throw; // Re-throw specific business rule exception
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi cập nhật khách hàng '{khachhang.Hoten}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes a customer from the database.
        /// </summary>
        /// <param name="makhachhang">The ID of the customer to delete.</param>
        /// <exception cref="ArgumentException">Thrown if the customer ID is invalid.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the customer to delete is not found.</exception>
        /// <exception cref="Exception">Thrown if an unexpected error occurs during deletion.</exception>
        public void DeleteKhachhang(string makhachhang)
        {
            if (string.IsNullOrWhiteSpace(makhachhang))
            {
                throw new ArgumentException("Mã khách hàng không được để trống.", nameof(makhachhang));
            }

            try
            {
                // Optional: Check for existence before deleting, or rely on DAL to handle non-existent ID gracefully
                // if (_khachhangDAL.GetKhachhangById(makhachhang) == null)
                // {
                //     throw new InvalidOperationException($"Không tìm thấy khách hàng với mã '{makhachhang}' để xóa.");
                // }

                _khachhangDAL.DeleteKhachhang(makhachhang);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi xóa khách hàng với mã '{makhachhang}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches for customers based on a keyword in Makhachhang, Hoten, Sodienthoai, Email columns.
        /// </summary>
        /// <param name="searchTerm">The search keyword.</param>
        /// <returns>A list of matching Khachhang objects.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while searching for customers.</exception>
        public List<Khachhang> SearchKhachhangs(string searchTerm)
        {
            // No strict validation for searchTerm, as empty/whitespace will just return an empty list
            try
            {
                return _khachhangDAL.SearchKhachhangs(searchTerm);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi nghiệp vụ khi tìm kiếm khách hàng với từ khóa '{searchTerm}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Retrieves the top 10 customers with the highest accumulated points.
        /// </summary>
        /// <returns>A list of Khachhang objects sorted by DiemTichLuy in descending order.</returns>
        /// <exception cref="Exception">Thrown if an error occurs while retrieving top customers.</exception>
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

            return "KH" + nextNumber.ToString("D3"); // KH001, NV002 ...
        }

        // Optional: Example for email validation if you need it.
        // private bool IsValidEmail(string email)
        // {
        //     if (string.IsNullOrWhiteSpace(email))
        //         return false;
        //     try
        //     {
        //         // Use Regex to validate email format
        //         return Regex.IsMatch(email,
        //             @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        //             RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        //     }
        //     catch (RegexMatchTimeoutException)
        //     {
        //         return false;
        //     }
        // }
    }
}