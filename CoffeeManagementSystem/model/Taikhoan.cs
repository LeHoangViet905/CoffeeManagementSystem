using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoffeeManagementSystem
{
    public class Taikhoan
    {
        [Key]
        public string Mataikhoan { get; set; }
        public string Tendangnhap { get; set; }
        public string Matkhau { get; set; }
        public string Vaitro { get; set; }

        [ForeignKey("Nhanvien")]
        public string Manhanvien { get; set; }

        public Nhanvien Nhanvien { get; set; }
    }
}
