using Arribaeats;
using System.Text.RegularExpressions;

namespace ArribaEats.Registrations
{
    public class ClientRegistration : Registrations
    {

        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            Client user = new Client(name, age, email, mobile, password,"Client");

            string restaurantname = RestNameParser();
            string type = RestTypeParser();
            Location location = LocationParser();

            Restaurant restaurant = new Restaurant(restaurantname, type, location);
            RestaurantStore.Restaurants.Add(restaurant);

            user.RestaurantSetter(restaurant);

            Console.WriteLine($"You have successfully registered as a client, {name}");
            return;

        }

        public string RestNameParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your restaurant's name");
                string input = Console.ReadLine();

                try
                {
                    if (!Regex.IsMatch(input, @".*\S*"))
                    {
                        throw new InvalidInputException("Invalid restaurant name.");
                    }

                    return input;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

            }
        }

        public string RestTypeParser()
        {
            while (true)
            {
                Console.WriteLine("Please select your restaurant's style:");
                Console.WriteLine("1: Italian");
                Console.WriteLine("2: French");
                Console.WriteLine("3: Chinese");
                Console.WriteLine("4: Japanese");
                Console.WriteLine("5: American");
                Console.WriteLine("6: Australian");
                Console.WriteLine("Please enter a choice between 1 and 6:");

                string input = Console.ReadLine();
                int choice;

                if (InputParser(input, 6, out choice))
                {
                    return choice switch
                    {
                        1 => "Italian",
                        2 => "French",
                        3 => "Chinese",
                        4 => "Japanese",
                        5 => "American",
                        6 => "Australian"
                    };
                }
            }

        }

    }
}