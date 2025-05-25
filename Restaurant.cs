namespace ArribaEats
{


    public enum CuisineType
    {
        Italian = 0,
        French = 1,
        Chinese = 2,
        Japanese = 3,
        American = 4,
        Australian = 5
    }



    /// <summary>
    /// Class for a restaurant.
    /// </summary>
    public class Restaurant
    {
        /// <summary>
        /// Name of the restaurant.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Type of cuisine of restaurant.
        /// </summary>
        public CuisineType Cuisine { get; }
        /// <summary>
        /// Location of the restaurant.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Menu of plates that the restaurant offers.
        /// </summary>
        public Carte Menu { get; }
        /// <summary>
        /// Average rating (based on list of Reviews).
        /// </summary>
        public double Rating
        {
            ///Computes the average of ratings in Reviews
            get
            {
                double ra = 0.0;
                ///If no reviews return 0.0
                if (Reviews.Count == 0) return ra;

                foreach (Review r in Reviews)
                {
                    ra += r.Rating;
                }
                return ra / Reviews.Count;
            }
        }

        /// <summary>
        /// List of orders placed on restaurant
        /// </summary>
        public List<Order> Orders { get; set; }

        /// <summary>
        /// List of reviews given to restaurant
        /// </summary>
        public List<Review> Reviews { get; set; }

        /// <summary>
        /// Basic constructor for a restaurant.
        /// </summary>
        /// <param name="name">Name of the restaurant.</param>
        /// <param name="type">Type of the restaurant.</param>
        /// <param name="location">Location of the restaurant.</param>
        public Restaurant(string name, CuisineType type, Location location)
        {
            this.Name = name;
            this.Cuisine = type;
            this.Location = location;
            Menu = new Carte();
            Orders = new List<Order>();
            Reviews = new List<Review>();
        }

        public string GetCusine()
        {
            return Cuisine.ToString();
        }

    }
}