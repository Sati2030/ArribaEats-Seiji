

namespace ArribaEats
{

    /// <summary>
    /// Main menu
    /// </summary>
    public class MainMenu : MenuBase
    {
        /// <summary>
        /// Shows the main menu.
        /// </summary>
        /// <returns>Either login menu, registratino menu or exit (null).</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("Please make a choice from the menu below:");
            Console.WriteLine("1: Login as a registered user");
            Console.WriteLine("2: Register as a new user");
            Console.WriteLine("3: Exit");
            Console.WriteLine("Please enter a choice between 1 and 3:");

            if (!InputParser(3, out int choice))
            {
                Show();
            }

            return choice switch
            {
                1 => new LoginMenu(),
                2 => new RegisterMenu(),
                3 => null,
                _ => null
            };
        }
    }

    /// <summary>
    /// Main menu before registration.
    /// </summary>
    public class RegisterMenu : MenuBase
    {
        /// <summary>
        /// Show options of what user to register
        /// </summary>
        /// <returns>Menu for each users registration or main menu</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("Which type of user would you like to register as?");
            Console.WriteLine("1: Customer");
            Console.WriteLine("2: Deliverer");
            Console.WriteLine("3: Client");
            Console.WriteLine("4: Return to the previous menu");
            Console.WriteLine("Please enter a choice between 1 and 4:");

            if (!this.InputParser(4, out int choice))
            {
                Show();
            }

            return choice switch
            {
                1 => new CustomerRegistration(),
                2 => new DeliveryRegistration(),
                3 => new ClientRegistration(),
                4 => new MainMenu(),
                _ => null
            };


        }
    }

    /// <summary>
    /// Login menu.
    /// </summary>
    public class LoginMenu : MenuBase
    {
        /// <summary>
        /// Show the login prompts.
        /// </summary>
        /// <returns>User specific menu or main menu if login failed.</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("Email:");
            string? email = Console.ReadLine();
            Console.WriteLine("Password:");
            string? password = Console.ReadLine();

            ///Check if email and password match in database
            foreach (User u in UserStore.Users)
            {
                ///If successfull
                if (u.Email == email && u.Password == password)
                {

                    Console.WriteLine($"Welcome back, {u.Name}!");

                    return u.Type switch
                    {
                        "Customer" => new CustomerMainMenu((Customer)u),
                        "Delivery" => new DeliveryMainMenu((Delivery)u),
                        "Client" => new ClientMainMenu((Client)u),
                        _ => null
                    };
                }
            }

            Console.WriteLine("Invalid email or password.");

            return new MainMenu();

        }
    }
    
}