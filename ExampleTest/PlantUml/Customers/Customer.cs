using System;
using System.Collections.Generic;
using System.Text;
using ExampleTest.PlantUml.Addresses;
using ExampleTest.PlantUml.Orders;

namespace ExampleTest.PlantUml.Customers
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
