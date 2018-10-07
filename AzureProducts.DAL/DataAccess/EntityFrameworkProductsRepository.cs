using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureProducts.DAL.Models;
using AzureProducts.DAL.Interfaces;
using System.Linq.Expressions;

namespace AzureProducts.DAL.DataAccess
{
    public class EntityFrameworkProductsRepository : IRepository<Product>
    {
        private readonly ProductsDbContext _context = new ProductsDbContext("ProductEntities");

        public void Add(Product item)
        {
            _context.Products.Add(item);
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
        }

        public IEnumerable<Product> Find(Expression<Func<Product, bool>> pred)
        {
            return _context.Products.Where(pred).ToList();
        }

        public Product Get(int id)
        {
            return _context.Products.FirstOrDefault(p => p.ProductID == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products.ToList();
        }

        public void Update(Product item)
        {
            if (item == null)
                return;
            
            _context.Entry(item).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
