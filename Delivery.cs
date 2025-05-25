

namespace ArribaEats
{
    /// <summary>
    /// Class of a delivery driver (extends user).
    /// </summary>
    public class Delivery : User
    {
        /// <summary>
        /// Order that is assigned to driver (could be none).
        /// </summary>
        public Order? OrderAssigned { get; set; }
        /// <summary>
        /// Licence plate of the driver.
        /// </summary>
        public string Licenceplate { get; }

        /// <summary>
        /// Basic constructor of the delivery driver
        /// </summary>
        /// <param name="name">Name of the driver.</param>
        /// <param name="age">Age of the driver.</param>
        /// <param name="email">Email of the driver.</param>
        /// <param name="mobile">Phone number of the driver.</param>
        /// <param name="password">Password of the driver.</param>
        /// <param name="type">"Driver"</param>
        /// <param name="licence">Licence plate of the driver.</param>
        public Delivery(string name, int age, string email, string mobile, string password, string type, string licence) : base(name, age, email, mobile, password, type)
        {
            this.Licenceplate = licence;
        }

    }
}