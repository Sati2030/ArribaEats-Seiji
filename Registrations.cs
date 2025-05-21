using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic;

namespace ArribaEats.Registrations
{
    public abstract class Registrations : MenuBase
    {
        protected User user;

        public override MenuBase? Show()
        {

            string name = NameParser();
            int age = AgeParser();
            string email = EmailParser();
            string mobile = PhoneParser();
            string password = PasswordParser();

            Registration(name,age,email,mobile,password);

            UserStore.Users.Add(user);

            return new MainMenu();
        }

        public string NameParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your name:");
                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input, @"^[A-Za-z][A-Za-z\s'-]*$"))
                    {
                        throw new InvalidInputException("Invalid name.");
                    }

                    return input;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        public int AgeParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your age (18-100)");
                string input = Console.ReadLine();

                try
                {
                    int age = int.Parse(input);
                    if (age < 18 || age > 100)
                    {
                        throw new InvalidInputException("Invalid age");
                    }

                    return age;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public string EmailParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your email address");
                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input, @"^[A-Za-z0-9.]+@[A-Za-z0-9.]+$"))
                    {
                        throw new InvalidInputException("Invalid email.");
                    }

                    foreach (User u in UserStore.Users)
                    {
                        if (u.Email == input)
                        {
                            throw new InvalidInputException("Invalid email.");
                        }
                    }

                    return input;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        public string PhoneParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your mobile phone number:");
                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input,@"^\d{10}$"))
                    {
                        throw new InvalidInputException("Invalid phone number.");
                    }

                    return input;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public string PasswordParser()
        {
            while (true)
            {
                Console.WriteLine("Your password must:");
                Console.WriteLine("- be at least 8 characters long");
                Console.WriteLine("- contain a number");
                Console.WriteLine("- contain a lowercase letter");
                Console.WriteLine("- contain an upper case letter");
                Console.WriteLine("Please enter a password");

                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$"))
                    {
                        throw new InvalidInputException("Invalid password");
                    }

                    Console.WriteLine("Please confirm your password:");

                    string second = Console.ReadLine();

                    if (second != input)
                    {
                        throw new InvalidInputException("Passwords do not match.");
                    }

                    return input;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

        }

        public Location LocationParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your location (in the form X,Y)");
                string input = Console.ReadLine();
                string[] values = input.Split(",");

                try
                {
                    if (values.Length != 2)
                    {
                        throw new InvalidInputException("Invalid location");
                    }

                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[1]);

                    return new Location(x, y);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


            }
        }

        public abstract void Registration(string name, int age, string email, string mobile, string password);

    }

}