using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    //user
    public abstract class UserBase : IUser
    {

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


    }

}
