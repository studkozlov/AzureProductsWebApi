using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AzureProducts.BLL.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Configuration;

namespace AzureProducts.BLL.Services
{
    public class ProductionDocumentManager : IProductionDocumentService
    {
        public void SendNotificationToQueue(CloudStorageAccount storage, HttpContent file, string blobName)
        {
            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            var filesize = file.Headers.ContentDisposition.Size;
            var creationDate = file.Headers.ContentDisposition.CreationDate;

            var queueClient = storage.CreateCloudQueueClient();
            var queue = queueClient.GetQueueReference("notifications-queue");
            var message = new CloudQueueMessage($"New document was added. Blob name:{blobName ?? string.Empty}; file name:{filename ?? string.Empty}; " +
                                                $"filesize:{filesize ?? 0}; creationDate:{(creationDate.HasValue ? creationDate.Value.UtcDateTime.ToString() : string.Empty)}.");
            queue.AddMessage(message);
        }

        public string UploadDocumentToBlob(CloudStorageAccount storage, HttpContent file)
        {
            var filename = file.Headers.ContentDisposition.FileName.Trim('\"');
            var buffer = file.ReadAsByteArrayAsync().Result;

            var blobClient = storage.CreateCloudBlobClient();
            var cloudBlobContainer = blobClient.GetContainerReference("production-docs-blob-container");
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(filename);
            cloudBlockBlob.Properties.ContentType = file.Headers.ContentType.ToString();

            cloudBlockBlob.UploadFromStreamAsync(new System.IO.MemoryStream(buffer));
            return filename;
        }
    }
}
