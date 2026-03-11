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
        static UserRepository userRepository = new UserRepository();
        static ProductRepository productRepository = new ProductRepository();
        static OrderRepository orderRepository = new OrderRepository();
        static List<Order> orders = new List<Order>();
        public static void Start()
        {
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
                                            orders = orderRepository.GetMyOrders(user.Id).ToList();
                                            while (true)
                                            {
                                                ShowAllProducts(products);
                                                Console.WriteLine("Click [0] to go CART");
                                                Console.WriteLine("Click [No] to add CART");

                                                int noOrZero = int.Parse(Console.ReadLine());

                                                if (noOrZero == 0)
                                                {
                                                    ShowCart(orders);
                                                    var key = Console.ReadKey().Key;
                                                    if (key == ConsoleKey.Escape)
                                                    {
                                                        continue;
                                                    }
                                                }
                                                else if (noOrZero >= 1 && noOrZero <= products.Count())
                                                {
                                                    var prodId = products[noOrZero - 1]?.Id;
                                                    var order = new Order
                                                    {
                                                        ProductId = prodId ?? -1,
                                                        UserId = user.Id,
                                                        Quantity = 1,
                                                        OrderDate = DateTime.Now,
                                                        HasCompleted = false
                                                    };

                                                    var orderOld = orders.FirstOrDefault(o => o.UserId == user.Id && o.ProductId == prodId);
                                                    if (orderOld != null)
                                                    {
                                                        orderRepository.UpdateOrderQuantity(orderOld, orderOld.Quantity + 1);
                                                        Console.WriteLine("Update order quantity successfully");
                                                    }
                                                    else
                                                    {
                                                        orderRepository.Add(order);
                                                        Console.WriteLine("Added to cart successfully");
                                                    }
                                                    Thread.Sleep(2000);
                                                    orders = orderRepository.GetMyOrders(user.Id).Where(o => !o.HasCompleted).ToList();
                                                    ShowCart(orders);
                                                    Console.WriteLine("Click any key to continue");
                                                    Console.ReadKey();

                                                }

                                                ///////////////////

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

        private static void ShowCart(List<Order> orders)
        {
            Console.Clear();
            Console.WriteLine("Your Cart");
            if (!orders.Any())
            {
                Console.WriteLine("No item exist in your cart");
            }
            else
            {
                ShowAllOrders(orders);
                var total = orders.Sum(o => o.Product.Price * o.Quantity);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\t\t\t\t\tTotal : {total}$");
                Console.ResetColor();
                EditCart(orders.ToList());
                Console.WriteLine("Click ESC to go back");
            }
        }
        public static Order CurrentOrder = null;
        public static int index = 1;
        private static void EditCart(List<Order> orders)
        {
            CurrentOrder = orders[0];
            ShowCartOrderInEdit(orders);

            while (true)
            {


            Console.WriteLine("Select [NO] to Edit item or click [0] to go back or click [-1] to save changes");
            var key = Console.ReadKey().Key;
            if (key == ConsoleKey.UpArrow)
            {
                if (index - 1 >= 1)
                {
                    --index;
                    CurrentOrder = orders[index-1];
                    ShowCartOrderInEdit(orders);
                }
            }
            else if (key == ConsoleKey.DownArrow)
            {
                if (index + 1 <= orders.Count())
                {
                    ++index;
                    CurrentOrder = orders[index - 1];
                    ShowCartOrderInEdit(orders);
                }
            }
            else if (key == ConsoleKey.Enter)
            {
                //Update save changes
            }
            else if (key == ConsoleKey.Escape)
            {
                return;
            }
                //else
                //{
                //    ShowInfo("Select correct no !");
                //}
            }

        }

        private static void ShowCartOrderInEdit(List<Order> orders)
        {
            Console.Clear();
            Console.WriteLine("Your Cart");
            Console.WriteLine();
            int no = 0;
            foreach (var order in orders)
            {
                if (CurrentOrder != null && order.Id == CurrentOrder.Id)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                }
                else
                {
                    Console.ResetColor();
                }
                var price = order?.Product?.Price;
                Console.WriteLine($"NO : [{++no}]");
                Console.WriteLine($"Product : {order?.Product?.Title} ({order?.Quantity} items) - {price}$  Total : {price * order?.Quantity}");
                Console.WriteLine($"ORDER DATE : {order?.OrderDate.ToLongDateString()} - {order?.OrderDate.ToShortTimeString()}");
                Console.WriteLine();

            }

            var total = orders.Sum(o => o.Product.Price * o.Quantity);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\t\t\t\t\tTotal : {total}$");
            Console.ResetColor();

            //var key = Console.ReadKey().Key;
            //if (key == ConsoleKey.Escape)
            //{
            //    CurrentOrder = null;
            //    EditCart(orders);
            //    return;
            //}
            //else if (key == ConsoleKey.UpArrow)
            //{
            //    if (CurrentOrder?.Quantity + 1 <= CurrentOrder?.Product.UnitsInStock)
            //    {
            //        CurrentOrder.Quantity++;
            //    }
            //    ShowCartOrderInEdit(orders);
            //}
            //else if (key == ConsoleKey.DownArrow)
            //{
            //    if (CurrentOrder?.Quantity - 1 >= 1)
            //    {
            //        CurrentOrder.Quantity--;
            //    }
            //    ShowCartOrderInEdit(orders);
            //}
            return;

        }

        private static void ShowAllOrders(List<Order> orders)
        {
            Console.WriteLine();
            int no = 0;
            foreach (var order in orders)
            {
                var price = order?.Product?.Price;
                Console.WriteLine($"NO : [{++no}]");
                Console.WriteLine($"Product : {order?.Product?.Title} ({order?.Quantity} items) - {price}$  Total : {price * order?.Quantity}");
                Console.WriteLine($"ORDER DATE : {order?.OrderDate.ToLongDateString()} - {order?.OrderDate.ToShortTimeString()}");
                Console.WriteLine();
            }
        }
    }
}
