using Microsoft.Azure.Cosmos.Table;

namespace ABCRetail.Models
{
    public class Product : TableEntity
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
