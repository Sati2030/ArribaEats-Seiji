using System.Data.SqlTypes;
using System.Text.RegularExpressions;
using System.Xml.Schema;

namespace ArribaEats
{

    public class User
    {
        public string Type { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Email { get; private set; }
        public string Mobile { get; private set; }
        public string Password { get; private set; }
        public Location? Location { get; set; }

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