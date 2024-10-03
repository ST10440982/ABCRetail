using Microsoft.Azure.Cosmos.Table;

namespace ABCRetail.Models
{
    public class CustomerProfile : TableEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
}
