using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //fileSource
    public class FileSource
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
        /// md5
        /// </summary>
        public virtual string md5
        {
            get;
            set;
        }
        /// <summary>
        /// isLocal
        /// </summary>
        public virtual bool isLocal
        {
            get;
            set;
        }
        /// <summary>
        /// content_type
        /// </summary>
        public virtual string content_type
        {
            get;
            set;
        }
        /// <summary>
        /// postfix
        /// </summary>
        public virtual string postfix
        {
            get;
            set;
        }
        /// <summary>
        /// path
        /// </summary>
        public virtual string path
        {
            get;
            set;
        }
        /// <summary>
        /// deleted
        /// </summary>
        public virtual bool deleted
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

    }
}