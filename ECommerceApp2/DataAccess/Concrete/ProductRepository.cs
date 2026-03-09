using Dapper;
using ECommerceApp2.DataAccess.Abstractions;
using ECommerceApp2.DataAccess.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Concrete
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;
        public ProductRepository()
        {
            _connectionString = Program.ConnectionString;
        }
        public Product Add(Product item)
        {
            throw new NotImplementedException();
        }

        public Product Get(string filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            using (var connection=new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Products";
                var products = connection.Query<Product>(sql);
                return products.ToList();
            }
        }

        public bool Remove(Product item)
        {
            throw new NotImplementedException();
        }

        public Product Update(Product item)
        {
            throw new NotImplementedException();
        }
    }
}
