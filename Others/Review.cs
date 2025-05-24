namespace ArribaEats
{
    public class Review
    {
        public Customer Customer { get; }
        public double Rating { get; }
        public string Comment { get; private set; }

        public Review(Customer customer, double rating, string comment)
        {
            this.Customer = customer;
            this.Rating = rating;
            this.Comment = comment;
        }

    }
}