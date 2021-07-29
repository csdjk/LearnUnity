namespace BehaviorTree
{
    /// <summary>
    /// 节点超类
    /// </summary>
    public abstract class NodeBase
    {
        /// <summary>
        /// 节点类型
        /// </summary>
        protected NODE_TYPE nodeType;
        /// <summary>
        /// 节点序列
        /// </summary>
        private int nodeIndex;

        /// <summary>
        /// 节点Id
        /// </summary>
        private int nodeId;

        /// <summary>
        /// EntityId
        /// </summary>
        private int entityId;

        /// <summary>
        /// 权重
        /// </summary>
        private int priority;

        protected NODE_STATUS nodeStatus = NODE_STATUS.READY;

        public NodeBase()
        {
        }

        protected void SetNodeType(NODE_TYPE nodeType)
        {
            this.nodeType = nodeType;
        }

        public NODE_TYPE NodeType
        {
            get { return nodeType; }
        }

        public int NodeIndex
        {
            get { return nodeIndex; }
            set { nodeIndex = value; }
        }

        public int NodeId
        {
            get { return nodeId; }
            set { nodeId = value; }
        }

        public int EntityId
        {
            get { return entityId; }
            set { entityId = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        /// <summary>
        /// 进入节点
        /// </summary>
        public virtual void OnEnter()
        {
            //ProDebug.Logger.LogError("OnEnter:" + NodeId);
        }

        /// <summary>
        /// 执行节点抽象方法
        /// </summary>
        /// <returns>返回执行结果</returns>
        public abstract ResultType Execute();

        /// <summary>
        /// 退出节点
        /// </summary>
        public virtual void OnExit()
        {
            //ProDebug.Logger.LogError("OnExit:" + NodeId);
        }

        //执行 Execute 的前置方法，在 Execute() 方法的第一行调用
        public void Preposition()
        {
            if (nodeStatus == NODE_STATUS.READY)
            {
                nodeStatus = NODE_STATUS.RUNNING;
                OnEnter();
            }
        }

        /// <summary>
        ///  执行 Execute 的后置方法，在 Execute() 方法的 returen 前调用
        /// </summary>
        public void Postposition(ResultType resultType)
        {
            if (resultType != ResultType.Running)
            {
                nodeStatus = NODE_STATUS.READY;
                OnExit();
            }
        }


    }
}
