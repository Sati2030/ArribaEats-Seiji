using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ArribaEats
{
    public class DeliveryRegistration : Registrations
    {

        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            Delivery delivery = new Delivery(name, age, email, mobile, password,"Delivery");
            string licence = LicenceParser();
            delivery.LicenceSetter(licence);

            User = delivery;

            Console.WriteLine($"You have been successfully registered as a deliverer, {name}!");
            return;
        }

        public string LicenceParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your licence plate:");
                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input, @"^(?!\s*$)[A-Z0-9 ]{1,8}$"))
                    {
                        throw new InvalidInputException("Invalid licence plate.");
                    }

                    return input;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }
    }
}