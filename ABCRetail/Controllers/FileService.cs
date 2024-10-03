using Azure.Storage.Files.Shares;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace ABCRetail.Controllers
{
    public class FileService : Controller
    {
        private readonly ShareClient _shareClient;
        private readonly ShareDirectoryClient _directoryClient;

        public FileService(string connectionString, string shareName)
        {
            _shareClient = new ShareClient(connectionString, shareName);
            _shareClient.CreateIfNotExists();
            _directoryClient = _shareClient.GetRootDirectoryClient();
        }

        public async Task UploadFileAsync(string fileName, Stream fileStream)
        {
            var fileClient = _directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadAsync(fileStream);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
