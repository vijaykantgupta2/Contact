using System;
using System.Collections.Generic;

namespace ContactAPI.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        IContactRepository Contacts { get; }
        int Complete();
    }
}
