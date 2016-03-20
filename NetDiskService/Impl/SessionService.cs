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
        public IUserRunTime GetCurrentUser(string sessionId)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString("N");
            }
            return (this._repository as ISessionRepository).GetCurrentUser(sessionId);
        }

    }

}
