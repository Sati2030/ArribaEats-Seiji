
namespace ArribaEats
{

    /// <summary>
    /// Storage for all the Users (Clients,Cusotmers,Delivery Drivers).
    /// </summary>
    public static class UserStore
    {
        public static List<User> Users { get; } = new List<User>();
    }

    /// <summary>
    /// Storage for all the Restaurants.
    /// </summary>
    public static class RestaurantStore
    {
        public static List<Restaurant> Restaurants { get; } = new List<Restaurant>();
    }

    /// <summary>
    /// Storage for all the Orders placed.
    /// </summary>
    public static class OrderStore
    {
        public static List<Order> Orders { get; } = new List<Order>();
    }


    public class Program
    {

        /// <summary>
        /// Starts the Arriba Eats menu loop.
        /// </summary>
        /// <param name="args">Command-line arguments (not used).</param>
        static void Main(String[] args)
        {

            Console.WriteLine("Welcome to Arriba Eats!");

            ///currentMenu will be the menu that is currently being displayed
            MenuBase? currentMenu = new MainMenu();

            ///If currentMenu is null then program exits
            while (currentMenu != null)
            {
                currentMenu = currentMenu.Show();
            }

            Console.WriteLine("Thank you for using Arriba Eats!");

        }


    }
}