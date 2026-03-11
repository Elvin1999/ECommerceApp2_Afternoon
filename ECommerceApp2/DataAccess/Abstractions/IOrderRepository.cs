using ECommerceApp2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Abstractions
{
    public interface IOrderRepository:IRepository<Order>
    {
        IEnumerable<Order> GetMyOrders(int userId);
        bool UpdateOrderQuantity(Order order,int quantity);
        bool UpdateOrders(IEnumerable<Order> orders);
    }
}
