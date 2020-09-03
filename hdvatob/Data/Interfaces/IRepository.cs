using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace hdvatob.Data.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        Task<IEnumerable<T>> GetAllAsync();
        IEnumerable<T> Find(Func<T, bool> predicate);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        T GetSingleNoTracking(Func<T, bool> predicate);

        T GetByTwoKey(string key1, string key2);

        T GetById(int id);
        T GetById(string id);
        T GetById(decimal id);

        Task<T> GetByIdAsync(string id);
        Task<T> GetByIdAsync(int id);
        Task<T> GetByIdAsync(decimal id);

        T Create(T entity);

      
       

        T Update(T entity);

      

        T Delete(T entity);

        int Count(Func<T, bool> predicate);

        bool Any(Func<T, bool> predicate);

        bool Any();
    }
}
