using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public interface ISessionAware
    {
        Session _Session
        {
            get;
            set;
        }
    }
}
