
namespace ArribaEats {


    public static class UserStore
    {
        public static List<User> Users { get; } = new List<User>();
    }

    public static class RestaurantStore
    {
        public static List<Restaurant> Restaurants { get; } = new List<Restaurant>();
    }



    public class Program
    {

        static void Main(String[] args)
        {

            Console.WriteLine("Welcome to Arriba Eats!");

            MenuBase? currentMenu = new MainMenu();

            while (currentMenu != null)
            {
                currentMenu = currentMenu.Show();
            }

            Console.WriteLine("Thank you for using Arriba Eats!");

        }


    }
}