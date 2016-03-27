using System;
using System.Collections.Generic;
using System.Linq;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //role
    public class Role
    {

        /// <summary>
        /// _id
        /// </summary>
        public virtual int _id
        {
            get;
            set;
        }


        ///// <summary>
        ///// pid
        ///// </summary>
        //public virtual int? pid
        //{
        //    get;
        //    set;
        //}

        public virtual Role ParentRole
        {
            get;
            set;
        }

        /// <summary>
        /// name
        /// </summary>
        public virtual string name
        {
            get;
            set;
        }

        public virtual string[] GetAllRole()
        {
            IList<string> l = new List<string>();
            Role tmp=this;
            while (tmp != null)
            {
                l.Add(this.name);
                tmp = tmp.ParentRole;
            }
            return l.ToArray();
        }

    }
}