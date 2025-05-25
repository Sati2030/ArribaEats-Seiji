using System.Reflection.Emit;
using ArribaEats;

namespace ArribaEats
{

    public static class LocationParser
    {
        public static Location Parse()
        { 
            while (true)
            {
                Console.WriteLine("Please enter your location (in the form of X,Y):");
                string input = Console.ReadLine();
                string[] values = input.Split(",");

                try
                {
                    if (values.Length != 2)
                    {
                        throw new InvalidInputException("Invalid location.");
                    }

                    int x = int.Parse(values[0]);
                    int y = int.Parse(values[1]);

                    return new Location(x, y);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Invalid location.");
                }
            }            
        }
    }


    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Location(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public string GetLocation()
        {
            return $"{X},{Y}";
        }

        public int CalculateDistance(Location location)
        {
            return Math.Abs(location.X - this.X) + Math.Abs(location.Y - this.Y);
        }

    }
}