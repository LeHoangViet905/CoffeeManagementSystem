using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace CoffeeManagementSystem // Hoặc namespace của các lớp model của bạn
{
    namespace CoffeeManagementSystem
    {
        public class OrderHistoryItem
        {
            public string Madonhang { get; set; }
            public DateTime Thoigiandat { get; set; }
            public string TenKhachhang { get; set; }
            public decimal Tongtien { get; set; }
            public string HinhThucThanhToan { get; set; }
            public string Trangthaidon { get; set; }
        }

        public class OrderDetailLine
        {
            public string Madouong { get; set; }
            public string Tendouong { get; set; }
            public int Soluong { get; set; }
            public decimal Dongia { get; set; }
            public decimal Thanhtien { get; set; }
        }
    }
}
