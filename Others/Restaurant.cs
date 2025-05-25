using System.Security;
using System.Text.RegularExpressions;

namespace ArribaEats
{
    public class Restaurant
    {
        public string Name { get; private set; }
        public CuisineType Cuisine { get; private set; }
        public Location Location { get; private set; }
        public Carte Menu { get; set; }
        public double Rating
        {
            get
            {
                double ra = 0.0;
                if (Reviews.Count == 0) return ra;

                foreach (Review r in Reviews)
                {
                    ra += r.Rating;
                }
                return ra / Reviews.Count;
            }
        }
        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }

        public Restaurant(string name, string type, Location location)
        {
            this.Name = name;
            this.Cuisine = new CuisineType(type);
            this.Location = location;
            Menu = new Carte();
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }
        
    }
}