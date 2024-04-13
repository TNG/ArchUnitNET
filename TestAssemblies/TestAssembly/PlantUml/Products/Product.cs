using TestAssembly.PlantUml.Customers;
using TestAssembly.PlantUml.Orders;

namespace TestAssembly.PlantUml.Products
{
    public class Product
    {
        public Customer _customer;

        internal Order Order => null; // the return type violates the specified UML diagram

        public void Register() { }

        public void Report() { }
    }
}
