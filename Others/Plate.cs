namespace ArribaEats
{
    public class Plate
    {
        public string Name { get; }
        public double Price { get; }

        public Plate(string name, double price)
        {
            this.Name = name;
            this.Price = price;
        }

    }
}