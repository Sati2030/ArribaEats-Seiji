namespace ArribaEats
{
    /// <summary>
    /// Class for a review
    /// </summary>
    public class Review
    {
        /// <summary>
        /// Customer that gave the review.
        /// </summary>
        public Customer Customer { get; }
        /// <summary>
        /// Rating given by customer.
        /// </summary>
        public double Rating { get; }
        /// <summary>
        /// Comment left by customer.
        /// </summary>
        public string Comment { get;}

        /// <summary>
        /// Constructor for review
        /// </summary>
        /// <param name="customer">Customer that left review</param>
        /// <param name="rating">Rating left by customer</param>
        /// <param name="comment">Comment left by customer</param>
        public Review(Customer customer, double rating, string comment)
        {
            this.Customer = customer;
            this.Rating = rating;
            this.Comment = comment;
        }

    }
}