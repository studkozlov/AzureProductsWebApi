using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureProducts.DAL.Models
{
    class ProductsDbContext : DbContext
    {
        public virtual DbSet<Product> Products { get; set; }
        
        public ProductsDbContext()
        { }
        public ProductsDbContext(string connection) : base(connection) { }
    }
}
