using System.Collections.Generic;
using TestAssembly.PlantUml.Addresses;
using TestAssembly.PlantUml.Customers;
using TestAssembly.PlantUml.Products;

namespace TestAssembly.PlantUml.Orders
{
    public class Order
    {
        public Customer _customer;
        private readonly List<Product> _products = new List<Product>();

        public void AddProducts(IList<Product> products)
        {
            _products.AddRange(products);
        }

        internal void Report()
        {
            Report(_customer.Address);
            foreach (var product in _products)
            {
                product.Report();
            }
        }

        private void Report(Address address) { }
    }
}
