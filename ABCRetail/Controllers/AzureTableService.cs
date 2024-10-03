using ABCRetail.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class AzureTableService : Controller
    {
        private CloudTable _customerTable;
        private CloudTable _productTable;

        public AzureTableService(string storageConnectionString)
        {
            var storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            _customerTable = tableClient.GetTableReference("Customers");
            _productTable = tableClient.GetTableReference("Products");

            _customerTable.CreateIfNotExists();
            _productTable.CreateIfNotExists();
        }

        public async Task InsertCustomerAsync(CustomerProfile customer)
        {
            var insertOperation = TableOperation.Insert(customer);
            await _customerTable.ExecuteAsync(insertOperation);
        }

        public async Task InsertProductAsync(Product product)
        {
            var insertOperation = TableOperation.Insert(product);
            await _productTable.ExecuteAsync(insertOperation);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
