using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Salus.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Salus.Services
{
    public class GenericServices<T> where T : class,  IGenericServices<T>
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public GenericServices(DataContext dataContext, HttpContextAccessor httpContextAccessor)
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
        public async Task<UserProfile> GetAuthenticatedUserProfile(DataContext dataContext)
        {
            var userProfile = await dataContext.Set<UserProfile>().FirstOrDefaultAsync(u => u.authOfProfileId == GetAuthId());
            if (userProfile == null)
                throw new Exception("You need to create a user profile first!");

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
