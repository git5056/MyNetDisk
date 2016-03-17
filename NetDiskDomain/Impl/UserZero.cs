using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public class UserZero : AbstractUserRunTime, IFileManager
    {
        public UserZero() { }
        public UserZero(Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("session");
            }
            this._Session = session;
        }

        #region public property
        /// <summary>
        /// _id
        /// </summary>
        public virtual int _id
        {
            get;
            set;
        }
        /// <summary>
        /// userId
        /// </summary>
        public virtual string userId
        {
            get;
            set;
        }
        /// <summary>
        /// userPwd
        /// </summary>
        public virtual string userPwd
        {
            get;
            set;
        }
        /// <summary>
        /// enabled
        /// </summary>
        public virtual bool enabled
        {
            get;
            set;
        }

        #endregion

        #region private field

        private Session _session;

        #endregion

        public virtual void Upfile()
        {
            throw new NotImplementedException();
        }

        public virtual void Downfile()
        {
            throw new NotImplementedException();
        }

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

        protected override int GetUserId()
        {
            return _id;
        }

    }
}
