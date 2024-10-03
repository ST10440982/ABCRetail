using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.IO;
using ABCRetail.Models;
using System.Text;


namespace ABCRetail.Controllers
{
    public class OrderController : Controller
    {
        private readonly AzureTableService _tableService;
        private readonly BlobService _blobService;
        private readonly QueueService _queueService;
        private readonly FileService _fileService;

        public OrderController(IConfiguration configuration)
        {
            string storageConnectionString = configuration.GetConnectionString("st10440982");
            _tableService = new AzureTableService(storageConnectionString);
            _blobService = new BlobService(storageConnectionString, "order-images"); 
            _queueService = new QueueService(storageConnectionString, "order-queue");
            _fileService = new FileService(storageConnectionString, "order-documents"); 
        }

        // Action to upload an image
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    await _blobService.UploadBlobAsync(file.FileName, stream);
                    await _queueService.SendMessageAsync($"Image '{file.FileName}' uploaded.");
                }
            }
            return RedirectToAction("Index"); 
        }

        // Action to log contact information
        [HttpPost]
        public async Task<IActionResult> LogContact(string contactInfo)
        {
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(contactInfo)))
            {
                await _fileService.UploadFileAsync("contacts.txt", stream);
            }
            return RedirectToAction("Index"); 
        }
    }
}