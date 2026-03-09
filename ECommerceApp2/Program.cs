using ECommerceApp2.Services;

namespace ECommerceApp2
{
    public class Program
    {
        public static string ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ECommerceDB2;Trusted_Connection=True";
        static void Main(string[] args)
        {
            AppService.Start();
        }
    }
}
