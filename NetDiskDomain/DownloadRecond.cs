using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //downloadRecond
    public class DownloadRecond
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
        ///// nodeId
        ///// </summary>
        //public virtual int nodeId
        //{
        //    get;
        //    set;
        //}
        /// <summary>
        /// The Special Node
        /// </summary>
        public virtual Node Node
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
        /// downloadTime
        /// </summary>
        public virtual DateTime downloadTime
        {
            get;
            set;
        }

    }
}