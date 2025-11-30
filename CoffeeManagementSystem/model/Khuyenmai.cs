using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CoffeeManagementSystem
{
    public class Khuyenmai
    {
        public string MaKM { get; set; }
        public string TenChuongTrinh { get; set; }
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public int PhanTramGiam { get; set; }
        public int TrangThai { get; set; }
        public string GhiChu { get; set; }
    }
}
