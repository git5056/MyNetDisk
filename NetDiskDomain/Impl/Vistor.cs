using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public class Vistor : AbstractUserRunTime, IFileDownloader
    {
        public Vistor(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            this._Session = session;
        }

        private Session _session;

        public override Session _Session
        {
            get
            {
                return _session;
            }
            set
            {
                _session = value;
            }
        }

        /// <summary>
        /// return -1
        /// </summary>
        /// <returns></returns>
        protected override int GetUserId()
        {
            return -1;
        }


        #region public methods
        public virtual Session Log(string userId, string userPwd, Func<string, string, UserZero> TryLog)
        {
            var result=TryLog(userId,userPwd);
            if (result == null)
            {
                throw new Exception("log error");
            }
            //this._Session._User = result;
            //return this._Session;
            //create a new session ,not inherit
            Session session = new Session();
            session._User = result;
            session.cTime = DateTime.Now;
            session.enabled = true;
            session.sessionId = Guid.NewGuid().ToString("N");
            return session;
        }

        #endregion

        public DownloadRecond DownFile(Node node, Func<string, bool> doDown)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 未实现游客下载
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>

        public bool TryDown(object context)
        {
            return false;
        }
    }

}
