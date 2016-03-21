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
    public class NodeTree : NodeBase ,IEnumerable, IEnumerator
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
                var tmp = n as NodeTree;
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
        public virtual NodeTree GetChildInAll(int nodeId)
        {
            if (TeyGetChild())
            {
                if (nodeId == ROOT_NODE_ID)
                {
                    return IsRootNode() ? this : null;
                }
                NodeTree pNode = null;
                TravelForLoop(this, n =>
                {
                    var tmp = n as NodeTree;
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

        public virtual NodeTree GetChildrenInFirstGrade(int nodeId)
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
        public virtual NodeTree ParentNode
        {
            get;
            set;
        }


        private Iesi.Collections.Generic.ISet<NodeTree> childNodes;

        /// <summary>
        /// ChildNodes,mapping pid,lazy load
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

        #region Override NodeBase
        public override IEnumerable<NodeBase> ChildNodesWrapper
        {
            get
            {
                return ChildNodes;
            }
            set
            {
                ChildNodes = (Iesi.Collections.Generic.ISet<NodeTree>)(value);
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
                foreach (NodeTree i in chnode)
                {
                    i.RemoveLogical();
                }
            }
            //throw new Exception("未找到");
            //nop
        }

        public virtual NodeTree AppendChild(string name, FileSource fs)
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
            var node = new NodeTree();
            node.name = name;
            node.enabled = true;
            node.ParentNode = this;
            node.FileSource = fs;
            ChildNodes.Add(node);
            return node;
        }

        public virtual NodeTree RenameChild(int childId, string name)
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
                if (ParentNode != null)
                {
                    foreach (NodeTree i in ParentNode)
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
        /// <param name="parentId"></param>
        /// <param name="nodeId"></param>
        /// <returns></returns>
        public virtual NodeTree Move(int parentId, int nodeId)
        {
            var pNode = GetChildInAll(parentId);
            var cNode = GetChildInAll(nodeId);
            if (pNode == null || cNode == null)
            {
                throw new Exception("节点不存在");
            }
            if (cNode.HasChild(parentId))
            {
                throw new Exception("不能将父节点移到子节点");
            }
            foreach (var i in pNode.ChildNodes)
            {
                if (i.name == cNode.name)
                {
                    throw new Exception("同一级内不能有同名node");
                }
            }
            //update
            cNode.ParentNode = this;
            return cNode;
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