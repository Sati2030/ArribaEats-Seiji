using System.Reflection;
using System.Security;
using System.Security.Cryptography.X509Certificates;

namespace ArribaEats
{
    public class CustomerMainMenu : MenuBase
    {

        public Customer Customer { get; }

        public CustomerMainMenu(Customer customer)
        {
            this.Customer = customer;
        }

        public override MenuBase? Show()
        {
            while (true)
            {
                Console.WriteLine("Please make a choice from the menu below:");
                Console.WriteLine("1: Display your user information");
                Console.WriteLine("2: Select a list of restaurants to order from");
                Console.WriteLine("3: See the status of your orders");
                Console.WriteLine("4: Rate a restaurant you've ordered from");
                Console.WriteLine("5: Log out");
                Console.WriteLine("Please enter a choice between 1 and 5:");

                int choice;

                if (!InputParser(5, out choice))
                {
                    return null;
                }

                switch (choice)
                {
                    case 1:
                        return new CustomerDisplayer(Customer);
                    case 2:
                        return new SelectRestaurantMenu(Customer);
                    case 3:
                        return new OrderStatusMenu(Customer);
                    case 4:
                        return new RateRestaurantMenu(Customer);
                    case 5:
                        Console.WriteLine("You are now logged out.");
                        return new MainMenu();
                }
            }
        }
    }

    public class CustomerDisplayer : DisplayInformation
    {

        public CustomerDisplayer(Customer customer) : base(customer) { }
        public override MenuBase UserSpecific()
        {
            Customer customer = (Customer)User;

            Console.WriteLine($"Location: {customer.Location.GetLocation()}");
            Console.WriteLine($"You've made {customer.Orders.Count} order(s) and spent a total of {customer.Orders.Sum(o => o.Price):C2} here.");
            return new CustomerMainMenu(customer);


        }

    }

    public class SelectRestaurantMenu : MenuBase
    {

        public Customer customer;
        public List<Restaurant> restaurants;

        public SelectRestaurantMenu(Customer customer)
        {
            restaurants = RestaurantStore.Restaurants;
            this.customer = customer;
        }

        public override MenuBase? Show()
        {
            Console.WriteLine("How would you like the list of restaurants ordered?");
            Console.WriteLine("1: Sorted alphabetically by name");
            Console.WriteLine("2: Sorted by distance");
            Console.WriteLine("3: Sorted by style");
            Console.WriteLine("4: Sorted by average rating");
            Console.WriteLine("5: Return to the previous menu");
            Console.WriteLine("Please enter a choice between 1 and 5:");

            int choice;

            if (!InputParser(5, out choice))
            {
                return this;
            }

            return (choice) switch
            {
                1 => SortRestaurant(r => r.Name),
                2 => SortRestaurant(r => r.Location.CalculateDistance(customer.Location)),
                3 => SortRestaurant(r => r.TypeMapper()),
                4 => SortRestaurant(r => r.Rating),
                5 => new CustomerMainMenu(customer)
            };


        }

        public MenuBase SortRestaurant<T>(Func<Restaurant, T> selector)
            where T : IComparable<T>
        {
            List<Restaurant> sorted = restaurants.OrderBy(selector).ToList();
            int maxNum = sorted.Count + 1;

            Console.WriteLine("You can order from the following restaurants:");

            Console.WriteLine("   {0,-22}{1,-7}{2,-6}{3,-12}{4}", "Restaurant Name", "Loc", "Dist", "Style", "Rating");

            for (int i = 0; i < sorted.Count; i++)
            {
                PrintEntry(sorted[i], i + 1);
            }
            Console.WriteLine($"{maxNum}: Return to the previous menu");
            Console.WriteLine("Please enter a choice between 1 and 2:");

            int choice;

            if (!InputParser(maxNum, out choice))
            {
                return this;
            }

            if (choice == maxNum)
            {
                return new CustomerMainMenu(customer);
            }

            Console.WriteLine($"Placing order from {restaurants[choice - 1].Name}.");
            return new RestaurantMainMenu(restaurants[choice - 1], customer);


        }

        public void PrintEntry(Restaurant restaurant, int num)
        {
            string rating;

            if (restaurant.Rating == 0.0)
            {
                rating = "-";
            }
            else
            {
                rating = $"{restaurant.Rating}:F1";
            }

            Console.WriteLine("{0,-3}{1,-22}{2,-7}{3,-6}{4,-12}{5,-3}", $"{num}:", $"{restaurant.Name}", $"{restaurant.Location.GetLocation()}", $"{restaurant.Location.CalculateDistance(customer.Location)}", $"{restaurant.Type}", $"{rating}");
        }

    }

    public class OrderStatusMenu : MenuBase
    {

        public Customer customer;

        public OrderStatusMenu(Customer customer)
        {
            this.customer = customer;
        }


        public override MenuBase? Show()
        {

            if (customer.Orders.Count == 0)
            {
                Console.WriteLine("You have not placed any orders.");
            }
            else
            {
                foreach (Order o in customer.Orders)
                {
                    Console.WriteLine($"Order #{o.ID} from {o.Restaurant.Name}: {o.GetStatus()}");
                    if (o.GetStatus() == "Delivered")
                    {
                        Console.WriteLine($"This order was delivered by {o.Delivery.Name} (licence plate: {o.Delivery.Licenceplate})");
                    }
                    foreach (OrderItem oi in o.Items)
                    {
                        Console.WriteLine($"{oi.Quantity} x {oi.Plate.Name}");
                    }

                    Console.WriteLine();

                }
            }

            return new CustomerMainMenu(customer);

        }

    }


    public class RateRestaurantMenu : MenuBase
    {
        public Customer customer;

        public RateRestaurantMenu(Customer customer)
        {
            this.customer = customer;
        }

        public override MenuBase? Show()
        {
            Console.WriteLine("Select a previous order to rate the restaurant it came from:");
            int maxNum = customer.Orders.Count;
            for (int i = 0; i < maxNum; i++)
            {
                Order o = customer.Orders[i];
                Console.WriteLine($"{i + 1}: Order #{o.ID} from {o.Restaurant.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}:");

            int choice;

            if (!InputParser(maxNum + 1, out choice))
            {
                Show();
            }

            if (!(choice == maxNum + 1))
            {
                Order chosen = customer.Orders[choice - 1];
                Console.WriteLine($"You are rating order #{chosen.ID} from {chosen.Restaurant.Name}:");
                foreach (OrderItem oi in chosen.Items)
                {
                    Console.WriteLine($"{oi.Quantity} X {oi.Plate.Name}");
                }
                Console.WriteLine("Please enter a rating for this restaurant (1-5), 0 to cancel):");
                string input = Console.ReadLine();

                try
                {
                    int rating = int.Parse(input);
                    if (rating < 0 || rating > 5)
                    {
                        throw new InvalidInputException("Invalid rating.");
                    }
                    else if (rating == 0)
                    {
                        return new CustomerMainMenu(customer);
                    }
                    else
                    {
                        Console.WriteLine("Please enter a comment to accompany this rating:");
                        string comment = Console.ReadLine();
                        Review review = new Review(customer, rating, comment);
                        chosen.Restaurant.Reviews.Add(review);
                        Console.WriteLine($"Thank you for rating {chosen.Restaurant.Name}");
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid rating.");
                }

            }

            return new CustomerMainMenu(customer);

        }


    }


}