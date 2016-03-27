using Iesi.Collections.Generic;
using NetDiskHelper;
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
    public class Node : NodeBase ,IEnumerable, IEnumerator
    {
        public static readonly int FOLDER_NODE_ID = 0;
        public static readonly int ROOT_NODE_ID = 0;

        #region Property


        private int __id;
        /// <summary>
        /// _id
        /// </summary>
        public virtual int _id
        {
            get
            {
                return __id;
            }
            set
            {
                AssertHelper.Against(value, val => val < 1,this.GetType());
                __id = value;
            }
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

        #endregion

        #region Public Methods

        /// <summary>
        /// 是根节点?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsRootNode()
        {
            return this.ParentNode == null;
        }

        /// <summary>
        /// 是目录节点?
        /// </summary>
        /// <returns></returns>
        public virtual bool IsFolderNode()
        {
            return this.FileSource == null;
        }

        public virtual bool HasChild(int nodeId)
        {
            var bhas = false;
            if (nodeId == ROOT_NODE_ID && IsRootNode())
                return true;
            if (nodeId < 1)
                return bhas;
            TravelForLoop(this, n =>
            {
                var tmp = n as Node;
                if (tmp._id == nodeId)
                {
                    bhas = false;
                    return true;
                }
                else
                    return false;

            });
            return bhas;
        }

        public virtual bool TeyGetChild()
        {
            return ChildNodes != null;
        }
        public virtual Node GetChildInAll(int nodeId)
        {
            if (TeyGetChild())
            {
                if (nodeId == ROOT_NODE_ID)
                {
                    return IsRootNode() ? this : null;
                }
                Node pNode = null;
                TravelForLoop(this, n =>
                {
                    var tmp = n as Node;
                    if (tmp._id == nodeId)
                    {
                        pNode = tmp;
                        return true;
                    }
                    else
                        return false;

                });
                return pNode;
            }
            return null;
        }

        public virtual Node GetChildrenInFirstGrade(int nodeId)
        {
            if (TeyGetChild())
            {
                foreach (var i in ChildNodes)
                {
                    if (i._id == nodeId)
                    {
                        return i;
                    }
                }
            }
            return null;
        }

        public virtual bool Verify()
        {
            return true;
        }

        /// <summary>
        /// ParentNode,mapping pid
        /// </summary>
        public virtual Node ParentNode
        {
            get;
            set;
        }


        private Iesi.Collections.Generic.ISet<Node> childNodes;

        /// <summary>
        /// ChildNodes,mapping pid,lazy load
        /// </summary>
        public virtual Iesi.Collections.Generic.ISet<Node> ChildNodes
        {
            get
            {
                if (childNodes == null)
                {
                    childNodes = new HashedSet<Node>();
                }
                return childNodes;
            }
            set
            {
                childNodes = value;
            }
        }

        #endregion

        #region Override NodeBase
        public override IEnumerable<NodeBase> ChildNodesWrapper
        {
            get
            {
                return ChildNodes;
            }
            set
            {
                ChildNodes = (Iesi.Collections.Generic.ISet<Node>)(value);
            }
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Common Methods


        /// <summary>
        /// Delete logical
        /// </summary>
        public virtual void RemoveLogical()
        {
            if (IsRootNode())
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
            var chnode = GetChildInAll(nodeId);
            if (chnode != null)
            {
                //移除目录节点以及其下的子节点
                foreach (Node i in chnode)
                {
                    i.RemoveLogical();
                }
            }
            //throw new Exception("未找到");
            //nop
        }

        public virtual Node AppendChild(string name, FileSource fs)
        {
            if (!IsFolderNode())
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
            var node = new Node();
            node.name = name;
            node.enabled = true;
            node.ParentNode = this;
            node.FileSource = fs;
            ChildNodes.Add(node);
            return node;
        }

        public virtual Node RenameChild(int childId, string name)
        {
            if (name == null || name.Trim().Length < 1)
            {
                throw new Exception("非有效字符");
            }
            var child = GetChildInAll(childId);
            if (child != null)
            {
                if (child == this)
                {
                    throw new Exception("不能重命名自己");
                }
                if (child.ParentNode != null)
                {
                    foreach (Node i in child.ParentNode.ChildNodes)
                    {
                        if (i.name == name && i.enabled)
                        {
                            throw new Exception("同一级下不能有重名文件");
                        }
                    }
                    child.name = name;
                    return child;
                }
                throw new Exception("root节点属于顶级，不能在其同级添加节点");

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
        /// <param name="targetId"></param>
        /// <param name="movedId"></param>
        /// <returns></returns>
        public virtual Node Move(int targetId, int movedId)
        {
            var targetNode = GetChildInAll(targetId);
            var movedNode = GetChildInAll(movedId);      
            if (targetNode == null)
            {
                ThrowsNodeException(string.Format("逻辑错误:未从当前节点'{0}'找到目标节点'{1}'", this.__id, targetId));
            }
            if (movedNode == null)
            {
                ThrowsNodeException(string.Format("逻辑错误:未从当前节点'{0}'找到被移动节点'{1}'", this.__id, movedId));
            }
            if (movedNode.HasChild(targetId))
            {
                ThrowsNodeException(string.Format("逻辑错误:目标节点'{0}'为待移动节点'{1}'的父节点", targetNode.__id, movedNode.__id));
            }
            if (!targetNode.IsFolderNode())
            {
                ThrowsNodeException(string.Format("逻辑错误:目标节点'{0}'为文件节点,不能添加子节点", targetNode.__id));
            }
            foreach (var i in targetNode.ChildNodes)
            {
                if (i.name == movedNode.name && i.__id != movedNode.__id)
                {
                    throw new Exception(string.Format("逻辑错误:目标节点'{0}'下已存在同名'{1}'节点", targetNode.__id, i.name));
                }
            }
            //update
            movedNode.ParentNode = targetNode;
            return movedNode;
        }

        public virtual bool IsRemoved()
        {
            return !(this.enabled == true);
        }

        #endregion

        #region Enumerat Provide

        internal class EnumeratHelper
        {
            public static int CurrentIndex;
            public static IList<Node> Nodes;
            public static void TreeToList(Node node, IList<Node> nodes)
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
            public static void BeginEnumerat(Node node)
            {
                CurrentIndex = 0;
                Nodes = new List<Node>();
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

        #region Throws Exception

        protected void ThrowsNodeException(string message)
        {
            throw new NodeException(message);
        }

        #endregion

    }

}