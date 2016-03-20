using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetDiskDomain;

namespace NetDiskRepository
{
    public class SessionRepository : Repository<Session>, ISessionRepository
    {

        public IUserRunTime GetCurrentUser(string sessionid)
        {
            var listSession = FindByHQL("from " + typeof(Session) +" where sessionId=?", sessionid);
            if (listSession.Count == 1)
            {
                //登记
                //listSession[0].Wrapper.Register();
                return listSession[0].Wrapper;
            }

            if (listSession.Count == 0)
            {
                string _sessionId = Guid.NewGuid().ToString("N");
                var _session = new Session() { cTime = DateTime.Now, enabled = true, sessionId = _sessionId };
                
                //其实这是折衷的做法
                _session._User = new UserZero();
                _session._User._id = 1;
                _session._User.userId = "visitor";
                _session._User.userPwd = "visitor";
                Save(_session);
                return GetCurrentUser(_sessionId);
            }

            else
            {
                throw new Exception("internal error");
            }
        }

    }
}
