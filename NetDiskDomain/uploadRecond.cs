using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //uploadRecond
    public class UploadRecond
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
        /// fileSourceId
        /// </summary>
        public virtual int fileSourceId
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
        /// uploadTime
        /// </summary>
        public virtual DateTime uploadTime
        {
            get;
            set;
        }
        /// <summary>
        /// flashUp
        /// </summary>
        public virtual bool flashUp
        {
            get;
            set;
        }

    }
}