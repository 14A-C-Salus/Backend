using Microsoft.EntityFrameworkCore;
using Salus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salus.Services
{
    public class CRUD<T> where T : class
    {
        private readonly DataContext _dataContext;

        public CRUD(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public virtual T Create(T entity)
        {
            _dataContext.Set<T>().Add(entity);
            _dataContext.SaveChanges();
            return entity;
        }
        public virtual T? Read(int key)
        {
            return _dataContext.Set<T>().Find(key);
        }

        public virtual IEnumerable<T> ReadAll()
        {
            return _dataContext.Set<T>().ToList();
        }

        public virtual T Update(T entity)
        {
            _dataContext.Entry(entity).State = EntityState.Modified;
            _dataContext.SaveChanges();
            return entity;
        }

        public virtual void Delete(T entity)
        {
            _dataContext.Set<T>().Remove(entity);
            _dataContext.SaveChanges();
        }
    }
}
