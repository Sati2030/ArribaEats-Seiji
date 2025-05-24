using System.Dynamic;

namespace ArribaEats
{
    public abstract class DisplayInformation : MenuBase
    {

        public User User { get; }

        public DisplayInformation(User user)
        {
            this.User = user;
        }

        public override MenuBase? Show()
        {
            Console.WriteLine("Your user details are as follows:");
            Console.WriteLine($"Name: {User.Name}");
            Console.WriteLine($"Age: {User.Age}");
            Console.WriteLine($"Email: {User.Email}");
            Console.WriteLine($"Mobile: {User.Mobile}");

            return UserSpecific();

        }

        public abstract MenuBase UserSpecific();

    }
}