using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoffeeManagementSystem
{
    public class Thanhtoan
    {
        [Key]
        public string Mathanhtoan { get; set; }

        [ForeignKey("Donhang")]
        public string Madonhang { get; set; }

        public DateTime Thoigianthanhtoan { get; set; }
        public string Hinhthucthanhtoan { get; set; }
        public decimal Sotienthanhtoan { get; set; }

        [ForeignKey("Nhanvien")]
        public string Manhanvienthu { get; set; }

        public string Ghichu { get; set; }

        public Donhang Donhang { get; set; }
        public Nhanvien Nhanvien { get; set; }
    }
}
