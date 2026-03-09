using ECommerceApp2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Abstractions
{
    public interface IProductRepository:IRepository<Product>
    {
    }
}
