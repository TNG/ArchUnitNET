using ExampleTest.PlantUml.Addresses;
using ExampleTest.PlantUml.Customers;
using ExampleTest.PlantUml.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleTest.PlantUml.Orders
{
    public class Order
    {
        public Customer _customer;
        private List<Product> _products = new List<Product>();

        public void AddProducts(IList<Product> products)
        {
            _products.AddRange(products);
        }

        internal void Report()
        {
            Report(_customer.Address);
            foreach (Product product in _products)
            {
                product.Report();
            }
        }

        private void Report(Address address)
        {
        }
    }
}
