using Azure.Storage.Queues;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class QueueService : Controller
    {
        private readonly QueueClient _queueClient;

        public QueueService(string connectionString, string queueName)
        {
            _queueClient = new QueueClient(connectionString, queueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
