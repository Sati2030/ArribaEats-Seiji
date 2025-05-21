using System.Dynamic;

namespace ArribaEats
{
    public class Delivery : User
    {
        public string Licenceplate { get; private set; }

        public Delivery(string name, int age, string email, string mobile, string password,string type) : base(name, age, email, mobile, password,type) { }

        public void LicenceSetter(string licence)
        {
            this.Licenceplate = licence;
        }

    }
}