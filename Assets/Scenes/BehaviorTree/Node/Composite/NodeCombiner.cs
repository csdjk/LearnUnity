/*** 
 * @Descripttion: 
 * @Author: lichanglong
 * @Date: 2021-02-23 11:09:58
 * @FilePath: \LearnUnity\Assets\Scenes\BehaviorTree\Node\Composite\NodeCombiner.cs
 */
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 组合节点
    /// </summary>
    public abstract class NodeCombiner : NodeBase
    {
        // 保存子节点
        protected List<NodeBase> nodeChildList = new List<NodeBase>();

        public NodeCombiner(NODE_TYPE nodeType) : base()
        {
            SetNodeType(nodeType);
        }

        public void AddNode(NodeBase node)
        {
            int count = nodeChildList.Count;
            node.NodeIndex = count;
            nodeChildList.Add(node);
        }

        public List<NodeBase> GetChilds()
        {
            return nodeChildList;
        }

        public override ResultType Execute()
        {
            return ResultType.Fail;
        }
    }
}
