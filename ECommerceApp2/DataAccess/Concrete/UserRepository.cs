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
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;
        public UserRepository()
        {
            _connectionString = Program.ConnectionString;
        }
        public User Add(User item)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO Users([Username],[Password],[Email])
                            VALUES(@UUsername,@UPassword,@UEmail)
";

                var rowAffected = connection.Execute(sql, new
                {
                    UUsername = item.Username,
                    UPassword = item.Password,
                    UEmail = item.Email
                });

                if (rowAffected > 0)
                {
                    return Get($"username = {item.Username}");
                }

            }
            return null;
        }

        /// <summary>
        /// username = John
        /// age = 20
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public User Get(string filter)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var model = FilterHelper.GetFilterValues(filter);
                if (model != null)
                {
                    var sql = $@"
                SELECT * FROM Users
                WHERE {model.Property} {model.Operator} @UValue
";
                    var user = connection.QueryFirstOrDefault<User>(sql, new { UValue = model.Value });
                    return user;
                }
            }
            return null;
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public bool Remove(User item)
        {
            throw new NotImplementedException();
        }

        public User Update(User item)
        {
            throw new NotImplementedException();
        }
    }
}
