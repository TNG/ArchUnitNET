using ExampleTest.PlantUml.Orders;
using ExampleTest.PlantUml.Products;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExampleTest.PlantUml.Catalog
{
    public class ProductCatalog
    {
        private List<Product> _allProducts = new List<Product>();

        internal void GonnaDoSomethingIllegalWithOrder()
        {
            Order order = new Order();
            foreach (Product product in _allProducts)
            {
                product.Register();
            }
            order.AddProducts(_allProducts);
        }
    }
}
