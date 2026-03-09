using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Entities
{
    public class Order:BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public bool HasCompleted { get; set; }
        public DateTime OrderDate { get; set; }

    }
}
