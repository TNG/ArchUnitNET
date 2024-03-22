using System;
using System.Collections.Generic;
using System.Text;
using ExampleTest.PlantUml.Customers;
using ExampleTest.PlantUml.Orders;

namespace ExampleTest.PlantUml.Products
{
    public class Product
    {
        public Customer _customer;

        internal Order Order
        {
            get
            {
                return null; // the return type violates the specified UML diagram
            }
        }

        public void Register() { }

        public void Report() { }
    }
}
