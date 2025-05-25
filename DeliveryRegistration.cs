using System.Text.RegularExpressions;

namespace ArribaEats
{
    /// <summary>
    /// Registration for delivery driver (extends Registrations).
    /// </summary>
    public class DeliveryRegistration : Registrations
    {

        /// <summary>
        /// Delivery driver specific registration
        /// </summary>
        /// <param name="name">Name of the driver.</param>
        /// <param name="age">Age of the driver.</param>
        /// <param name="email">Email of the driver.</param>
        /// <param name="mobile">Phone number of the driver.</param>
        /// <param name="password">Password of the driver.</param>
        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            string licence = LicenceParser();
            
            Delivery delivery = new Delivery(name, age, email, mobile, password, "Delivery",licence);

            user = delivery;

            Console.WriteLine($"You have been successfully registered as a deliverer, {name}!");
            return;
        }

        /// <summary>
        /// Parse licence plate number of the vehicle deliverer uses.
        /// </summary>
        /// <returns>Licence place.</returns>
        /// <exception cref="InvalidInputException">Thrown if not between 1 and 8 characters long, has lowercase letters, and has no numbers and spaces, or is entirely of spaces.</exception>
        public string LicenceParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your licence plate:");
                string? input = Console.ReadLine();

                try
                {
                    ///Checks if it is between 1 and 8 characters long, only contains uppercase letters, numbers and spaces, and may consist entirely of spaces.
                    if (!Regex.IsMatch(input!, @"^(?!\s*$)[A-Z0-9 ]{1,8}$"))
                    {
                        throw new InvalidInputException("Invalid licence plate.");
                    }

                    return input!;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid licence plate.");
                }

            }
        }
    }
}