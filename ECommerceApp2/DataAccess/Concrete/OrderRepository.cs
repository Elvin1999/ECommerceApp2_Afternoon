using Dapper;
using ECommerceApp2.DataAccess.Abstractions;
using ECommerceApp2.DataAccess.Entities;
using ECommerceApp2.Services;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Concrete
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        public OrderRepository()
        {
            _connectionString = Program.ConnectionString;
        }
        public Order Add(Order item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO Orders([UserId],[ProductId],[Quantity],[OrderDate])
                            VALUES(@UId,@PId,@Q,@OD)                            
";
                connection.Execute(sql, new { UId = item.UserId, PId = item.ProductId, Q = item.Quantity, OD = item.OrderDate });
                return new Order();

            }
        }

        public IEnumerable<Order> GetMyOrders(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"
                SELECT O.Id,O.HasCompleted,O.OrderDate,O.Quantity,O.UserId,O.ProductId,
                P.Id,P.Title,P.Price,P.UnitsInStock
                FROM Orders AS O
                INNER JOIN Products AS P
                ON O.ProductId=P.Id
                WHERE O.UserId=@UId
";
                var orders = connection.Query<Order, Product, Order>(sql,
                    (order, product) =>
                    {
                        order.Product = product;
                        order.ProductId = product.Id;
                        return order;
                    }, new { UId = userId }, splitOn: "ProductId");

                return orders;
            }
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order Get(string filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var model = FilterHelper.GetFilterValues(filter);
                if (model != null)
                {
                    var sql = $@"
                SELECT * FROM Orders
                WHERE {model.Property} {model.Operator} @OValue
";
                    var order = connection.QueryFirstOrDefault<Order>(sql, new { OValue = model.Value });
                    return order;
                }
            }
            return null;
        }

        public bool Remove(Order item)
        {
            throw new NotImplementedException();
        }

        public Order Update(Order item)
        {
            throw new NotImplementedException();
        }

        public bool UpdateOrders(IEnumerable<Order> orders)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var order in orders)
                {
                    var sql = $@"
                UPDATE Orders 
                Set Quantity={order.Quantity}
                WHERE Id={order.Id}
                ";
                    sb.Append(sql);
                }

                var res = connection.Execute(sb.ToString());
                return res > 0;
            }
        }

        public bool UpdateOrderQuantity(Order order, int quantity)
        {
            using (var connection=new SqlConnection(_connectionString))
            {
                var sql = @"
            UPDATE Orders 
            SET Quantity=@OQuantity
            WHERE Id=@OId
";
                var rowAffected = connection.Execute(sql, new {OQuantity=quantity,OId=order.Id});
                return rowAffected > 0;
            }
        }
    }
}
