using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using AzureProducts.BLL.Infrastructure;
using AzureProducts.BLL.Services;

namespace AzureProducts.API.Controllers
{
    public class ProductsController : ApiController
    {
        private ProductsManager _service = new ProductsManager();

        // GET api/products
        public IHttpActionResult Get()
        {
            var products = _service.GetAllProducts();
            return Ok(products);
        }

        // GET api/products/5
        public IHttpActionResult Get(int id)
        {
            if (id < 0)
                return new BadRequestResult(Request);

            var product = _service.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        // POST api/products
        public IHttpActionResult Post([FromBody]ProductDto product)
        {
            try
            {
                _service.AddProduct(product);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Exception has occured during Post action.");
                return InternalServerError(ex);
            }

            return Ok();
        }

        // PUT api/products/5
        public IHttpActionResult Put(int id, [FromBody]ProductDto product)
        {
            try
            {
                _service.UpdateProduct(product);
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Exception has occured during Put action.");
                return InternalServerError(ex);
            }

            return Content(HttpStatusCode.Accepted, product);
        }

        // DELETE api/products/5
        public IHttpActionResult Delete(int id)
        {
            _service.DeleteProductById(id);
            return Ok();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _service.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
