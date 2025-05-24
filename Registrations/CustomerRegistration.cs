namespace ArribaEats
{
    public class CustomerRegistration : Registrations
    {

        public CustomerRegistration(){}

        public override void Registration(string name, int age, string email, string mobile, string password)
        {

            User = new Customer(name, age, email, mobile, password,"Customer");
            User.Location = LocationParser.Parse();

            Console.WriteLine($"You have been successfully registered as a customer, {User.Name}!");
            return;
        }

    }    
    
}