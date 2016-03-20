using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskService
{
    public interface ISessionService : IService<Session>
    {
        #region Extension Methods

        IUserRunTime GetCurrentUser(string sessionId);

        #endregion
    }
}
