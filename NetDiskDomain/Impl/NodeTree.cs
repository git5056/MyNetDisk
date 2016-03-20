using Iesi.Collections.Generic;
using System;
using System.Collections;
using System.Collections.Generic;

//Nhibernate Code Generation Template 1.0
//author:MythXin
//blog:www.cnblogs.com/MythXin
//Entity Code Generation Template
namespace NetDiskDomain
{
    //nodeTree
    public class NodeTree:IEnumerable,IEnumerator
    {
        public static readonly int DIR_NODE_ID = 0;
        public static readonly int ROOT_NODE_ID = -1;

        #region Property

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

        ///// <summary>
        ///// fileSouceId
        ///// </summary>
        //public virtual int fileSouceId
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// enabled
        /// </summary>
        public virtual bool enabled
        {
            get;
            set;
        }

        /// <summary>
        /// FileSource
        /// </summary>
        public virtual FileSource FileSource
        {
            get;
            set;
        }


        private Iesi.Collections.Generic.ISet<NodeTree> childNodes;

        /// <summary>
        /// ChildNodes
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<NodeTree> ChildNodes
        {
            get
            {
                if (childNodes == null)
                {
                    childNodes = new HashedSet<NodeTree>();
                }
                return childNodes;
            }
            set
            {
                childNodes = value;
            }
        }

        #endregion

        #region Methods

        public virtual NodeTree MatchFileSourceId(int fsId)
        {
            var nodes = new List<NodeTree>();

            TreeToList(this, nodes);
            foreach (var i in nodes)
            {
                if (i.FileSource != null && i.FileSource._id == fsId)
                {
                    return i;
                }
            }
            return null;
        }

        /// <summary>
        /// Delete logical
        /// </summary>
        public virtual void RemoveOneSelf(){
            if (pid == -1)
            {
                throw new Exception("根节点不能删除");
            }
            if (!enabled)
            {
                //throw new Exception("已被删除的不能再被删除");
                //处理子节点问题，待优化
                //nop
            }
            //deleted mark
            enabled = false;
        }

        public virtual void RemoveChild(int nodeId)
        {
            var chnode = FindIt(nodeId);
            if (chnode != null)
            {
                //移除目录节点以及其下的子节点
                foreach (NodeTree i in chnode)
                {
                    i. RemoveOneSelf();
                }
            }
            //throw new Exception("未找到");
            //nop
        }

        public virtual NodeTree AppendChild(string name, FileSource fs)
        {
            if (!IsDirNode())
            {
                throw new Exception("只有文件夹节点可以添加子节点");
            }
            if (IsRemoved())
            {
                throw new Exception("该节点已被逻辑删除");
            }
            foreach (var i in ChildNodes)
            {
                if (i.name == name && i.enabled)
                {
                    throw new Exception("同一层次下不能有同名节点");
                }
            }
            var node = new NodeTree();
            node.name = name;
            node.enabled = true;
            node.pid = this._id;
            node.FileSource = fs;
            ChildNodes.Add(node);
            return node;
        }

        public virtual NodeTree RenameChild(int childId,string name)
        {
            if (name == null || name.Trim().Length < 1)
            {
                throw new Exception("非有效字符");
            }
            var child = FindIt(childId);
            if (child != null)
            {
                if (child == this)
                {
                    throw new Exception("不能重命名自己");
                }
                var pNode = FindIt(child.pid);
                //check the same name
                foreach (NodeTree i in pNode)
                {
                    if (i.name == name && i.enabled)
                    {
                        throw new Exception("同一级下不能有重名文件");
                    }
                }
                child.name = name;
                return child;
            }
            else
            {
                throw new Exception("未找到该节点" + childId);
            }

        }

        /// <summary>
        /// move the node that nodeId pointed to the node that parentId pointed
        /// return updated node
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public virtual NodeTree Move(int parentId, int nodeId)
        {
            var pNode = FindIt(parentId);
            var node = FindIt(nodeId);
            if (pNode == null || node == null)
            {
                throw new Exception("节点不存在");
            }
            var tmp = node.FindIt(parentId);
            if (tmp != null)
            {
                throw new Exception("不能将父节点移到子节点");
            }
            foreach (var i in pNode.childNodes)
            {
                if (i.name == node.name)
                {
                    throw new Exception("同一级内不能有同名node");
                }
            }
            //update
            node.pid = pNode._id;
            return node;
        }

        public virtual bool IsFileNode()
        {
            return !IsDirNode();
        }

        public virtual bool IsDirNode()
        {
            return  this.FileSource==null || this.FileSource._id ==DIR_NODE_ID;
        }

        public virtual bool IsRootNode()
        {
            return this.pid == ROOT_NODE_ID;
        }

        public virtual bool IsRemoved()
        {
            return !(this.enabled == true);
        }

        #endregion

        #region FindIt
        /// <summary>
        /// To Find Child Node In This Tree,Or Return NULL
        /// </summary>
        /// <param name="nodeId"></param>
        /// <returns></returns>

        public virtual NodeTree FindIt(int nodeId)
        {
            if (ROOT_NODE_ID == nodeId)
                if (IsRootNode())
                    return this;
                else
                    return null;
            return FindItInternal(nodeId, this);
        }
        private NodeTree FindItInternal(int nodeId, NodeTree node)
        {

            var nodes = new List<NodeTree>();
            TreeToList(node, nodes);
            foreach (var i in nodes)
            {
                if (i._id == nodeId)
                    return i;
            }
            return null;
        }

        /// <summary>
        /// to make code simpler
        /// </summary>
        /// <param name="node"></param>
        /// <param name="nodes"></param>
        private void TreeToList(NodeTree node, IList<NodeTree> nodes)
        {
            nodes.Add(node);
            foreach (var i in node.ChildNodes)
            {
                TreeToList(i, nodes);
            }
        }

        #endregion

        #region Enumerat Provider

        internal class EnumeratHelper
        {
            public static int CurrentIndex;
            public static IList<NodeTree> Nodes;
            public static void TreeToList(NodeTree node, IList<NodeTree> nodes)
            {
                if (node == null)
                {
                    throw new ArgumentNullException("nodes");
                }
                nodes.Add(node);
                foreach (var i in node.ChildNodes)
                {
                    TreeToList(i, nodes);
                }
            }
            public static void BeginEnumerat(NodeTree node)
            {
                CurrentIndex = 0;
                Nodes = new List<NodeTree>();
                TreeToList(node, EnumeratHelper.Nodes);
            }

        }

        public virtual IEnumerator GetEnumerator()
        {
            EnumeratHelper.BeginEnumerat(this);
            return this;
        }

        public virtual object Current
        {
            get
            {
                return EnumeratHelper.Nodes[EnumeratHelper.CurrentIndex++];
            }
        }

        public virtual bool MoveNext()
        {
            if (EnumeratHelper.CurrentIndex < EnumeratHelper.Nodes.Count)
            {
                return true;
            }
            return false;
        }

        public virtual void Reset()
        {
            EnumeratHelper.CurrentIndex = 0;
        }

        #endregion

    }

}