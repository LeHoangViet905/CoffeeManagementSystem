using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeManagementSystem // Hoặc namespace của các lớp model của bạn
{
    public partial class Chitietdonhang
    {
        // Định nghĩa khóa chính kép (Composite Primary Key)
        // [Key] đánh dấu đây là một phần của khóa chính
        // [Column(Order = X)] chỉ định thứ tự của cột trong khóa chính kép
        [Key]
        [Column(Order = 0)] // Madonhang là phần đầu tiên của khóa chính kép
        public string Madonhang { get; set; }

        [Key]
        [Column(Order = 1)] // Madouong là phần thứ hai của khóa chính kép
        public string Madouong { get; set; }

        public int Soluong { get; set; }
        public decimal Dongia { get; set; }
        public decimal Thanhtien { get; set; }
        public string Ghichu { get; set; } // Đã thêm cột Ghichu nếu có trong CSDL

        // Thuộc tính điều hướng (Navigation properties)
        // Giúp liên kết các đối tượng trong C# giống như mối quan hệ trong CSDL
        [ForeignKey("Madonhang")]
        public Donhang Donhang { get; set; } // Đảm bảo lớp Donhang tồn tại và được using

        [ForeignKey("Madouong")]
        public Douong Douong { get; set; } // Đảm bảo lớp Douong tồn tại và được using

        // Thuộc tính này không được ánh xạ trực tiếp vào cột trong CSDL
        // Nó dùng để hiển thị tên đồ uống, thường được điền từ Douong.Tendouong
        [NotMapped]
        public string Tendouong { get; set; }
    }
}
