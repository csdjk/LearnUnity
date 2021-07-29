// /*** 
//  * @Descripttion: 
//  * @Author: lichanglong
//  * @Date: 2021-04-28 11:17:36
//  * @FilePath: \LearnUnity\Assets\Scenes\BehaviorTree\NodeCombiner.cs
//  */
// namespace BehaviorTree
// {
//     /// <summary>
//     /// 选择节点(组合节点)
//     /// </summary>
//     public class NodeSelect : NodeCombiner
//     {
//         private NodeRoot lastRunningNode;
//         public NodeSelect():base(NodeType.Select)
//         {    }

//         /// <summary>
//         /// 选择节点依次遍历所有子节点，如果都返回 Fail，则向父节点返回 Fail
//         /// 直到一个节点返回 Success 或者 Running，停止后续节点的执行，向父节点
//         /// 返回 Success 或者 Running
//         /// 注意：如果节点返回 Running 需要记住这个节点，下次直接从此节点开始执行
//         /// </summary>
//         /// <returns></returns>
//         public override ResultType Execute()
//         {
//             int index = 0;
//             if (lastRunningNode != null)
//             {
//                 index = lastRunningNode.NodeIndex;
//             }
//             lastRunningNode = null;

//             ResultType resultType = ResultType.Fail;
//             for (int i = index; i < nodeChildList.Count; ++i)
//             {
//                 NodeRoot nodeRoot = nodeChildList[i];
//                 resultType = nodeRoot.Execute();
//                 if (resultType == ResultType.Fail)
//                 {
//                     continue;
//                 }

//                 if (resultType == ResultType.Success)
//                 {
//                     break;
//                 }

//                 if (resultType == ResultType.Running)
//                 {
//                     lastRunningNode = nodeRoot;
//                     break;
//                 }
//             }

//             return resultType;
//         }
//     }
// }
