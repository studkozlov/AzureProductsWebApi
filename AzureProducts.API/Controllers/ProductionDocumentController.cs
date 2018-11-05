using Microsoft.WindowsAzure.Storage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AzureProducts.BLL.Services;

namespace AzureProductsWebApi.Controllers
{
    public class ProductionDocumentController : ApiController
    {
        private ProductionDocumentManager _pdManager = new ProductionDocumentManager();

        /*// GET: api/ProductionDocument
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/ProductionDocument/5
        public string Get(int id)
        {
            return "value";
        }*/

        // POST: api/production/document
        [Route("api/production/document")]
        public IHttpActionResult Post()
        {
            if (!Request.Content.IsMimeMultipartContent())
            {
                var ex = new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
                Serilog.Log.Error(ex, "Exception has occured during Post action (production/documents).");
                throw ex;
            }

            var provider = new MultipartMemoryStreamProvider();
            Request.Content.ReadAsMultipartAsync(provider).GetAwaiter().GetResult();
            var storage = CloudStorageAccount.Parse(ConfigurationManager.AppSettings["StorageConnectionString"]);
            foreach (var file in provider.Contents)
            {
                try
                {
                    string blobName = _pdManager.UploadDocumentToBlob(storage, file);
                    _pdManager.SendNotificationToQueue(storage, file, blobName);
                }
                catch (Exception ex)
                {
                    Serilog.Log.Error(ex, "Exception has occured during uploading file to an Azure storage.");
                    return InternalServerError(ex);
                }
            }

            return Ok();
        }

        /*// PUT: api/ProductionDocument/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ProductionDocument/5
        public void Delete(int id)
        {
        }*/
    }
}
