namespace ArribaEats
{
    /// <summary>
    /// Main menu interface for a delivery driver.
    /// </summary>
    public class DeliveryMainMenu : MenuBase
    {
        /// <summary>
        /// The delivery driver using this menu.
        /// </summary>
        public Delivery Delivery { get; }

        /// <summary>
        /// Initializes a new instance of the DeliveryMainMenu class.
        /// </summary>
        /// <param name="delivery">The delivery driver using the menu.</param>
        public DeliveryMainMenu(Delivery delivery)
        {
            this.Delivery = delivery;
        }

        /// <summary>
        /// Displays the delivery main menu and handles user choice.
        /// </summary>
        /// <returns>The next menu to display, or null if exiting.</returns>
        public override MenuBase? Show()
        {
            while (true)
            {
                Console.WriteLine("Please make a choice from the menu below:");
                Console.WriteLine("1: Display your user information");
                Console.WriteLine("2: List orders available to deliver");
                Console.WriteLine("3: Arrived at restaurant to pick up order");
                Console.WriteLine("4: Mark this delivery as complete");
                Console.WriteLine("5: Log out");
                Console.WriteLine("Please enter a choice between 1 and 5:");

                int choice;

                /// Parse user menu choice
                if (!InputParser(5, out choice))
                {
                    return this;
                }

                /// Handle each menu option based on choice
                switch (choice)
                {
                    case 1: return new DeliveryDisplayer(Delivery);
                    case 2: return new AvailableOrdersMenu(Delivery);
                    case 3: return new ArrivedRestaurantMenu(Delivery);
                    case 4: return new CompletedMenu(Delivery);
                    case 5:
                        Console.WriteLine("You are now logged out.");
                        return new MainMenu();
                }
            }
        }
    }

    /// <summary>
    /// Displays detailed delivery driver information.
    /// </summary>
    public class DeliveryDisplayer : DisplayInformation
    {
        /// <summary>
        /// Initializes a new instance of the DeliveryDisplayer class.
        /// </summary>
        /// <param name="delivery">The delivery driver to display information for.</param>
        public DeliveryDisplayer(Delivery delivery) : base(delivery) { }

        /// <summary>
        /// Displays the delivery driver's details and current assignment.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase UserSpecific()
        {
            Delivery delivery = (Delivery)User;

            /// Show delivery driver info
            Console.WriteLine($"Licence plate: {delivery.Licenceplate}");

            if (delivery.OrderAssigned != null)
            {
                /// Show assigned order details if any
                Console.WriteLine("Current delivery:");
                Console.WriteLine($"Order #{delivery.OrderAssigned.ID} from {delivery.OrderAssigned.Restaurant.Name} at {delivery.OrderAssigned.Restaurant.Location.GetLocation()}.");
                Console.WriteLine($"To be delivered to {delivery.OrderAssigned.Customer.Name} at {delivery.OrderAssigned.Customer.Location!.GetLocation()}.");
            }

            return new DeliveryMainMenu(delivery);
        }
    }

    /// <summary>
    /// Menu showing available orders to accept for delivery.
    /// </summary>
    public class AvailableOrdersMenu : MenuBase
    {
        /// <summary>
        /// The delivery driver.
        /// </summary>
        public Delivery delivery;

        /// <summary>
        /// Initializes a new instance of the AvailableOrdersMenu class.
        /// </summary>
        /// <param name="delivery">The delivery driver viewing available orders.</param>
        public AvailableOrdersMenu(Delivery delivery)
        {
            this.delivery = delivery;
        }

        /// <summary>
        /// Displays the list of available orders and handles accepting one.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            if (delivery.OrderAssigned != null)
            {
                Console.WriteLine("You have already selected an order for delivery.");
            }
            else
            {
                /// Update driver's location
                delivery.Location = LocationParser.Parse();

                Console.WriteLine("The following orders are available for delivery. Select an order to accept it:");
                Console.WriteLine("   {0,-7}{1,-22}{2,-7}{3,-17}{4,-7}{5,-4}", "Order", "Restaurant Name", "Loc", "Customer Name", "Loc", "Dist");

                /// Get all available orders
                List<Order> available = OrderStore.Orders.Where(order => order.Available == true).ToList();
                int maxNum = available.Count;

                /// List each available order with details
                for (int i = 0; i < maxNum; i++)
                {
                    Restaurant r = available[i].Restaurant;
                    Customer c = available[i].Customer;

                    Console.WriteLine("{0,-3}{1,-7}{2,-22}{3,-7}{4,-17 }{5,-7}{6}",
                        $"{i + 1}:",
                        $"{available[i].ID}",
                        $"{r.Name}",
                        $"{r.Location.GetLocation()}",
                        $"{c.Name}",
                        $"{c.Location!.GetLocation()}",
                        $"{delivery.Location.CalculateDistance(r.Location) + r.Location.CalculateDistance(c.Location)}");
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
                    /// Assign the selected order to this delivery driver
                    Order chosen = available[choice - 1];
                    delivery.OrderAssigned = chosen;
                    chosen.Delivery = delivery;
                    chosen.Available = false;

                    Console.WriteLine($"Thanks for accepting the order. Please head to {chosen.Restaurant.Name} at {chosen.Restaurant.Location.GetLocation()} to pick it up.");
                }
            }

            return new DeliveryMainMenu(delivery);
        }
    }

    /// <summary>
    /// Menu for when the delivery driver arrives at the restaurant.
    /// </summary>
    public class ArrivedRestaurantMenu : MenuBase
    {
        /// <summary>
        /// The delivery driver.
        /// </summary>
        public Delivery delivery;

        /// <summary>
        /// The order assigned to the driver.
        /// </summary>
        public Order assigned;

        /// <summary>
        /// Initializes a new instance of the ArrivedRestaurantMenu class.
        /// </summary>
        /// <param name="delivery">The delivery driver arriving at the restaurant.</param>
        public ArrivedRestaurantMenu(Delivery delivery)
        {
            this.delivery = delivery;
            this.assigned = delivery.OrderAssigned!;
        }

        /// <summary>
        /// Handles the arrival process at the restaurant.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            if (assigned == null)
            {
                Console.WriteLine("You have not yet accepted an order.");
            }
            else if (assigned.GetStatus() == "Being Delivered")
            {
                Console.WriteLine("You have already picked up this order.");
            }
            else if (assigned.ReadyToCollect)
            {
                Console.WriteLine("You already indicated that you have arrived at this restaurant.");
            }
            else
            {
                /// Confirm driver arrival at the restaurant
                Console.WriteLine($"Thanks. We have informed {assigned.Restaurant.Name} that you have arrived and are ready to pick up order #{assigned.ID}.");
                Console.WriteLine("Please show the staff this screen as confirmation.");

                if (assigned.GetStatus() == "Ordered" || assigned.GetStatus() == "Cooking")
                {
                    Console.WriteLine("The order is still being prepared, so please wait patiently until it is ready.");
                }

                Console.WriteLine($"When you have the order, please deliver it to {assigned.Customer.Name} at {assigned.Customer.Location!.GetLocation()}.");

                assigned.ReadyToCollect = true;
            }

            return new DeliveryMainMenu(delivery);
        }
    }

    /// <summary>
    /// Menu for completing a delivery.
    /// </summary>
    public class CompletedMenu : MenuBase
    {
        /// <summary>
        /// The delivery driver.
        /// </summary>
        public Delivery delivery;

        /// <summary>
        /// The order assigned to the driver.
        /// </summary>
        public Order? assigned;

        /// <summary>
        /// Initializes a new instance of the CompletedMenu class.
        /// </summary>
        /// <param name="delivery">The delivery driver completing the order.</param>
        public CompletedMenu(Delivery delivery)
        {
            this.delivery = delivery;
            this.assigned = delivery.OrderAssigned;
        }

        /// <summary>
        /// Handles marking the current delivery as complete.
        /// </summary>
        /// <returns>The next menu to display.</returns>
        public override MenuBase? Show()
        {
            if (assigned == null)
            {
                Console.WriteLine("You have not yet accepted an order.");
            }
            else if (assigned.Status != OrderStatus.BeingDelivered)
            {
                Console.WriteLine("You have not yet picked up this order.");
            }
            else
            {
                /// Mark order as delivered and clear assignment
                assigned.Status = OrderStatus.Delivered;
                delivery.OrderAssigned = null;

                Console.WriteLine("Thank you for making the delivery.");
            }

            return new DeliveryMainMenu(delivery);
        }
    }
}
