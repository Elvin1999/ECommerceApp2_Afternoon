using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Entities
{
    public class Product:BaseEntity
    {
        public string Title { get; set; }
        public decimal Price { get; set; }
        public int UnitsInStock { get; set; }
    }
}
