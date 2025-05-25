namespace ArribaEats
{
    /// <summary>
    /// Main menu interface for interacting with a specific restaurant as a customer.
    /// </summary>
    public class RestaurantMainMenu : MenuBase
    {
        /// <summary>
        /// The restaurant being interacted with.
        /// </summary>
        private Restaurant restaurant;

        /// <summary>
        /// The customer using this menu.
        /// </summary>
        private Customer customer;

        /// <summary>
        /// Initializes a new instance of the RestaurantMainMenu class.
        /// </summary>
        /// <param name="restaurant">The selected restaurant.</param>
        /// <param name="customer">The current customer.</param>
        public RestaurantMainMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.customer = customer;
        }

        /// <summary>
        /// Displays the restaurant-specific menu and handles user choices.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("1: See this restaurant's menu and place an order");
            Console.WriteLine("2: See reviews for this restaurant");
            Console.WriteLine("3: Return to main menu");
            Console.WriteLine("Please enter a choice between 1 and 3:");

            int choice;

            /// Parse user choice
            if (!InputParser(3, out choice))
            {
                return this;
            }

            /// Navigate to appropriate submenu based on choice
            return choice switch
            {
                1 => new PlaceOrderMenu(restaurant, customer),
                2 => new SeeReviewsMenu(restaurant, customer),
                3 => new CustomerMainMenu(customer),
                _ => null
            };
        }
    }

    /// <summary>
    /// Menu for placing an order at a restaurant.
    /// </summary>
    public class PlaceOrderMenu : MenuBase
    {
        /// <summary>
        /// The restaurant being ordered from.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// The restaurant's menu.
        /// </summary>
        public Carte menu;

        /// <summary>
        /// The customer placing the order.
        /// </summary>
        public Customer customer;

        /// <summary>
        /// The current order being created.
        /// </summary>
        public Order order;

        /// <summary>
        /// Initializes a new instance of the PlaceOrderMenu class.
        /// </summary>
        /// <param name="restaurant">The selected restaurant.</param>
        /// <param name="customer">The customer placing the order.</param>
        public PlaceOrderMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.menu = restaurant.Menu;
            this.customer = customer;
            this.order = new Order(restaurant, customer);
        }

        /// <summary>
        /// Displays the order menu and handles adding items, completing, or canceling.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            while (true)
            {
                List<Plate> carte = menu.Plates;
                int numPlates = carte.Count;

                Console.WriteLine($"Current order total: {order.Price:C2}");

                /// Display all available menu items
                for (int i = 0; i < numPlates; i++)
                {
                    Console.WriteLine("{0,-4}{1,6}  {2}", $"{i + 1}:", $"{carte[i].Price:C2}", $"{carte[i].Name}");
                }
                Console.WriteLine($"{numPlates + 1}: Complete order");
                Console.WriteLine($"{numPlates + 2}: Cancel order");
                Console.WriteLine($"Please enter a choice between 1 and {carte.Count + 2}:");

                int choice;

                if (!InputParser(numPlates + 2, out choice))
                {
                    continue;
                }

                if (choice >= 1 && choice <= numPlates)
                {
                    /// Add selected item to order
                    AddToOrder(carte[choice - 1]);
                }
                else if (choice == numPlates + 1)
                {
                    /// Finalize and store the order
                    order.Status = OrderStatus.Ordered;
                    OrderStore.Orders.Add(order);
                    order.ID = OrderStore.Orders.IndexOf(order) + 1;
                    customer.Orders.Add(order);
                    restaurant.Orders.Add(order);

                    Console.WriteLine($"Your order has been placed. Your order number is #{order.ID}.");
                    break;
                }
                else if (choice == numPlates + 2)
                {
                    /// Cancel and return to main menu
                    return new RestaurantMainMenu(restaurant, customer);
                }
            }

            return new RestaurantMainMenu(restaurant, customer);
        }

        /// <summary>
        /// Adds a selected plate to the current order with user-specified quantity.
        /// </summary>
        /// <param name="plate">The plate to add.</param>
        public void AddToOrder(Plate plate)
        {
            while (true)
            {
                Console.WriteLine($"Adding {plate.Name} to order.");
                Console.WriteLine("Please enter quantity (0 to cancel):");

                try
                {
                    string? input = Console.ReadLine();
                    int quantity = int.Parse(input!);

                    if (quantity == 0)
                    {
                        /// Cancel adding item
                        break;
                    }
                    else if (quantity < 0)
                    {
                        throw new InvalidInputException("Invalid quantity.");
                    }
                    else
                    {
                        /// Add item and confirm
                        order!.AddItem(plate, quantity);
                        Console.WriteLine($"Added {quantity} x {plate.Name} to order.");
                        break;
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid quantity.");
                }
            }
        }
    }

    /// <summary>
    /// Menu for viewing reviews of a restaurant.
    /// </summary>
    public class SeeReviewsMenu : MenuBase
    {
        /// <summary>
        /// The restaurant whose reviews are shown.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// The customer viewing the reviews.
        /// </summary>
        public Customer customer;

        /// <summary>
        /// Initializes a new instance of the SeeReviewsMenu class.
        /// </summary>
        /// <param name="restaurant">The restaurant being reviewed.</param>
        /// <param name="customer">The customer viewing the reviews.</param>
        public SeeReviewsMenu(Restaurant restaurant, Customer customer)
        {
            this.restaurant = restaurant;
            this.customer = customer;
        }

        /// <summary>
        /// Displays all reviews left for the restaurant.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            if (restaurant.Rating == 0.0)
            {
                /// Handle no reviews case
                Console.WriteLine("No reviews have been left for this restaurant.");
            }
            else
            {
                /// Print each review with stars and comment
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
