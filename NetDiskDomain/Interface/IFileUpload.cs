using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public delegate bool DoUpHandle(out object context);

    public class FileUploadContextInfo
    {
        public static readonly int FOlDER_ID = 0;
        public static readonly int ROOT_NODE_ID = -1;
        public string FilePath;
        public string FileName;
        //将在其下‘上传文件’或‘建立虚拟文件夹’的某节点id
        //-1 表示虚拟root节点
        //一个用户只能对应一个root节点
        public int ParentNodeId;

        public bool IsLocal;
        public string MD5;
        public string ContentType;
        public string PostFix;
        public FileSource Fs;
        public UploadRecond UploadRecond;
        public Node NodeTree;
        public bool IsFlash = false;
    }

    public interface IFileUploader
    {
        void UpFile(DoUpHandle doUp);

        bool TryUp(object context);

    }

}
