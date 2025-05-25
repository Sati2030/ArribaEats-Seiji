
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
            Console.WriteLine("Please enter a choice between 1 and 3:");

            int choice;

            if (!InputParser(3, out choice))
            {
                Show();
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
            Console.WriteLine("2: Deliverer");
            Console.WriteLine("3: Client");
            Console.WriteLine("4: Return to the previous menu");
            Console.WriteLine("Please enter a choice between 1 and 4:");

            int choice;

            if (!this.InputParser(4, out choice))
            {
                Show();
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

                    Console.WriteLine($"Welcome back, {u.Name}!");

                    return u.Type switch
                    {
                        "Customer" => new CustomerMainMenu((Customer)u),
                        "Delivery" => new DeliveryMainMenu((Delivery)u),
                        "Client" => new ClientMainMenu((Client)u)
                    };
                }
            }

            Console.WriteLine("Invalid email or password.");

            return new MainMenu();

        }
    }
    
}