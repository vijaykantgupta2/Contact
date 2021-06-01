using ContactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAPI.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public IContactRepository Contacts { get; }

        private readonly AppDBContext _context;
        public UnitOfWork(AppDBContext context, IContactRepository contact)
        {
            this._context = context;
            this.Contacts = contact;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                _context.Dispose();
        }
    }
}
