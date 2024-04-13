using TestAssembly.PlantUml.Addresses;
using TestAssembly.PlantUml.Orders;

namespace TestAssembly.PlantUml.Customers
{
    public class Customer
    {
        public Address Address { get; set; }

        internal void AddOrder(Order order)
        {
            // simply having such a parameter violates the specified UML diagram
        }
    }
}
