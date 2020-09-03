using hdvatob.Data.Interfaces;
using System;
using System.Collections.Generic;
using hdvatob.Data.Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;


namespace hdvatob.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected hdvatobDbContext _context;

        public Repository (hdvatobDbContext context)
        {
            _context = context;
        }       

        protected void Save() => _context.SaveChanges();

        public bool Any(Func<T, bool> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public bool Any()
        {
            return _context.Set<T>().Any();
        }

        public int Count(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).Count();
        }

        public T Create(T entity)
        {
            try
            {
                _context.Add(entity);
                Save();
                return entity;
            }
            catch
            {
                throw;
            }
        }

        public T Delete(T entity)
        {
            _context.Remove(entity);
            Save();
            return entity;
        }

        public IEnumerable<T> Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }
        

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }

        
        public T GetById(string id)
        {
            return _context.Set<T>().Find(id);
        }

        public T GetById(decimal id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            Save();
            return entity;
        }

        public async  Task<T> GetByIdAsync(string id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(decimal id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await   _context.Set<T>().ToListAsync();
        }
        public T GetSingleNoTracking(Func<T, bool> predicate)
        {
            return _context.Set<T>().AsNoTracking().SingleOrDefault(predicate);
        }

        public T GetByTwoKey(string key1, string key2)
        {
            return _context.Set<T>().Find(key1,key2);
        }
    }
}
