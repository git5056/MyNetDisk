using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;
using NetDiskRepository;

namespace NetDiskService
{
    public class SessionService : Service<Session>, ISessionService
    {

        public IUserRunTime GetCurrentUser()
        {
            return (this._repository as ISessionRepository).GetCurrentUser("");
        }
    }
}
