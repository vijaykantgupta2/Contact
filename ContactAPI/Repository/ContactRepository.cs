using Microsoft.EntityFrameworkCore;
using ContactAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactAPI.Repository
{
    public class ContactRepository:GenericRepository<Contact>,IContactRepository
    {
        public ContactRepository(AppDBContext context):base(context)
        {

        }

    }
}
