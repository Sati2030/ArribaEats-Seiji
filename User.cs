
namespace ArribaEats
{

    /// <summary>
    /// General class for all users (client, customer and deliverer).
    /// </summary>
    public class User
    {
        /// <summary>
        /// Type of user (client, customer or deliverer).
        /// </summary>
        public string Type { get;}
        /// <summary>
        /// Name of user.
        /// </summary>
        public string Name { get;}
        /// <summary>
        /// Age of the user.
        /// </summary>
        public int Age { get;}
        /// <summary>
        /// Email of the user.
        /// </summary>
        public string Email { get;}
        /// <summary>
        /// Phone number of the user.
        /// </summary>
        public string Mobile { get;}
        /// <summary>
        /// Password of the user.
        /// </summary>
        public string Password { get; }
        /// <summary>
        /// Current Location of the user.
        /// </summary>
        public Location? Location { get; set; }

        /// <summary>
        /// Basic constructor for any user.
        /// </summary>
        /// <param name="name">Name of the user.</param>
        /// <param name="age">Age of the user.</param>
        /// <param name="email">Email of the user.</param>
        /// <param name="mobile">Phone number of the user.</param>
        /// <param name="Password">Password of the user.</param>
        /// <param name="type">Type of user.</param>
        public User(string name, int age, string email, string mobile, string Password, string type)
        {
            this.Name = name;
            this.Age = age;
            this.Email = email;
            this.Mobile = mobile;
            this.Password = Password;
            this.Type = type;
        }

    }


}