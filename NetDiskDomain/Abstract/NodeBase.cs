using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetDiskDomain
{
    public abstract class NodeBase
    {
        #region Protected Methods

        /// <summary>
        /// 递归遍历
        /// </summary>
        /// <param name="pNode"></param>
        protected virtual void TravelForRecursive(NodeBase pNode, Func<NodeBase, bool> cutEarly)
        {
            if (pNode == null)
            {
                return;
            }
            if (cutEarly != null && cutEarly(pNode))
            {
                return;
            }
            foreach (var i in pNode.ChildNodesWrapper)
            {
                TravelForRecursive(i, cutEarly);
            }
        }

        /// <summary>
        /// 非递归遍历,循环加栈
        /// </summary>
        /// <param name="pNode"></param>
        protected virtual void TravelForLoop(NodeBase pNode, Func<NodeBase, bool> cutEarly)
        {
            Stack<NodeBase> stack = new Stack<NodeBase>(); ;
            stack.Push(pNode);
            NodeBase lpNode = null;

            while (stack.Count > 0)
            {
                lpNode = stack.Pop();
                if (cutEarly != null && cutEarly(lpNode))
                {
                    return;
                }
                foreach (var i in lpNode.ChildNodesWrapper)
                {
                    stack.Push(i);
                }
            }
        }

        /// <summary>
        /// 层次遍历加队列
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="cutEarly"></param>
        protected virtual void TravelFoLevel(NodeBase pNode, Func<NodeBase, bool> cutEarly)
        {
             Queue<NodeBase> queue =new Queue<NodeBase>();
             queue.Enqueue(pNode);
             while (queue.Count > 0)
             {
                 var tmp = queue.Dequeue();
                 if (cutEarly != null && cutEarly(tmp))
                 {
                     return;
                 }
                 foreach (var i in tmp.ChildNodesWrapper)
                 {
                     queue.Enqueue(i);
                 }
             }
        }

        #endregion

        public virtual NodeBase ParentNodeWrapper
        {
            get
            {
                throw new NotSupportedException("ParentNode");
            }
            set
            {
                throw new NotSupportedException("ParentNode");
            }
        }

        public abstract IEnumerable<NodeBase> ChildNodesWrapper
        {
            get;
            set;
        }

    }
}
