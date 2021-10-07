using TestAssembly.PlantUml.Catalog;
using TestAssembly.PlantUml.Customers;
using TestAssembly.PlantUml.Xml.Processor;
using TestAssembly.PlantUml.Xml.Types;

namespace TestAssembly.PlantUml.Importer
{
    public class ProductImport
    {
        public ProductCatalog productCatalog;
        public XmlTypes xmlType;
        public XmlProcessor xmlProcessor;

        public Customer Customer => new Customer(); // violates diagram -> product import may not directly know Customer

        private ProductCatalog Parse(byte[] xml)
        {
            return new ProductCatalog();
        }
    }
}