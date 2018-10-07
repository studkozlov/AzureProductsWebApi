using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureProducts.BLL.Infrastructure;

namespace AzureProducts.BLL.Interfaces
{
    interface IProductService : IDisposable
    {
        IEnumerable<ProductDto> GetAllProducts();
        ProductDto GetProductById(int id);
        void AddProduct(ProductDto product);
        void UpdateProduct(ProductDto product);
        void DeleteProductById(int id);
    }
}
