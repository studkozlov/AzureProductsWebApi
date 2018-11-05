using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AzureProducts.BLL.Interfaces
{
    interface IProductionDocumentService
    {
        void SendNotificationToQueue(CloudStorageAccount storage, HttpContent file, string blobName);
        string UploadDocumentToBlob(CloudStorageAccount storage, HttpContent file);
    }
}
