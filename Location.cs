
namespace ArribaEats
{

    /// <summary>
    /// Static class to parse a location
    /// </summary>
    public static class LocationParser
    {
        /// <summary>
        /// Method to parse a location
        /// </summary>
        /// <returns>Location objectn</returns>
        /// <exception cref="InvalidInputException">If the input is not in format "x,y"</exception>
        public static Location Parse()
        {
            while (true)
            {
                Console.WriteLine("Please enter your location (in the form of X,Y):");
                string? input = Console.ReadLine();

                try
                {
                    //Split input based on commas
                    string[] values = input!.Split(",");
                    
                    ///If there are more or less values than 2
                    if (values.Length != 2)
                    {
                        throw new InvalidInputException("Invalid location.");
                    }

                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[1]);

                    ///Creates new location object baseed on inputs
                    return new Location(x, y);

                }
                catch (Exception)
                {
                    Console.WriteLine("Invalid location.");
                }
            }
        }
    }

    /// <summary>
    /// Class for to denote a location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// X coordinate
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Y coordinate
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor for a location object
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Get location in string format
        /// </summary>
        /// <returns>Location in string format.</returns>
        public string GetLocation()
        {
            return $"{X},{Y}";
        }

        /// <summary>
        /// Calculate taxicab distance of another location relative to the one here
        /// </summary>
        /// <param name="location">Location to find distance</param>
        /// <returns>Distance in integer value</returns>
        public int CalculateDistance(Location location)
        {
            return Math.Abs(location.X - this.X) + Math.Abs(location.Y - this.Y);
        }

    }
}