
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.Swift;
using System.Xml.Serialization;
using Arribaeats;

namespace ArribaEats
{

    public class MainMenu : MenuBase
    {
        public override MenuBase? Show()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Login as a registered user");
            Console.WriteLine("2: Register as a new user");
            Console.WriteLine("3: Exit");
            Console.WriteLine("Please enter a chioce between 1 and 3:");

            string input = Console.ReadLine();
            int choice;

            if (!InputParser(input, 3, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => new LoginMenu(),
                2 => new RegisterMenu(),
                3 => null
            };

        }
    }

    public class RegisterMenu : MenuBase
    {
        public override MenuBase? Show()
        {
            Console.WriteLine("Which type of user would you like to register as?");
            Console.WriteLine("1: Customer");
            Console.WriteLine("2: Deliver");
            Console.WriteLine("3: Client");
            Console.WriteLine("4: Return to the previous Menu");
            Console.WriteLine("Please enter a choice between 1 and 4");

            string input = Console.ReadLine();
            int choice;


            if (!this.InputParser(input, 4, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => new CustomerRegistration(),
                2 => new DeliveryRegistration(),
                3 => new ClientRegistration(),
                4 => new MainMenu()
            };


        }
    }

    public class LoginMenu : MenuBase
    {
        public override MenuBase? Show()
        {
            Console.WriteLine("Email:");
            string email = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();

            foreach (User u in UserStore.Users)
            {
                if (u.Email == email && u.Password == password)
                {
                    return u.Type switch
                    {
                        "Customer" => new CustomerMainMenu((Customer)u),
                        "Delivery" => new DeliveryMainMenu(),
                        "Client" => new ClientMainMenu()
                    };
                }
            }

            return new MainMenu();

        }
    }

    public class CustomerMainMenu : MenuBase
    {

        public Customer Customer { get; }

        public CustomerMainMenu(Customer customer)
        {
            this.Customer = customer;
        }

        public override MenuBase? Show()
        {

            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Display your user information");
            Console.WriteLine("2: Select a list of restaruants to order from");
            Console.WriteLine("3: See the status of your orders");
            Console.WriteLine("4: Rate a restaurant you've ordered from");
            Console.WriteLine("5: Log out");
            Console.WriteLine("Please enter a choice betwen 1 and 5:");

            string input = Console.ReadLine();
            int choice;

            if (!InputParser(input, 5, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => new DisplayInformation(customer),
                2 => new SelectRestaurant(),
                3 => new OrderStatus(),
                4 => new RateRestaurant(),
                5 => new MainMenu()
            };


        }
    }
    public class DeliveryMainMenu : MenuBase
    {
        public Delivery Delivery { get; }

        public DeliveryMainMenu(Delivery delivery)
        {
            this.Delivery = delivery;
        }

        public override MenuBase? Show()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Display your user information");
            Console.WriteLine("2: List orders availlable to deliver");
            Console.WriteLine("3: Arrived at restaurant to pick up order");
            Console.WriteLine("4: Mark this delivery as complete");
            Console.WriteLine("5: Log out");

            string input = Console.ReadLine();
            int choice;

            if (!InputParser(input, 5, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => new DisplayInformation(customer),
                2 => new AvailableOrders(),
                3 => new Arrived(),
                4 => new Completed(),
                5 => new MainMenu()
            };

        }
    }
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

            string input = Console.ReadLine();
            int choice;

            if (!InputParser(string, 7, out choice))
            {
                return this;
            }

            return choice switch
            {
                1 => DisplayInformation(client),
                2 => AddItem(),
                3 => SeeOrders(),
                4 => Cooking(),
                5 => FinishCooking(),
                6 => HandleDelivery(),
                7 => new MainMenu()
            };


        }
    }


}