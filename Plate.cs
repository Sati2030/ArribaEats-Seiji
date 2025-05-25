namespace ArribaEats
{
    /// <summary>
    /// Class for a plate
    /// </summary>
    public class Plate
    {
        /// <summary>
        /// Name of the plate
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Price of the plate
        /// </summary>
        public double Price { get; }

        /// <summary>
        /// Constructor for a plate object
        /// </summary>
        /// <param name="name">Name of the plate</param>
        /// <param name="price">Price of the plate</param>
        public Plate(string name, double price)
        {
            this.Name = name;
            this.Price = price;
        }

    }
}