namespace ArribaEats
{

    /// <summary>
    /// Enumeration for order status
    /// </summary>
    public enum OrderStatus
    {
        Ordered = 1,
        Cooking = 2,
        Cooked = 3,
        BeingDelivered = 4,
        Delivered = 5,
        Invalid = -1
    }



    /// <summary>
    /// Class for and order.
    /// </summary>
    public class Order
    {
        /// <summary>
        /// Order ID.
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// Total Price of the order (based on adding price of all OrderItems).
        /// </summary>
        public double Price
        {
            get
            {
                double running = 0.0;
                foreach (OrderItem oi in Items)
                {
                    running += oi.TotalPrice();
                }
                return running;
            }
        }
        /// <summary>
        /// List of Items.
        /// </summary>
        public List<OrderItem> Items { get; }
        /// <summary>
        /// Restaurant its from.
        /// </summary>
        public Restaurant Restaurant { get; }
        /// <summary>
        /// Customer assigned to order
        /// </summary>
        public Customer Customer { get; }
        /// <summary>
        /// Delivery driver assgigned to order
        /// </summary>
        public Delivery? Delivery { get; set; }
        /// <summary>
        /// Status of the order 
        /// </summary>
        public OrderStatus Status { get; set; }
        /// <summary>
        /// If Driver is in restaurant to pickup order
        /// </summary>
        public bool ReadyToCollect { get; set; }
        /// <summary>
        /// If Order has not been assigned a driver.
        /// </summary>
        public bool Available { get; set; }

        /// <summary>
        /// Basic constructor for an order
        /// </summary>
        /// <param name="restaurant">Restaurant assigned</param>
        /// <param name="customer">Customer assgined</param>
        public Order(Restaurant restaurant, Customer customer)
        {
            this.Restaurant = restaurant;
            this.Customer = customer;
            ///Available for assignment
            this.Available = true;
            ///Driver hasn't arrived to restaurant
            this.ReadyToCollect = false;
            this.Items = new List<OrderItem>();
        }

        /// <summary>
        /// Add item to order
        /// </summary>
        /// <param name="plate">Plate to be added</param>
        /// <param name="quantity">Quantity</param>
        public void AddItem(Plate plate, int quantity)
        {
            foreach (OrderItem oi in Items)
            {
                ///If plate already in order, just add more quantity to it
                if (oi.Plate == plate)
                {
                    oi.Quantity += quantity;
                    return;
                }
            }
            OrderItem orderitem = new OrderItem(plate, quantity);
            Items.Add(orderitem);
        }

        /// <summary>
        /// Gets the status of order in string
        /// </summary>
        /// <returns>Order status in string</returns>
        public string GetStatus()
        {
            return Status.ToString();
        }

    }

    /// <summary>
    /// Class for an item in the order
    /// </summary>
    public class OrderItem
    {
        /// <summary>
        /// Plate that is the item
        /// </summary>
        public Plate Plate { get; }
        /// <summary>
        /// Quantity of the plate
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Constructor for an entry in order
        /// </summary>
        /// <param name="plate">Plate of the order</param>
        /// <param name="quantity">Quantity of the order</param>
        public OrderItem(Plate plate, int quantity)
        {
            this.Plate = plate;
            this.Quantity = quantity;
        }

        /// <summary>
        /// Computes total price of order entry
        /// </summary>
        /// <returns>Total price of order entry</returns>
        public double TotalPrice()
        {
            return Plate.Price * Quantity;
        }

    }


}