/*** 
 * @Descripttion: 
 * @Author: lichanglong
 * @Date: 2021-02-23 11:10:54
 * @FilePath: \LearnUnity\Assets\Scenes\BehaviorTree\NodeLeaf.cs
 */
using System.Collections.Generic;

namespace BehaviorTree
{
    /// <summary>
    /// 叶节点
    /// </summary>
    [System.Serializable]
    public class NodeLeaf : NodeBase
    {
        public NodeLeaf() : base()
        {
        }

        public override ResultType Execute()
        {
            return ResultType.Fail;
        }
    }
}
