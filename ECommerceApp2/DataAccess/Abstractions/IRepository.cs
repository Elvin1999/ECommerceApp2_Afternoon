using ECommerceApp2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.DataAccess.Abstractions
{
    public interface IRepository<T> where T:BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(string filter);//age > 10, Username=Elvin
        T Add(T item);
        T Update(T item);
        bool Remove(T item);
    }
}
