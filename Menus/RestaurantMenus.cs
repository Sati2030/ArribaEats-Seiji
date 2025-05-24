using System.ComponentModel.DataAnnotations;
using System.IO.Pipes;

namespace ArribaEats
{
    public class RestaurantMainMenu : MenuBase
    {
        Restaurant restaurant;
        Customer customer;

        public RestaurantMainMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.customer = customer;
        }

        public override MenuBase? Show()
        {
            Console.WriteLine("1: See this restaurant's menu and place an order");
            Console.WriteLine("2: See reviews for this restaurant");
            Console.WriteLine("3: Return to main menu");
            Console.WriteLine("Please enter a choice between 1 and 3:");

            int choice;

            if (!InputParser(3, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => new PlaceOrderMenu(restaurant, customer),
                2 => new SeeReviewsMenu(restaurant,customer),
                3 => new CustomerMainMenu(customer)
            };

        }
    }

    public class PlaceOrderMenu : MenuBase
    {

        public Restaurant restaurant;
        public List<Plate> menu;
        public Customer customer;

        public Order order;

        public PlaceOrderMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.menu = restaurant.Menu;
            this.customer = customer;
            this.order = new Order(restaurant, customer);
        }

        public override MenuBase? Show()
        {
            while (true)
            {
                Console.WriteLine($"Current order total: {order.Price:C2}");
                for (int i = 0; i < menu.Count; i++)
                {
                    Console.WriteLine("{0,-4}{1,6}  {2}", $"{i + 1}:", $"{menu[i].Price:C2}", $"{menu[i].Name}");
                }
                Console.WriteLine($"{menu.Count + 1}: Complete order");
                Console.WriteLine($"{menu.Count + 2}: Cancel order");
                Console.WriteLine($"Please enter a choice between 1 and {menu.Count + 2}:");

                int choice;

                if (!InputParser(menu.Count + 2, out choice))
                {
                    continue;
                }

                if (choice >= 1 && choice <= menu.Count)
                {
                    AddToOrder(menu[choice - 1]);
                }
                else if (choice == menu.Count + 1)
                {
                    order.SetStatus("Ordered");
                    OrderStore.Orders.Add(order);
                    order.ID = OrderStore.Orders.IndexOf(order)+1;
                    customer.Orders.Add(order);
                    restaurant.Orders.Add(order);

                    Console.WriteLine($"Your order has been placed. Your order number is #{order.ID}.");
                    break;

                }
                else if (choice == menu.Count + 2)
                {
                    return new RestaurantMainMenu(restaurant, customer);
                }

            }

            return new RestaurantMainMenu(restaurant, customer);

        }

        public void AddToOrder(Plate plate)
        {
            while (true)
            {
                Console.WriteLine($"Adding {plate.Name} to order.");
                Console.WriteLine("Please enter quantity (0 to cancel):");

                try
                {
                    string input = Console.ReadLine();
                    int quantity = int.Parse(input);

                    if (quantity == 0)
                    {
                        break;
                    }
                    else if (quantity < 0)
                    {
                        throw new InvalidInputException("Invalid quantity.");
                    }
                    else
                    {
                        order.AddItem(plate,quantity);
                        Console.WriteLine($"Added {quantity} x {plate.Name} to order.");
                        break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid quantity.");
                }
            }

        }


    }


    public class SeeReviewsMenu : MenuBase
    {

        public Restaurant restaurant;
        public Customer customer;

        public SeeReviewsMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.customer = customer;
        }


        public override MenuBase? Show()
        {
            if (restaurant.Rating == 0.0)
            {
                Console.WriteLine("No reviews have been left for this restaurant.");
            }
            else
            {
                foreach (Review r in restaurant.Reviews)
                {
                    string stars = "";
                    for (int i = 0; i < r.Rating; i++)
                    {
                        stars += "*";
                    }

                    Console.WriteLine($"Reviewer: {r.Customer.Name}");
                    Console.WriteLine($"Rating: {stars}");
                    Console.WriteLine($"Comment: {r.Comment}");
                    Console.WriteLine();
                }
            }

            return new RestaurantMainMenu(restaurant, customer);

        }

    }


}