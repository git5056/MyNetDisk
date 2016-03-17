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
        private const string DEFAULT_NULL_MD5_STRING = "";

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

        #region public methods

        /// <summary>
        /// if obj is string,deal with local path
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            //deal with localpath
            if (obj is string)
            {
                if (isLocal)
                {
                    return NetDiskHelper.FileHelper.CompareWithMD5(md5, obj as string);
                }
                //NetDiskHelper.FileHelper.Compare()
            }
            return base.Equals(obj);
        }

        /// <summary>
        /// Validate OneSelf
        /// </summary>
        /// <returns></returns>
        public virtual bool TryValidate()
        {
            if (isLocal)
            {
                return NetDiskHelper.FileHelper.CompareWithMD5(md5, path);
            }
            //con't deal with !isLocal
            else
            {
                return true;
            }
            
        }

        /// <summary>
        /// if TryValidate() return true,then exec nop
        /// </summary>
        public virtual void Validate()
        {
            if (TryValidate())
            {
                //nop
            }
            else
            {
                throw new Exception("DB出错,");
            }
        }

        #endregion

    }
}