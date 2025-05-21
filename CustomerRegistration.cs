namespace ArribaEats.Registrations
{
    public class CustomerRegistration : Registrations
    {

        public CustomerRegistration(){}

        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            Location location = LocationParser();

            Customer user = new Customer(name, age, email, mobile, password,"Customer");
            user.LocationSetter(location);

            Console.WriteLine($"You have been successfully registered as a customer, {user.Name}");
            return;
        }

    }    
    
}