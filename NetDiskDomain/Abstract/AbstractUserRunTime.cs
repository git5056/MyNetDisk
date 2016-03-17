using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public abstract class AbstractUserRunTime : IUserRunTime, ISessionAware
    {
        private readonly TimeSpan InvalidTime = TimeSpan.FromMinutes(20);

        public virtual bool TryRegister()
        {       
            return !Invalid();
        }

        /// <summary>
        /// 登记
        /// </summary>
        public virtual void Register()
        {
            if (TryRegister())
            {
                return;
            }
            Session session = new Session();
            session.cTime = DateTime.Now;
            session.enabled = true;
            session.sessionId = Guid.NewGuid().ToString("N");
            //session.userId = GetUserId();
            _Session = session;
        }

        public virtual bool Invalid()
        {
            return _Session.cTime + InvalidTime <= DateTime.Now;
        }

        public abstract Session _Session
        {
            get;
            set;
        }

        /// <summary>
        /// 这里设计有误，使用UserId字符串那个应该更好
        /// </summary>
        /// <returns></returns>
        protected abstract int GetUserId();
    }

}
