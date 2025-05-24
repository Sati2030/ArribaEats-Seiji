namespace ArribaEats
{
    public class Order
    {
        public int ID { get; set; }
        public double Price
        { get
            {
                double running = 0.0;
                foreach (OrderItem oi in Items)
                {
                    running += oi.TotalPrice();
                }
                return running;
            }
        }
        public List<OrderItem> Items { get; private set; }
        public Restaurant Restaurant { get; private set; }
        public Customer Customer { get; private set; }
        public Delivery Delivery { get; set; }
        private int Status { get; set; }

        public bool ReadyToCollect { get; set; }

        public bool Available { get; set; }

        public Order(Restaurant restaurant, Customer customer)
        {
            this.Restaurant = restaurant;
            this.Customer = customer;
            this.Available = true;
            this.ReadyToCollect = false;
            this.Items = new List<OrderItem>();
        }

        public void AddItem(Plate plate, int quantity)
        {
            foreach (OrderItem oi in Items)
            {
                if (oi.Plate == plate)
                {
                    oi.Quantity += quantity;
                    return;
                }
            }
            OrderItem orderitem = new OrderItem(plate, quantity);
            Items.Add(orderitem);
        }

        public void SetStatus(string status)
        {
            switch (status)
            {
                case "Ordered":
                    Status = 1;
                    break;
                case "Cooking":
                    Status = 2;
                    break;
                case "Cooked":
                    Status = 3;
                    break;
                case "Being Delivered":
                    Status = 4;
                    break;
                case "Delivered":
                    Status = 5;
                    break;
                default:
                    Status = -1;
                    break;
            }
        }

        public string GetStatus()
        {
            return Status switch
            {
                1 => "Ordered",
                2 => "Cooking",
                3 => "Cooked",
                4 => "Being Delivered",
                5 => "Delivered",
                -1 => "Invalid"
            };
        }


    }


    public class OrderItem
    {
        public Plate Plate { get; }
        public int Quantity { get; set; }

        public OrderItem(Plate plate, int quantity)
        {
            this.Plate = plate;
            this.Quantity = quantity;
        }

        public double TotalPrice()
        {
            return Plate.Price * Quantity;
        }

    }


}