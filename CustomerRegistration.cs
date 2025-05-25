namespace ArribaEats
{
    /// <summary>
    /// Menu for registering a customer (extends registration).
    /// </summary>
    public class CustomerRegistration : Registrations
    {

        /// <summary>
        /// Customer specific registration.
        /// </summary>
        /// <param name="name">Name of the customer.</param>
        /// <param name="age">Age of the customer.</param>
        /// <param name="email">Email of the customer.</param>
        /// <param name="mobile">Phone number of the customer.</param>
        /// <param name="password">Password of the customer.</param>
        public override void Registration(string name, int age, string email, string mobile, string password)
        {
            Location location = LocationParser.Parse();

            user = new Customer(name, age, email, mobile, password, "Customer",location);    

            Console.WriteLine($"You have been successfully registered as a customer, {user.Name}!");
            return;
        }

    }    
    
}