using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureProducts.DAL.Interfaces
{
    public interface IRepository<T> : IDisposable where T : class
    {
        T Get(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> Find(Expression<Func<T, bool>> pred);
        void Add(T item);
        void Update(T item);
        void Delete(int id);
        void Save();
    }
}
