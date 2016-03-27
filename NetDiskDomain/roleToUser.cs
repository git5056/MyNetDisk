using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //roleToUser
    public class RoleToUser
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
        /// userId
        /// </summary>
        public virtual int userId
        {
            get;
            set;
        }
        ///// <summary>
        ///// roleId
        ///// </summary>
        //public virtual int roleId
        //{
        //    get;
        //    set;
        //}

        public virtual Role role
        {
            get;
            set;
        }

    }
}