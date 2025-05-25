

namespace ArribaEats
{
    /// <summary>
    /// Class for the customer (extends user).
    /// </summary>
    public class Customer : User
    {

        /// <summary>
        /// List of orders placed.
        /// </summary>
        public List<Order> Orders { get; }

        /// <summary>
        /// Basic constructor for the customer
        /// </summary>
        /// <param name="name">Name of the customer.</param>
        /// <param name="age">Age of the customer.</param>
        /// <param name="email">Email of the customer.</param>
        /// <param name="mobile">Phone number of the customer.</param>
        /// <param name="password">Password of the customer.</param>
        /// <param name="type">"Customer"</param>
        /// <param name="location">Location of customer's house.</param>
        public Customer(string name, int age, string email, string mobile, string password, string type, Location location) : base(name, age, email, mobile, password, type)
        {
            this.Location = location;
            Orders = new List<Order>();
        }

    }
}