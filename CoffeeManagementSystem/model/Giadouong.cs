using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CoffeeManagementSystem
{
    public class Giadouong
    {
        [Key]
        public string Magia { get; set; }

        [ForeignKey("Douong")]
        public string Madouong { get; set; }

        public decimal Giaban { get; set; }
        public DateTime Thoigianapdung { get; set; }

        public Douong Douong { get; set; }
    }
}
