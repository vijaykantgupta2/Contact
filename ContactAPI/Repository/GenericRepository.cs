using Microsoft.EntityFrameworkCore;
using ContactAPI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ContactAPI.Repository
{
    public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly AppDBContext _context;
        public GenericRepository(AppDBContext context)
        {
            this._context = context;
        }

        public async Task<T> Get(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task Add(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public void Delete(int id)
        {
            var TT =  _context.Set<T>().Find(id);
            _context.Set<T>().Remove(TT);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }
}
