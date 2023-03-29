using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Salus.Data;
using Salus.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Salus.Services
{
    public class GenericService<T> where T : class
    {
        private readonly DataContext _dataContext;
        public readonly IHttpContextAccessor _httpContextAccessor;

        public GenericService(DataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;
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
        public UserProfile GetAuthenticatedUserProfile()
        {
            var userProfile = _dataContext.Set<UserProfile>().FirstOrDefaultAsync(u => u.authOfProfileId == GetAuthId()).Result;
            if (userProfile == null)
                throw new EUserNotFound();
            return userProfile;
        }
        public int GetAuthId()
        {
            var result = -1;

            if (_httpContextAccessor.HttpContext != null)
                result = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("id"));

            return result;
        }
    }
}
