using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class BlobService : Controller
    {
        private readonly BlobContainerClient _containerClient;

        public BlobService(string connectionString, string containerName)
        {
            var blobServiceClient = new BlobServiceClient(connectionString);
            _containerClient = blobServiceClient.GetBlobContainerClient(containerName);
            _containerClient.CreateIfNotExists();
        }

        public async Task UploadBlobAsync(string blobName, Stream fileStream)
        {
            var blobClient = _containerClient.GetBlobClient(blobName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
