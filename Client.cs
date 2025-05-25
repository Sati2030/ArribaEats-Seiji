
namespace ArribaEats
{
    /// <summary>
    /// Class for a client (extends user).
    /// </summary>
    public class Client : User
    {
        /// <summary>
        /// Restaurant that is assigned to the client
        /// </summary>
        public Restaurant Restaurant { get; }

        /// <summary>
        /// Costructs the client object
        /// </summary>
        /// <param name="name">Name of the client.</param>
        /// <param name="age">Age of the client.</param>
        /// <param name="email">Email of the client.</param>
        /// <param name="mobile">Phone number of the client.</param>
        /// <param name="password">Password of the client.</param>
        /// <param name="type">"Client"</param>
        /// <param name="restaurant">Restaurant that client owns.</param>
        public Client(string name, int age, string email, string mobile, string password, string type, Restaurant restaurant) : base(name, age, email, mobile, password, type)
        {
            this.Restaurant = restaurant;
        }

    }
}