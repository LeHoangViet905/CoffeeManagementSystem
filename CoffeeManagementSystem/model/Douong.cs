using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeManagementSystem
{
    public partial class Douong
    {
        [Key]
        public string Madouong { get; set; }
        public string Tendouong { get; set; }

        [ForeignKey("Loaidouong")]
        public string Maloai { get; set; }

        public string Mota { get; set; }
        public string Hinhanh { get; set; }

        public Loaidouong Loaidouong { get; set; }
        public ICollection<Giadouong> Giadouongs { get; set; }
        public ICollection<Chitietdonhang> Chitietdonhangs { get; set; }

        // Thêm thuộc tính này để lưu trữ giá hiện tại khi hiển thị
        // [NotMapped] báo hiệu rằng thuộc tính này không được ánh xạ trực tiếp vào cột CSDL
        [NotMapped]
        public decimal CurrentGia { get; set; }
    }
}
