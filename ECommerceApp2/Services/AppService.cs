using ECommerceApp2.DataAccess.Concrete;
using ECommerceApp2.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp2.Services
{
    public class AppService
    {
        static void ShowInfo(string info)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(info);
        }

        static void ShowProductInCart(Product currentItem)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine($"Title {currentItem.Title}");
            Console.WriteLine($"Price {currentItem.Price}$");
        }

        private static void ShowCurrentProduct(Product currentProduct)
        {
            Console.WriteLine("Your Cart");
            //ordersden goturmek lazimdi// birinci kohne kartda olanlari gostersin sonra cari
            //
            ShowProductInCart(currentProduct);
            Console.WriteLine("Quantity : 1");

        }

        static void ShowAllProducts(IEnumerable<Product> products)
        {
            Console.Clear();
            int index = 0;
            foreach (var p in products)
            {
                Console.WriteLine($"No : {++index}");
                Console.WriteLine($"Title : {p.Title}");
                Console.WriteLine($"Price : {p.Price}$");
                Console.WriteLine();
            }
        }
        public static void Start()
        {
            var userRepository = new UserRepository();
            var productRepository = new ProductRepository();
            while (true)
            {
                Console.ResetColor();
                Console.WriteLine("ECommerce App");
                Console.WriteLine("\n\tSign IN [1]");
                Console.WriteLine("\n\tSign UP [2]");
                int select = int.Parse(Console.ReadLine());
                switch (select)
                {
                    case 1:
                        {
                            Console.WriteLine("Enter username : ");
                            string username = Console.ReadLine().Trim();
                            var user = userRepository.Get($"username = {username}");
                            if (user == null)
                            {
                                ShowInfo($"{username} does not exist");
                            }
                            else
                            {
                                Console.WriteLine("Enter password : ");
                                string password = Console.ReadLine();
                                if (!string.IsNullOrEmpty(password) && password.Length >= 10)
                                {
                                    if (user.Password == password)
                                    {
                                        Console.Clear();
                                        Console.WriteLine($"Welcome {username} to our ECommerceApp");
                                        var products = productRepository.GetAll().ToList();
                                        if (products.Any())
                                        {
                                            while (true)
                                            {
                                                ShowAllProducts(products);
                                                Console.WriteLine("Select product NO to add CART");
                                                int no = int.Parse(Console.ReadLine()) - 1;
                                                if (no >= 0 && no <= (products.Count()))
                                                {
                                                    var currentProduct = products[no];
                                                    ShowCurrentProduct(currentProduct);
                                                    var key = Console.ReadKey();
                                                    if (key.Key == ConsoleKey.Escape)
                                                    {
                                                        Console.Clear();
                                                        continue;
                                                    }
                                                }
                                                else
                                                {
                                                    ShowInfo($"Range should be between 0 and {products.Count()}");
                                                    Thread.Sleep(1000);
                                                    Console.ResetColor();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("No items exist in our stock");
                                        }

                                    }
                                    else
                                    {
                                        ShowInfo($"Password is wrong!");
                                    }
                                }
                            }
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.WriteLine("In Registration");
                            Console.WriteLine("Enter username : ");
                            string username = Console.ReadLine().Trim();
                            var user = userRepository.Get($"Username = {username}");
                            if (user != null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine($"{username} is already exist");
                                Thread.Sleep(500);
                            }
                            else
                            {
                                Console.WriteLine("Enter Password : ");
                                string password = Console.ReadLine().Trim();
                                if (!string.IsNullOrEmpty(password) && password.Length >= 10)
                                {
                                    Console.WriteLine("Enter email : ");
                                    string email = Console.ReadLine().Trim();
                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        var createdUser = new User
                                        {
                                            Email = email,
                                            Password = password,
                                            Username = username
                                        };
                                        try
                                        {

                                            var resultUser = userRepository.Add(createdUser);
                                            if (resultUser != null)
                                            {
                                                Console.ForegroundColor = ConsoleColor.Green;
                                                Console.WriteLine("Registration completed successfully");
                                                Console.WriteLine("Click any key to continue");
                                                Console.ReadKey();
                                            }
                                            else
                                            {
                                                Console.WriteLine("Something went wrong");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }
                                    else
                                    {
                                        ShowInfo($"Please set email");
                                    }
                                }
                                else
                                {
                                    ShowInfo($"Please set password");
                                }
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
        }


    }
}
