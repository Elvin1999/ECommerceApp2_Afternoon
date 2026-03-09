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
        public static void Start()
        {
            var userRepository = new UserRepository();
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
                                if (!string.IsNullOrEmpty(password) && password.Length>=10)
                                {
                                    Console.WriteLine("Enter email : ");
                                    string email = Console.ReadLine().Trim();
                                    if (!string.IsNullOrEmpty(email))
                                    {
                                        var createdUser = new User
                                        {
                                             Email=email,
                                              Password=password,
                                               Username=username
                                        };
                                        try
                                        {

                                        var resultUser=userRepository.Add(createdUser);
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
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Please set email");
                                    }
                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine($"Please set password");
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
