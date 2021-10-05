using System.Collections.Generic;
using TestAssembly.PlantUml.Orders;
using TestAssembly.PlantUml.Products;

namespace TestAssembly.PlantUml.Catalog
{
    public class ProductCatalog
    {
        private readonly List<Product> _allProducts = new List<Product>();

        internal void GonnaDoSomethingIllegalWithOrder()
        {
            var order = new Order();
            foreach (var product in _allProducts)
            {
                product.Register();
            }

            order.AddProducts(_allProducts);
        }
    }
}