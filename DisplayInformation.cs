using System.Dynamic;

namespace ArribaEats
{
    /// <summary>
    /// Class for displaying user information 
    /// </summary>
    public abstract class DisplayInformation : MenuBase
    {

        /// <summary>
        /// User that is being displayed
        /// </summary>
        protected User User;

        /// <summary>
        /// Constructor for the menu
        /// </summary>
        /// <param name="user">User to display</param>
        public DisplayInformation(User user)
        {
            this.User = user;
        }

        /// <summary>
        /// Main information of user
        /// </summary>
        /// <returns>User type specific information menu</returns>
        public override MenuBase? Show()
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {User.Name}");
            Console.WriteLine($"Age: {User.Age}");
            Console.WriteLine($"Email: {User.Email}");
            Console.WriteLine($"Mobile: {User.Mobile}");

            return UserSpecific();

        }

        /// <summary>
        /// User type specific information menu
        /// </summary>
        /// <returns>Main menu of user type</returns>
        public abstract MenuBase UserSpecific();

    }
}