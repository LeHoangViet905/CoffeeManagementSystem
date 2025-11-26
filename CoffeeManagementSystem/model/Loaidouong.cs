using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace CoffeeManagementSystem
{
    public class Loaidouong
    {
        [Key]
        public string Maloai { get; set; }
        public string Tenloai { get; set; }
        public ICollection<Douong> Douongs { get; set; }
    }

}
