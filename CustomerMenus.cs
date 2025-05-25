
namespace ArribaEats
{
    /// <summary>
    /// Main menu for customer user.
    /// </summary>
    public class CustomerMainMenu : MenuBase
    {

        /// <summary>
        /// Customer logged in
        /// </summary>
        private Customer customer;
        /// <summary>
        /// Constructor for the main menu
        /// </summary>
        /// <param name="customer">Customer logged in</param>
        public CustomerMainMenu(Customer customer)
        {
            this.customer = customer;
        }

        /// <summary>
        /// Displayer of options in customer main menu
        /// </summary>
        /// <returns>Sub-menu depending on choice</returns>
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

                if (!InputParser(5, out int choice))
                {
                    Show();
                }

                switch (choice)
                {
                    case 1:
                        return new CustomerDisplayer(customer);
                    case 2:
                        return new SelectRestaurantMenu(customer);
                    case 3:
                        return new OrderStatusMenu(customer);
                    case 4:
                        return new RateRestaurantMenu(customer);
                    case 5:
                        Console.WriteLine("You are now logged out.");
                        return new MainMenu();
                }
            }
        }
    }

    /// <summary>
    /// Displays basic information abotu customer (extends DisplayInformation).
    /// </summary>
    public class CustomerDisplayer : DisplayInformation
    {

        /// <summary>
        /// Constructor for the customer displayer
        /// </summary>
        /// <param name="customer">Customer logged in</param>
        public CustomerDisplayer(Customer customer) : base(customer) { }

        /// <summary>
        /// Displays location and how many orders have been placed and total money spent
        /// </summary>
        /// <returns>To main menu for customer</returns>
        public override MenuBase UserSpecific()
        {
            Customer customer = (Customer)User;

            Console.WriteLine($"Location: {customer.Location.GetLocation()}");
            Console.WriteLine($"You've made {customer.Orders.Count} order(s) and spent a total of {customer.Orders.Sum(o => o.Price):C2} here.");
            return new CustomerMainMenu(customer);
        }

    }

    /// <summary>
    /// Menu for selecting a restaurant to order from or see infromation on 
    /// </summary>
    public class SelectRestaurantMenu : MenuBase
    {
        /// <summary>
        /// Customer logged in
        /// </summary>
        private Customer customer;
        /// <summary>
        /// List of all restaurants (from RestaurantStore)
        /// </summary>
        public List<Restaurant> restaurants;

        /// <summary>
        /// Constructs the Restaurant selector menu
        /// </summary>
        /// <param name="customer">Customer logged in</param>
        public SelectRestaurantMenu(Customer customer)
        {
            ///Sets the restaurant to all restaurants
            restaurants = RestaurantStore.Restaurants;
            this.customer = customer;
        }

        /// <summary>
        /// Show options fo choosing how to sort the restaurant list
        /// </summary>
        /// <returns>Customer main menu</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("How would you like the list of restaurants ordered?");
            Console.WriteLine("1: Sorted alphabetically by name");
            Console.WriteLine("2: Sorted by distance");
            Console.WriteLine("3: Sorted by style");
            Console.WriteLine("4: Sorted by average rating");
            Console.WriteLine("5: Return to the previous menu");
            Console.WriteLine("Please enter a choice between 1 and 5:");

            if (!InputParser(5, out int choice))
            {
                Show();
            }

            return (choice) switch
            {
                1 => SortRestaurant(r => r.Name),
                2 => SortRestaurant(r => r.Location.CalculateDistance(customer.Location!)),
                3 => SortRestaurant(r => (int)r.Cuisine),
                4 => SortRestaurant(r => -r.Rating),
                5 => new CustomerMainMenu(customer),
                _ => null
            };
        }

        /// <summary>
        /// Sorts the list based on function selected to sort on
        /// </summary>
        /// <typeparam name="T">Dounle or int or string to sort based on</typeparam>
        /// <param name="selector">Function to sort based on</param>
        /// <returns>Selected Restaurant Main menu or Customer Main menu</returns>
        public MenuBase SortRestaurant<T>(Func<Restaurant, T> selector)
            where T : IComparable<T>
        {
            ///Make a sorted list of restaurants based on function passed
            List<Restaurant> sorted = restaurants.OrderBy(selector).ThenBy(r => r.Name).ToList();
            ///Max number that can be inputed
            int maxNum = sorted.Count + 1;

            Console.WriteLine("You can order from the following restaurants:");

            Console.WriteLine("   {0,-22}{1,-7}{2,-6}{3,-12}{4}", "Restaurant Name", "Loc", "Dist", "Style", "Rating");

            ///For each restaurant PrintEntry
            for (int i = 0; i < sorted.Count; i++)
            {
                PrintEntry(sorted[i], i + 1);
            }
            Console.WriteLine($"{maxNum}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum}:");

            ///Capture choice
            if (!InputParser(maxNum, out int choice))
            {
                return this;
            }

            if (choice == maxNum)
            {
                return new CustomerMainMenu(customer);
            }

            Console.WriteLine($"Placing order from {sorted[choice - 1].Name}.");
            return new RestaurantMainMenu(sorted[choice - 1], customer);

        }

        /// <summary>
        /// Prints a given restaurant
        /// </summary>
        /// <param name="restaurant">Restaurant to print</param>
        /// <param name="num">Number of entry in list</param>
        public void PrintEntry(Restaurant restaurant, int num)
        {
            string rating;

            if (restaurant.Rating == 0.0)
            {
                rating = "-";
            }
            else
            
            {
                rating = $"{restaurant.Rating:F1}";
            }

            Console.WriteLine("{0,-3}{1,-22}{2,-7}{3,-6}{4,-12}{5,-3}",
                num + ":",
                restaurant.Name,
                restaurant.Location.GetLocation(),
                restaurant.Location.CalculateDistance(customer.Location!),
                restaurant.GetCusine(),
                rating);
        }
    }

    /// <summary>
    /// Get the status of an order
    /// </summary>
    public class OrderStatusMenu : MenuBase
    {

        /// <summary>
        /// Logged in customer
        /// </summary>
        public Customer customer;

        /// <summary>
        /// Constructor for Status of order menu
        /// </summary>
        /// <param name="customer">Logged in customer</param>
        public OrderStatusMenu(Customer customer)
        {
            this.customer = customer;
        }

        /// <summary>
        /// Show the orders placed
        /// </summary>
        /// <returns>Customer main menu</returns>
        public override MenuBase? Show()
        {
            ///If customer has not placed an order
            if (customer.Orders.Count == 0)
            {
                Console.WriteLine("You have not placed any orders.");
            }
            else
            {
                ///For each order print order id, restaurant and status
                foreach (Order o in customer.Orders)
                {
                    Console.WriteLine($"Order #{o.ID} from {o.Restaurant.Name}: {o.GetStatus()}");
                    ///If order is already delivered print the name of deliverer and licence plate
                    if (o.GetStatus() == "Delivered")
                    {
                        Console.WriteLine($"This order was delivered by {o.Delivery!.Name} (licence plate: {o.Delivery.Licenceplate})");
                    }
                    ///Print the quantity and name of each item in order
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

    /// <summary>
    /// Menu for rating a restaurant
    /// </summary>
    public class RateRestaurantMenu : MenuBase
    {
        /// <summary>
        /// Logged in customer
        /// </summary>
        public Customer customer;
        /// <summary>
        /// Constructor of menu for rating a restuarant
        /// </summary>
        /// <param name="customer">Logged in customer</param>
        public RateRestaurantMenu(Customer customer)
        {
            this.customer = customer;
        }

        /// <summary>
        /// Shows previous orders to rate and allows rating
        /// </summary>
        /// <returns>Customer main menu</returns>
        /// <exception cref="InvalidInputException">When there is an invalid rating value (not 1-5)</exception>
        public override MenuBase? Show()
        {
            Console.WriteLine("Select a previous order to rate the restaurant it came from:");
            ///Number of orders
            int maxNum = customer.Orders.Count;
            ///Print info of each order (id and restaurant name)
            for (int i = 0; i < maxNum; i++)
            {
                Order o = customer.Orders[i];
                Console.WriteLine($"{i + 1}: Order #{o.ID} from {o.Restaurant.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}:");

            ///Parse choice
            if (!InputParser(maxNum + 1, out int choice))
            {
                Show();
            }

            ///If not chosen to return to previous menu
            if (!(choice == maxNum + 1))
            {
                //Capture chosen order
                Order chosen = customer.Orders[choice - 1];
                Console.WriteLine($"You are rating order #{chosen.ID} from {chosen.Restaurant.Name}:");
                ///Print each item in the order with quantity and name
                foreach (OrderItem oi in chosen.Items)
                {
                    Console.WriteLine($"{oi.Quantity} x {oi.Plate.Name}");
                }
                Console.WriteLine("Please enter a rating for this restaurant (1-5, 0 to cancel):");
                ///Capture rating
                string? input = Console.ReadLine();

                try
                {
                    int rating = int.Parse(input!);
                    ///If rating outside of specified range
                    if (rating < 0 || rating > 5)
                    {
                        throw new InvalidInputException("Invalid rating.");
                    }
                    ///If 0 selected then exit
                    else if (rating == 0)
                    {
                        return new CustomerMainMenu(customer);
                    }
                    ///Else asssign rating
                    else
                    {
                        Console.WriteLine("Please enter a comment to accompany this rating:");
                        string? comment = Console.ReadLine();
                        ///Creates rating
                        Review review = new Review(customer, rating, comment!);
                        ///Attaches rating to restaurant
                        chosen.Restaurant.Reviews.Add(review);
                        ///Removes order from past list
                        customer.Orders.Remove(chosen);
                        Console.WriteLine($"Thank you for rating {chosen.Restaurant.Name}.");
                    }

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid rating.");
                }

            }

            return new CustomerMainMenu(customer);

        }


    }


}