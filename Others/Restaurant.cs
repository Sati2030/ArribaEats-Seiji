using System.Security;
using System.Text.RegularExpressions;

namespace ArribaEats
{
    public class Restaurant
    {
        public string Name { get; private set; }
        public string Type { get; private set; }
        public static readonly Dictionary<string, int> TypeMapping = new()
        {
            ["Italian"] = 0,
            ["French"] = 1,
            ["Chinese"] = 2,
            ["Japanese"] = 3,
            ["American"] = 4,
            ["Australian"] = 5
        };
        public Location Location { get; private set; }
        public List<Plate> Menu { get; set; }
        public double Rating { get; set; }
        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }

        public Restaurant(string name, string type, Location location)
        {
            this.Name = name;
            this.Type = type;
            this.Location = location;
            Menu = new List<Plate>();
            Orders = new List<Order>();
            Rating = 0.0;
            Reviews = new List<Review>();
        }

        public void AddToMenu(Plate plate)
        {
            Menu.Add(plate);
        }

        public int TypeMapper()
        {
            return TypeMapping[Type.ToLower()];
        }

    }
}