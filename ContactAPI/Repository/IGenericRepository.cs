using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAPI.Repository
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(int id);
        Task<IEnumerable<T>> GetAll();
        Task Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
