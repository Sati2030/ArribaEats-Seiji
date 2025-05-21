using System.Text.RegularExpressions;

namespace ArribaEats
{
    public class Restaurant
    {
        public string Name { get; private set; }

        public string Type { get; private set; }
        public Location Location { get; private set;}


        public Restaurant(string name, string type, Location location)
        {
            this.Name = name;
            this.Type = type;
            this.Location = location;
        }
        

    }
}