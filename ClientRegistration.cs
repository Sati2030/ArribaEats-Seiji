using System.Text.RegularExpressions;

namespace ArribaEats
{
    /// <summary>
    /// Menu for registering a client (extends Registrations).
    /// </summary>
    public class ClientRegistration : Registrations
    {

        /// <summary>
        /// Clinet specifc registration.
        /// </summary>
        /// <param name="name">Name of the client.</param>
        /// <param name="age">Age of the client.</param>
        /// <param name="email">Email of the client.</param>
        /// <param name="mobile">Phone number of the client.</param>
        /// <param name="password">Password of the client.</param>
        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            string restaurantname = RestNameParser();
            CuisineType type = RestTypeParser();
            Location location = LocationParser.Parse();

            ///Creates a new restaurant based on inputs
            Restaurant restaurant = new Restaurant(restaurantname, type, location);
            ///Adds restaurant to storage of restaurants
            RestaurantStore.Restaurants.Add(restaurant);
            ///Assigns restaurant to client
            Client client = new Client(name, age, email, mobile, password, "Client",restaurant);
            user = client;

            Console.WriteLine($"You have been successfully registered as a client, {name}!");
            return;

        }

        /// <summary>
        /// Parses the name of the restaurant.
        /// </summary>
        /// <returns>Name of the restaurant.</returns>
        /// <exception cref="InvalidInputException">Thrown if is blank or empty.</exception>
        public string RestNameParser()
        {
            while (true)
            {
                Console.WriteLine("Please enter your restaurant's name:");
                string? input = Console.ReadLine();

                try
                {
                    ///Check if it has at least one non-whitespace character
                    if (!Regex.IsMatch(input!, @"\S"))
                    {
                        throw new InvalidInputException("Invalid restaurant name.");
                    }

                    return input!;
                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid restaurant name.");
                }

            }
        }

        /// <summary>
        /// Parses the type of the restaurant.
        /// </summary>
        /// <returns>The restaurant type.</returns>
        public CuisineType RestTypeParser()
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

                int choice;

                if (InputParser(6, out choice))
                {
                    return (CuisineType)choice-1;
                }
            }

        }

    }
}