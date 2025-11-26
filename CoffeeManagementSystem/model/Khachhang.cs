using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeManagementSystem
{
    public class Khachhang
    {
        [Key]
        public string Makhachhang { get; set; }
        public string Hoten { get; set; }
        public string Sodienthoai { get; set; }
        public string Email { get; set; }
        public DateTime Ngaydangky { get; set; }
        public int Diemtichluy { get; set; }
    }
}
