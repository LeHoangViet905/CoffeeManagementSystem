using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeManagementSystem
{
    public class Donhang
    {
        [Key]
        public string Madonhang { get; set; }

        [ForeignKey("Nhanvien")]
        public string Manhanvien { get; set; }

        [ForeignKey("Khachhang")]
        public string Makhachhang { get; set; }

        public DateTime Thoigiandat { get; set; }
        public string Trangthaidon { get; set; }
        public decimal Tongtien { get; set; }
        public Nhanvien Nhanvien { get; set; }
        public Khachhang Khachhang { get; set; }
        public ICollection<Chitietdonhang> Chitietdonhangs { get; set; }
    }

}
