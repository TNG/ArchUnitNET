using System;
using System.Collections.Generic;
using System.Text;
using ExampleTest.PlantUml.Catalog;
using ExampleTest.PlantUml.Customers;
using ExampleTest.PlantUml.Xml.Processor;
using ExampleTest.PlantUml.Xml.Types;

namespace ExampleTest.PlantUml.Importer
{
    public class ProductImport
    {
        public ProductCatalog productCatalog;
        public XmlTypes xmlType;
        public XmlProcessor xmlProcessor;

        public Customer Customer
        {
            get
            {
                return new Customer(); // violates diagram -> product import may not directly know Customer
            }
        }

        ProductCatalog Parse(byte[] xml)
        {
            return new ProductCatalog();
        }
    }
}
