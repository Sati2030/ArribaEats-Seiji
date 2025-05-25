using System.Text.RegularExpressions;

namespace ArribaEats
{
    /// <summary>
    /// Abstract class for registering a new user (extends MenuBase)
    /// </summary>
    public abstract class Registrations : MenuBase
    {
        /// <summary>
        /// The user that is going to be registered.
        /// </summary>
        protected User? user;

        /// <summary>
        /// Collection of basic information that all users share.
        /// </summary>
        /// <returns>To main menu when finished.</returns>
        public override MenuBase? Show()
        {

            string name = NameParser();
            int age = AgeParser();
            string email = EmailParser();
            string mobile = PhoneParser();
            string password = PasswordParser();

            ///User specific
            Registration(name, age, email, mobile, password);

            ///Add to User Storage
            UserStore.Users.Add(user!);

            return new MainMenu();
        }

        /// <summary>
        /// Parses the name of the user.
        /// </summary>
        /// <returns>Name of user.</returns>
        /// <exception cref="InvalidInputException">Thrown if input does not contain at least one letter and only letters, spaces, apostrophes and hyphens.</exception>
        public string NameParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your name:");
                string? input = Console.ReadLine();

                try
                {
                    ///Checks non-empty, only has either letters, spaces, apostrophes and hyphens
                    if (!Regex.IsMatch(input!, @"^[A-Za-z][A-Za-z\s'-]*$"))
                    {
                        throw new InvalidInputException("Invalid name.");
                    }

                    return input!;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid name.");
                }

            }
        }


        /// <summary>
        /// Parses the age of the user.
        /// </summary>
        /// <returns>Age of user.</returns>
        /// <exception cref="InvalidInputException">Thrown if under 18 or over 100.</exception>
        public int AgeParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your age (18-100):");
                string? input = Console.ReadLine();

                try
                {
                    int age = int.Parse(input!);
                    if (age < 18 || age > 100)
                    {
                        throw new InvalidInputException("Invalid age.");
                    }

                    return age;

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid age.");
                }
            }
        }

        /// <summary>
        /// Parses the email of the user.
        /// </summary>
        /// <returns>Email of the user.</returns>
        /// <exception cref="InvalidInputException">Thrown if does not include exactly one @ and have at least one other character on either side.</exception>
        public string EmailParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your email address:");
                string? input = Console.ReadLine();

                try
                {
                    ///Checks if doesn't have one letter at each side of @ and just one "@"
                    if (!Regex.IsMatch(input!, @"^[A-Za-z0-9.]+@[A-Za-z0-9.]+$"))
                    {
                        throw new InvalidInputException("Invalid email address.");
                    }

                    ///Checks if email is not already registered
                    foreach (User u in UserStore.Users)
                    {
                        if (u.Email == input)
                        {
                            throw new InvalidInputException("This email address is already in use.");
                        }
                    }
                    return input!;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        /// <summary>
        /// Parses the phone number of the user.
        /// </summary>
        /// <returns>Phone number of the user.</returns>
        /// <exception cref="InvalidInputException">Thrown if there is not 10 digits and doesn't start with 0.</exception>
        public string PhoneParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your mobile phone number:");
                string? input = Console.ReadLine();

                try
                {
                    /// Checks if number has 10 digits and starts with a 0
                    if (!Regex.IsMatch(input!, @"^0\d{9}$"))
                    {
                        throw new InvalidInputException("Invalid phone number.");
                    }

                    return input!;

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid phone number.");
                }
            }
        }


        /// <summary>
        /// Parses the password of the user.
        /// </summary>
        /// <returns>Password of the user.</returns>
        /// <exception cref="InvalidInputException">Thrown if specifications listed are not met.</exception>
        public string PasswordParser()
        {
            while (true)
            {
                Console.WriteLine("Your password must:");
                Console.WriteLine("- be at least 8 characters long");
                Console.WriteLine("- contain a number");
                Console.WriteLine("- contain a lowercase letter");
                Console.WriteLine("- contain an uppercase letter");
                Console.WriteLine("Please enter a password:");

                string? input = Console.ReadLine();

                try
                {
                    ///Checks for mentioned specifications
                    if (!Regex.IsMatch(input!, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                    {
                        throw new InvalidInputException("Invalid password.");
                    }

                    Console.WriteLine("Please confirm your password:");

                    string? second = Console.ReadLine();

                    //If passwords don't match
                    if (second != input)
                    {
                        throw new InvalidInputException("Passwords do not match.");
                    }

                    return input!;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        /// <summary>
        /// Specifc to each type of user, either client, customer or Deliverer.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="age">Age of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="mobile">Phone number of the user.</param>
        /// <param name="password">Password of the user.</param>
        public abstract void Registration(string name, int age, string email, string mobile, string password);

    }

}