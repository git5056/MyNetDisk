using System;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //fileSourceStatistics
    public class FileSourceStatistics
    {

        /// <summary>
        /// fileSourceId
        /// </summary>
        public virtual int fileSourceId
        {
            get;
            set;
        }
        /// <summary>
        /// downCount
        /// </summary>
        public virtual int downCount
        {
            get;
            set;
        }
        /// <summary>
        /// uploadCount
        /// </summary>
        public virtual int uploadCount
        {
            get;
            set;
        }

    }
}