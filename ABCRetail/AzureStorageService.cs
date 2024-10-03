using Azure.Data.Tables;
using System;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using System.IO;
using Azure.Storage.Queues;
using System.Text;
using Azure.Storage.Files.Shares;
using ABCRetail.Models;


namespace ABCRetail
{
    public class AzureStorageService
    {
        private readonly string _connectionString;
        private readonly string _tableName = "Orders";
        private readonly string _queueName = "order-queue";
        private readonly string _blobContainerName = "order-images";
        private readonly string _fileShareName = "order-documents";
        public AzureStorageService(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task StoreInformationInAzureTable(Order order)
        {
            var serviceClient = new TableServiceClient(_connectionString);
            var tableClient = serviceClient.GetTableClient(_tableName);

            await tableClient.CreateIfNotExistsAsync();

            var entity = new TableEntity(order.Id.ToString(), order.CustomerId.ToString())
        {
            { "OrderDate", order.OrderDate },
            { "TotalAmount", order.TotalAmount },
            { "Status", order.Status }
        };

            await tableClient.AddEntityAsync(entity);
        }

        public async Task WriteToBlobStorage(string fileName, Stream fileStream)
        {
            var blobServiceClient = new BlobServiceClient(_connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient("product-images");
            await containerClient.CreateIfNotExistsAsync();

            var blobClient = containerClient.GetBlobClient(fileName);
            await blobClient.UploadAsync(fileStream, overwrite: true);
        }

        public async Task SendMessageToQueue(string message)
        {
            var queueServiceClient = new QueueServiceClient(_connectionString);
            var queueClient = queueServiceClient.GetQueueClient("order-queue");
            await queueClient.CreateIfNotExistsAsync();

            var messageBytes = Encoding.UTF8.GetBytes(message);
            var base64Message = Convert.ToBase64String(messageBytes);
            await queueClient.SendMessageAsync(base64Message);
        }

        public async Task<string> ReceiveMessageFromQueue()
        {
            var queueServiceClient = new QueueServiceClient(_connectionString);
            var queueClient = queueServiceClient.GetQueueClient("order-queue");

            var response = await queueClient.ReceiveMessagesAsync();
            var message = response.Value.FirstOrDefault();

            if (message != null)
            {
                await queueClient.DeleteMessageAsync(message.MessageId, message.PopReceipt);
                return Encoding.UTF8.GetString(Convert.FromBase64String(message.MessageText));
            }

            return null;

        }

        public async Task WriteToAzureFiles(string fileName, Stream fileStream)
        {
            var shareServiceClient = new ShareServiceClient(_connectionString);
            var shareClient = shareServiceClient.GetShareClient("fileshare");
            await shareClient.CreateIfNotExistsAsync();

            var directoryClient = shareClient.GetDirectoryClient("documents");
            await directoryClient.CreateIfNotExistsAsync();

            var fileClient = directoryClient.GetFileClient(fileName);
            await fileClient.CreateAsync(fileStream.Length);
            await fileClient.UploadAsync(fileStream);
        }

    }
}
