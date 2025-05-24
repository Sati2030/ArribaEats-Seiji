using System.Security.Cryptography.X509Certificates;

namespace ArribaEats
{
    public class DeliveryMainMenu : MenuBase
    {
        public Delivery Delivery { get; }

        public DeliveryMainMenu(Delivery delivery)
        {
            this.Delivery = delivery;
        }

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

                if (!InputParser(5, out choice))
                {
                    return this;
                }

                switch (choice)
                {
                    case 1:
                        return new DeliveryDisplayer(Delivery);
                    case 2:
                        return new AvailableOrdersMenu(Delivery);
                    case 3:
                        return new ArrivedRestaurantMenu(Delivery);
                    case 4:
                        return new CompletedMenu(Delivery);
                    case 5:
                        Console.WriteLine("You are now logged out.");
                        return new MainMenu();
                        
                }

            }   
        }
    }

    public class DeliveryDisplayer : DisplayInformation
    {
        public DeliveryDisplayer(Delivery delivery) : base(delivery) { }

        public override MenuBase UserSpecific()
        {

            Delivery delivery = (Delivery)User;

            Console.WriteLine($"Licence plate: {delivery.Licenceplate}");
            if (delivery.OrderAssigned != null)
            {
                Console.WriteLine("Current delivery:");
                Console.WriteLine($"Order {delivery.OrderAssigned.ID} from {delivery.OrderAssigned.Restaurant.Name} at {delivery.OrderAssigned.Restaurant.Location.GetLocation()}");
                Console.WriteLine($"To be delivered to {delivery.OrderAssigned.Customer.Name} at {delivery.OrderAssigned.Customer.Location.GetLocation()}");

            }

            return new DeliveryMainMenu(delivery);

        }

    }

    public class AvailableOrdersMenu : MenuBase
    {

        public Delivery delivery;

        public AvailableOrdersMenu(Delivery delivery)
        {
            this.delivery = delivery;
        }

        public override MenuBase? Show()
        {

            if (delivery.OrderAssigned != null)
            {
                Console.WriteLine("You have already selected an order for delivery.");
            }
            else
            {
                delivery.Location = LocationParser.Parse();

                Console.WriteLine("The following orders are available for delivery. Select an order to accept it:");
                Console.WriteLine("   {0,-7}{1,-22}{2,-7}{3,-17}{4,-7}{5,-4}", "Order", "Restaurant Name", "Loc", "Customer Name", "Loc", "Dist");

                List<Order> available = OrderStore.Orders.Where(order => order.Available == true).ToList();
                int maxNum = available.Count;

                for (int i = 0; i < maxNum; i++)
                {
                    Restaurant r = available[i].Restaurant;
                    Customer c = available[i].Customer;

                    Console.WriteLine("{0,3}:{1,7}{2,21}{3,7}{4,16}{5,7}{6,34}",
                    $"{i + 1}", $"{i + 1}",
                    $"{r.Name}",
                    $"{r.Location.GetLocation()}",
                    $"{c.Name}",
                    $"{c.Location.GetLocation()}",
                    $"{delivery.Location.CalculateDistance(r.Location)}");
                }
                Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
                Console.WriteLine("Please enter a chioce between 1 and N:");

                int choice;

                if (!InputParser(maxNum + 1, out choice))
                {
                    Show();
                }

                if (!(choice == maxNum + 1))
                {
                    Order chosen = available[choice - 1];
                    delivery.OrderAssigned = chosen;
                    chosen.Available = false;
                    Console.WriteLine($"THanks for accepting the order. Please head to {chosen.Restaurant.Name} at {chosen.Restaurant.Location.GetLocation()} to pick it up.");
                }

            }

            return new DeliveryMainMenu(delivery);

        }

    }

    public class ArrivedRestaurantMenu : MenuBase
    {

        public Delivery delivery;
        public Order assigned;

        public ArrivedRestaurantMenu(Delivery delivery)
        {
            this.delivery = delivery;
            this.assigned = delivery.OrderAssigned;
        }

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

                Console.WriteLine($"Thanks. We have informed {assigned.Restaurant.Name} that you have arrived and are ready to pick up order #{assigned.ID}. Please show the staff this screen as confirmation.");

                if (assigned.GetStatus() == "Ordered" || assigned.GetStatus() == "Cooking")
                {
                    Console.WriteLine("The order is still being prepared, so please wait patiently until it is ready.");
                }

                Console.WriteLine($"When you have the order, please deliver it to {assigned.Customer.Name} at {assigned.Customer.Location.GetLocation()}");

                assigned.ReadyToCollect = true;

            }

            return new DeliveryMainMenu(delivery);


        }

    }


    public class CompletedMenu : MenuBase
    {
        public Delivery delivery;
        public Order? assigned;

        public CompletedMenu(Delivery delivery)
        {
            this.delivery = delivery;
            this.assigned = delivery.OrderAssigned;
        }

        public override MenuBase? Show()
        {
            if (assigned == null)
            {
                Console.WriteLine("You have not yet accepted an order.");
            }
            else if (assigned.GetStatus() != "Being Delivered")
            {
                Console.WriteLine("You have not yet picked up this order");
            }
            else
            {
                assigned.SetStatus("Delivered");
                delivery.OrderAssigned = null;
                Console.WriteLine("Thank you for making the delivery.");
            }

            return new DeliveryMainMenu(delivery);
        }

    }


}
