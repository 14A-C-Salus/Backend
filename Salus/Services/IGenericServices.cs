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
    public interface IGenericServices<T> where T : class
    {
        public T Create(T entity);
        public T? Read(int key);

        public IEnumerable<T> ReadAll();

        public T Update(T entity);

        public void Delete(T entity);
        public abstract Task<UserProfile> GetAuthenticatedUserProfile(DataContext dataContext);
        public int GetAuthId();
    }
}
