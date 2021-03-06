﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public interface ISessionRepository : IRepository<Session>
    {
        #region Extension Methods

        IUserRunTime GetCurrentUser(string sessionid);

        #endregion
    }
}
