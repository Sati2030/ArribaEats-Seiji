using ArribaEats;

namespace Arribaeats
{
    public class Client : User
    {

        public Restaurant Restaurant { get; private set; }

        public Client(string name, int age, string email, string mobile, string password,string type) : base(name, age, email, mobile, password,type) { }

        public void RestaurantSetter(Restaurant restaurant)
        {
            this.Restaurant = restaurant;
        }

    }
}