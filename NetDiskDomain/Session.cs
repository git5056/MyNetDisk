using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //session
    public class Session
    {

        /// <summary>
        /// _id
        /// </summary>
        public virtual int _id
        {
            get;
            set;
        }
        /// <summary>
        /// sessionId
        /// </summary>
        public virtual string sessionId
        {
            get;
            set;
        }
        /// <summary>
        /// userId
        /// </summary>
        public virtual int userId
        {
            get;
            set;
        }
        /// <summary>
        /// cTime
        /// </summary>
        public virtual DateTime cTime
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

        public virtual UserZero _User
        {
            get;
            set;
        }

        public AbstractUserRunTime CurrentUser
        {
            get
            {
                if (_User ==null)
                {
                    return new Vistor(this);
                }
                else
                {
                    return new UserZero(this);
                }
            }
        }

    }
}