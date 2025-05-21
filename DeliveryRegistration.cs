using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace ArribaEats.Registrations
{
    public class DeliveryRegistration : Registrations
    {

        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            Delivery user = new Delivery(name, age, email, mobile, password,"Delivery");
            string licence = LicenceParser();
            user.LicenceSetter(licence);

            Console.WriteLine($"You have been successfuly registered as a deliverer, {name}");
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
                        throw new InvalidInputException("Invalid plate number.");
                    }

                    return input;
                }
                catch (Exception e)
                {

                }




            }
        }
    }
}