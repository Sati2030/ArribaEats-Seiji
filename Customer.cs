using System.Security.Cryptography;

namespace ArribaEats
{
    public class Customer : User
    {
        public Customer(string name, int age, string email, string mobile, string password, string type) : base(name, age, email, mobile, password,type) { }


        

    }
}