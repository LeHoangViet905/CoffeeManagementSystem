using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeManagementSystem
{
    public partial class CartItem
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal BasePrice { get; set; } // Giá gốc (chưa topping)

        // Lưu danh sách các tùy chọn đã chọn (Cả tính tiền và miễn phí)
        // Key: Tên tùy chọn (VD: "50% Đường"), Value: Giá thêm (0 hoặc >0)
        public Dictionary<string, decimal> SelectedOptions { get; set; } = new Dictionary<string, decimal>();

        public string ManualNote { get; set; } = ""; // Ghi chú nhập tay

        public int Quantity { get; set; } = 1;

        // Tính tổng tiền 1 ly (Gốc + Topping)
        public decimal UnitPrice
        {
            get
            {
                decimal totalOption = 0;
                foreach (var item in SelectedOptions) totalOption += item.Value;
                return BasePrice + totalOption;
            }
        }

        public decimal TotalPrice => UnitPrice * Quantity;

        // Hàm tạo chuỗi hiển thị (để hiện lên Grid)
        public string GetDisplayText()
        {
            string display = ProductName;
            List<string> notes = new List<string>(SelectedOptions.Keys); // Lấy tên các option

            if (!string.IsNullOrEmpty(ManualNote)) notes.Add(ManualNote);

            if (notes.Count > 0)
            {
                // Xuống dòng và liệt kê: "Trà Đào \n (Ít đá, 50% đường)"
                display += Environment.NewLine + "   (" + string.Join(", ", notes) + ")";
            }
            return display;
        }
    }
}
