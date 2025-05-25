using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Channels;

namespace ArribaEats
{
    /// <summary>
    /// Main menu interface for a client (restaurant owner).
    /// </summary>
    public class ClientMainMenu : MenuBase
    {
        /// <summary>
        /// The client (restaurant owner) using this menu.
        /// </summary>
        public Client Client { get; }

        /// <summary>
        /// Initializes a new instance of the ClientMainMenu class.
        /// </summary>
        /// <param name="client">The client using the menu.</param>
        public ClientMainMenu(Client client)
        {
            this.Client = client;
        }

        /// <summary>
        /// Displays the client main menu and handles user choice.
        /// </summary>
        /// <returns>The next menu to display, or null if exiting.</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Display your user information");
            Console.WriteLine("2: Add item to restaurant menu");
            Console.WriteLine("3: See current orders");
            Console.WriteLine("4: Start cooking order");
            Console.WriteLine("5: Finish cooking order");
            Console.WriteLine("6: Handle deliverers who have arrived");
            Console.WriteLine("7: Log out");
            Console.WriteLine("Please enter a choice between 1 and 7:");

            int choice;

            /// Parse user menu choice
            if (!InputParser(7, out choice))
            {
                return this;
            }

            /// Handle each menu option based on choice
            switch (choice)
            {
                case 1: return new ClientDisplayer(Client);
                case 2: return new AddItemMenu(Client);
                case 3: return new SeeOrdersMenu(Client);
                case 4: return new CookingMenu(Client);
                case 5: return new FinishCookingMenu(Client);
                case 6: return new HandDeliveryMenu(Client);
                case 7:
                    Console.WriteLine("You are now logged out.");
                    return new MainMenu();
            }

            return null;
        }
    }

    /// <summary>
    /// Displays detailed client (restaurant) information.
    /// </summary>
    public class ClientDisplayer : DisplayInformation
    {
        /// <summary>
        /// Initializes a new instance of the ClientDisplayer class.
        /// </summary>
        /// <param name="client">The client to display information for.</param>
        public ClientDisplayer(Client client) : base(client) { }

        /// <summary>
        /// Displays the client-specific details.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase UserSpecific()
        {
            Client client = (Client)User;

            /// Show restaurant information
            Console.WriteLine($"Restaurant name: {client.Restaurant.Name}");
            Console.WriteLine($"Restaurant style: {client.Restaurant.GetCusine()}");
            Console.WriteLine($"Restaurant location: {client.Restaurant.Location.GetLocation()}");

            return new ClientMainMenu(client);
        }
    }

    /// <summary>
    /// Menu to add a new item to the restaurant menu.
    /// </summary>
    public class AddItemMenu : MenuBase
    {
        /// <summary>
        /// The client (restaurant owner).
        /// </summary>
        public Client client;

        /// <summary>
        /// Initializes a new instance of the AddItemMenu class.
        /// </summary>
        /// <param name="client">The client adding the item.</param>
        public AddItemMenu(Client client)
        {
            this.client = client;
        }

        /// <summary>
        /// Displays the menu for adding a new plate.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("This is your restaurant's current menu:");

            /// Print all current menu items
            foreach (Plate p in client.Restaurant.Menu.Plates)
            {
                Console.WriteLine($"{p.Price,7:C2}  {p.Name}");
            }

            /// Prompt for new item name
            Console.WriteLine("Please enter the name of the new item (blank to cancel):");
            string? name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                /// If canceled, return to main menu
                return new ClientMainMenu(client);
            }

            /// Parse and validate the price
            while (true)
            {
                Console.WriteLine("Please enter the price of the new item (without the $):");
                string? p = Console.ReadLine();

                try
                {
                    /// Check if price has valid decimal format
                    if (!Regex.IsMatch(p!, @"^\d+\.\d+$"))
                    {
                        throw new InvalidInputException("Invalid price.");
                    }

                    double price = double.Parse(p!);

                    /// Create and add new plate
                    Plate plate = new Plate(name, price);
                    /// Add plate to menu
                    client.Restaurant.Menu.AddtoMenu(plate);

                    Console.WriteLine($"Successfully added {name} ({price:C2}) to menu.");
                    return new ClientMainMenu(client);
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid price.");
                }
            }
        }
    }

    /// <summary>
    /// Menu for viewing current orders.
    /// </summary>
    public class SeeOrdersMenu : MenuBase
    {
        /// <summary>
        /// The restaurant associated with the client.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// The client viewing the orders.
        /// </summary>
        public Client client;

        /// <summary>
        /// Initializes a new instance of the SeeOrdersMenu class.
        /// </summary>
        /// <param name="client">The client viewing orders.</param>
        public SeeOrdersMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        /// <summary>
        /// Displays the list of current orders.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            /// Check if there are any orders
            if (restaurant.Orders.Count == 0)
            {
                Console.WriteLine("Your restaurant has no current orders.");
            }
            else
            {
                /// Print each order and its items
                foreach (Order o in restaurant.Orders)
                {
                    Console.WriteLine($"Order #{o.ID} for {o.Customer.Name}: {o.GetStatus()}");
                    foreach (OrderItem oi in o.Items)
                    {
                        Console.WriteLine($"{oi.Quantity} x {oi.Plate.Name}");
                    }
                    Console.WriteLine();
                }
            }

            return new ClientMainMenu(client);
        }
    }

    /// <summary>
    /// Menu for marking orders as cooking.
    /// </summary>
    public class CookingMenu : MenuBase
    {
        /// <summary>
        /// The client managing the orders.
        /// </summary>
        public Client client;

        /// <summary>
        /// The restaurant where the orders are handled.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// Initializes a new instance of the CookingMenu class.
        /// </summary>
        /// <param name="client">The client handling cooking orders.</param>
        public CookingMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        /// <summary>
        /// Displays the list of orders ready to start cooking.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            /// Get list of ordered (not yet cooking) orders
            List<Order> orders = restaurant.Orders.Where(o => o.GetStatus() == "Ordered").ToList();
            int maxNum = orders.Count;

            Console.WriteLine("Select an order once you are ready to start cooking:");
            ///Prompts for all orders
            for (int i = 0; i < maxNum; i++)
            {
                Console.WriteLine($"{i + 1}: Order #{orders[i].ID} for {orders[i].Customer.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}:");

            if (!InputParser(maxNum + 1, out int choice))
            {
                Show();
            }
            ///If not chose to go back to mani menu
            if (!(choice == maxNum + 1))
            {
                /// Mark selected order as cooking
                Order o = orders[choice - 1];
                o.Status = OrderStatus.Cooking;

                Console.WriteLine($"Order #{o.ID} is now marked as cooking. Please prepare the order, then mark it as finished cooking:");
                foreach (OrderItem oi in o.Items)
                {
                    Console.WriteLine($"{oi.Quantity} x {oi.Plate.Name}");
                }
            }

            return new ClientMainMenu(client);
        }
    }

    /// <summary>
    /// Menu for marking cooking orders as finished.
    /// </summary>
    public class FinishCookingMenu : MenuBase
    {
        /// <summary>
        /// The client managing the orders.
        /// </summary>
        public Client client;

        /// <summary>
        /// The restaurant where the orders are handled.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// Initializes a new instance of the FinishCookingMenu class.
        /// </summary>
        /// <param name="client">The client finishing cooking.</param>
        public FinishCookingMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        /// <summary>
        /// Displays the list of cooking orders ready to finish.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            /// Get list of orders currently cooking
            List<Order> orders = restaurant.Orders.Where(o => o.GetStatus() == "Cooking").ToList();
            int maxNum = orders.Count;
            ///Prompt for all orders
            Console.WriteLine("Select an order once you have finished preparing it:");
            for (int i = 0; i < maxNum; i++)
            {
                Console.WriteLine($"{i + 1}: Order #{orders[i].ID} for {orders[i].Customer.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}:");

            int choice;

            if (!InputParser(maxNum + 1, out choice))
            {
                Show();
            }

            ///If not chosen to go back to main menu
            if (!(choice == maxNum + 1))
            {
                /// Mark selected order as cooked and ready for delivery
                Order o = orders[choice - 1];
                o.Status = OrderStatus.Cooked;

                Console.WriteLine($"Order #{o.ID} is now ready for collection.");
                if (o.Delivery == null)
                {
                    Console.WriteLine("No deliverer has been assigned yet.");
                }
                else if (o.ReadyToCollect)
                {
                    Console.WriteLine($"Please take it to the deliverer with licence plate {o.Delivery.Licenceplate}, who is waiting to collect it.");
                }
                else
                {
                    Console.WriteLine($"The deliverer with licence plate {o.Delivery.Licenceplate} will be arriving soon to collect it.");
                }
            }

            return new ClientMainMenu(client);
        }
    }

    /// <summary>
    /// Menu for handling delivery pickup.
    /// </summary>
    public class HandDeliveryMenu : MenuBase
    {
        /// <summary>
        /// The client managing the orders.
        /// </summary>
        public Client client;

        /// <summary>
        /// The restaurant where the orders are handled.
        /// </summary>
        public Restaurant restaurant;

        /// <summary>
        /// Initializes a new instance of the HandDeliveryMenu class.
        /// </summary>
        /// <param name="client">The client handling delivery pickups.</param>
        public HandDeliveryMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        /// <summary>
        /// Displays the list of orders ready to be handed off to deliverers.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("These deliverers have arrived and are waiting to collect orders.");
            Console.WriteLine("Select an order to indicate that the deliverer has collected it:");

            /// Get list of ready-to-collect orders
            List<Order> assigned = restaurant.Orders.Where(o => o.ReadyToCollect).ToList();
            int maxNum = assigned.Count;

            ///Prompts for all orders ready to be handed off
            for (int i = 0; i < maxNum; i++)
            {
                Order o = assigned[i];
                Console.WriteLine($"{i + 1}: Order #{o.ID} for {o.Customer.Name} (Deliverer licence plate: {o.Delivery!.Licenceplate}) (Order status: {o.GetStatus()})");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}:");

            if (!InputParser(maxNum + 1, out int choice))
            {
                Show();
            }

            ///If not chosen to go back to main menu
            if (!(choice == maxNum + 1))
            {
                /// Mark order as handed off and remove from active list
                Order chosen = assigned[choice - 1];
                if (chosen.GetStatus() != "Cooked")
                {
                    Console.WriteLine("This order has not yet been cooked.");
                }
                else
                {
                    chosen.ReadyToCollect = false;
                    chosen.Status = OrderStatus.BeingDelivered;
                    restaurant.Orders.Remove(chosen);
                    Console.WriteLine($"Order #{chosen.ID} is now marked as being delivered.");
                }
            }

            return new ClientMainMenu(client);
        }
    }
}
