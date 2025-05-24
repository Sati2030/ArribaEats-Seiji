using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Channels;
using Arribaeats;

namespace ArribaEats
{
    public class ClientMainMenu : MenuBase
    {
        public Client Client { get; }

        public ClientMainMenu(Client client)
        {
            this.Client = client;
        }

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

            if (!InputParser(7, out choice))
            {
                return this;
            }

            switch (choice)
            {
                case 1:
                    return new ClientDisplayer(Client);
                case 2:
                    return new AddItemMenu(Client);
                case 3:
                    return new SeeOrdersMenu(Client);
                case 4:
                    return new CookingMenu(Client);
                case 5:
                    return new FinishCookingMenu(Client);
                case 6:
                    return new HandDeliveryMenu(Client);
                case 7:
                    Console.WriteLine("You are now logged out.");
                    return new MainMenu();
                
            }

            return choice switch
            {
                1 => new ClientDisplayer(Client),
                2 => new AddItemMenu(Client),
                3 => new SeeOrdersMenu(Client),
                4 => new CookingMenu(Client),
                5 => new FinishCookingMenu(Client),
                6 => new HandDeliveryMenu(Client),
                7 => new MainMenu()
            };


        }
    }

    public class ClientDisplayer : DisplayInformation
    {
        public ClientDisplayer(Client client) : base(client) { }

        public override MenuBase UserSpecific()
        {

            Client client = (Client)User;

            Console.WriteLine($"Restaurant name: {client.Restaurant.Name}");
            Console.WriteLine($"Restaurant style: {client.Restaurant.Type}");
            Console.WriteLine($"Restaurant location: {client.Restaurant.Location.GetLocation()}");

            return new ClientMainMenu(client);

        }


    }

    public class AddItemMenu : MenuBase
    {

        public Client client;

        public AddItemMenu(Client client)
        {
            this.client = client;
        }


        public override MenuBase? Show()
        {
            Console.WriteLine("This is your restaurant's current menu:");

            foreach (Plate p in client.Restaurant.Menu)
            {
                Console.WriteLine($"{p.Price,7:C2}  {p.Name}");
            }

            Console.WriteLine("Please enter the name of the new item (blank to cancel):");
            string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                return new ClientMainMenu(client);
            }

            while (true)
            {
                Console.WriteLine("Please enter the price of the new item (without the $):");
                string p = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(p,@"^\d+\.\d+$"))
                    {
                        throw new InvalidInputException("Invalid price.");
                    }

                    double price = double.Parse(p);
                    Plate plate = new Plate(name, price);

                    client.Restaurant.AddToMenu(plate);
                    Console.WriteLine($"Successfully added {name} ({price:C2}) to menu.");
                    return new ClientMainMenu(client);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid price.");
                }
            }

        }

    }

    public class SeeOrdersMenu : MenuBase
    {
        public Restaurant restaurant;
        public Client client;

        public SeeOrdersMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }


        public override MenuBase? Show()
        {

            if (restaurant.Orders.Count == 0)
            {
                Console.WriteLine("Your restaurant has no current orders.");
            }
            else
            {
                foreach (Order o in restaurant.Orders)
                {
                    Console.WriteLine($"Order {o.ID} for {o.Customer.Name}: {o.GetStatus()}");
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

    public class CookingMenu : MenuBase
    {

        public Client client;
        public Restaurant restaurant;

        public CookingMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }


        public override MenuBase? Show()
        {
            int maxNum = restaurant.Orders.Count;

            Console.WriteLine("Select an order once you are ready to start cooking:");
            for (int i = 0; i < maxNum; i++)
            {
                Console.WriteLine($"{i + 1}: Order #{restaurant.Orders[i].ID} for {restaurant.Orders[i].Customer.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a chioce between 1 and {maxNum + 1}");

            int choice;

            if (!InputParser(maxNum + 1, out choice))
            {
                Show();
            }

            if (!(choice == maxNum + 1))
            {
                Order o = restaurant.Orders[choice - 1];
                o.SetStatus("Cooking");
                Console.WriteLine($"Order #{o.ID} is now marked as cooking. Please prepare the order, then mark it as finished cooking:");
                foreach (OrderItem oi in o.Items)
                {
                    Console.WriteLine($"{oi.Quantity} X {oi.Plate.Name}");
                }
            }

            return new ClientMainMenu(client);


        }

    }

    public class FinishCookingMenu : MenuBase
    {
        public Client client;
        public Restaurant restaurant;

        public FinishCookingMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        public override MenuBase? Show()
        {
            int maxNum = restaurant.Orders.Count;

            Console.WriteLine("Select an order once you have finished preparing it:");
            for (int i = 0; i < maxNum; i++)
            {
                Console.WriteLine($"{i + 1}: Order #{restaurant.Orders[i].ID} for {restaurant.Orders[i].Customer.Name}");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");
            Console.WriteLine($"Please enter a choice between 1 and {maxNum + 1}");

            int choice;

            if (!InputParser(maxNum + 1, out choice))
            {
                Show();
            }

            if (!(choice == maxNum + 1))
            {
                Order o = restaurant.Orders[choice - 1];
                o.SetStatus("Cooked");
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

    public class HandDeliveryMenu : MenuBase
    {
        public Client client;
        public Restaurant restaurant;

        public HandDeliveryMenu(Client client)
        {
            this.client = client;
            this.restaurant = client.Restaurant;
        }

        public override MenuBase? Show()
        {

            Console.WriteLine("These deliverers have arrived and are waiting to collect orders.");
            Console.WriteLine("Select an order to indicate that the deliverer has collected it:");

            List<Order> assigned = restaurant.Orders.Where(o => o.ReadyToCollect).ToList();
            int maxNum = assigned.Count;

            for (int i = 0; i < maxNum; i++)
            {
                Order o = assigned[i];
                Console.WriteLine($"{i + 1}: Order #{o.ID} for {o.Customer.Name} (Deliverer licence plate: {o.Delivery.Licenceplate}) (Order status: {o.GetStatus()})");
            }
            Console.WriteLine($"{maxNum + 1}: Return to the previous menu");

            int choice;

            if (!InputParser(maxNum + 1, out choice))
            {
                Show();
            }

            if (!(choice == maxNum + 1))
            {
                Order chosen = assigned[choice - 1];
                if (chosen.GetStatus() != "Cooked")
                {
                    Console.WriteLine("This order has not yet been cooked.");
                }
                else
                {
                    chosen.ReadyToCollect = false;
                    chosen.SetStatus("Being Delivered");
                    restaurant.Orders.Remove(chosen);
                    Console.WriteLine($"Order #{chosen.ID} is now marked as being delivered.");
                }

            }

            return new ClientMainMenu(client);

        }

    }


}