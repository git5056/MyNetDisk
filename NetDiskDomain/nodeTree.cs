using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //nodeTree
    public class NodeTree
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
        /// name
        /// </summary>
        public virtual string name
        {
            get;
            set;
        }
        /// <summary>
        /// pid
        /// </summary>
        public virtual int pid
        {
            get;
            set;
        }
        /// <summary>
        /// fileSouceId
        /// </summary>
        public virtual int fileSouceId
        {
            get;
            set;
        }

    }
}