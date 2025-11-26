using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace CoffeeManagementSystem
{
    public class Nhanvien
    {
        [Key]
        public string Manhanvien { get; set; }
        public string Hoten { get; set; }
        public DateTime Ngaysinh { get; set; }
        public string Gioitinh { get; set; }
        public string Diachi { get; set; }
        public string Sodienthoai { get; set; }
        public string Email { get; set; }
        public DateTime Ngayvaolam { get; set; }

        public ICollection<Taikhoan> Taikhoans { get; set; }
        public ICollection<Donhang> Donhangs { get; set; }
        public ICollection<Thanhtoan> Thanhtoans { get; set; }
    }
}
