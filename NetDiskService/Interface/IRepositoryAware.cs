using NetDiskRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskService
{
    public interface IRepositoryAware<T>
    {
        IRepository<T> Repository { set; }
    }
}
