
using System;
using UnityEngine;
using System.Collections;
using System.ComponentModel;

/// <summary>
/// 行为树节点类型
/// </summary>
public enum NODE_TYPE
{
    /// <summary>
    /// 选择节点
    /// </summary>
    [Description("选择节点")]
    SELECT = 0,

    /// <summary>
    /// 顺序节点
    /// </summary>
    [Description("顺序节点")]
    SEQUENCE = 1,

    /// <summary>
    /// 随机节点
    /// </summary>
    [Description("随机节点")]
    RANDOM = 2,

    /// <summary>
    /// 随机顺序节点
    /// </summary>
    [Description("随机顺序节点")]
    RANDOM_SEQUEUECE = 3,

    /// <summary>
    /// 随机权重节点
    /// </summary>
    [Description("随机权重节点")]
    RANDOM_PRIORITY = 4,

    /// <summary>
    /// 并行节点
    /// </summary>
    [Description("并行节点")]
    PARALLEL = 5,

    /// <summary>
    /// 并行选择节点
    /// </summary>
    [Description("并行选择节点")]
    PARALLEL_SELECT = 6,

    /// <summary>
    /// 并行执行所有节点
    /// </summary>
    [Description("并行执行所有节点")]
    PARALLEL_ALL = 7,

    /// <summary>
    /// IF 判断节点
    /// </summary>
    [Description("IF 判断节点")]
    IF_JUDEG = 8,

    /// <summary>
    /// 修饰节点_取反
    /// </summary>
    [Description("修饰节点_取反")]
    DECORATOR_INVERTER = 100,

    /// <summary>
    /// 修饰节点_重复
    /// </summary>
    [Description("修饰节点_重复")]
    DECORATOR_REPEAT = 101,

    /// <summary>
    /// 修饰节点_返回Fail
    /// </summary>
    [Description("修饰_返回Fail")]
    DECORATOR_RETURN_FAIL = 102,

    /// <summary>
    /// 修饰节点_返回Success
    /// </summary>
    [Description("修饰_返回Success")]
    DECORATOR_RETURN_SUCCESS = 103,

    /// <summary>
    /// 修饰节点_直到Fail
    /// </summary>
    [Description("修饰_直到Fail")]
    DECORATOR_UNTIL_FAIL = 104,

    /// <summary>
    /// 修饰节点_直到Success
    /// </summary>
    [Description("修饰_直到Success")]
    DECORATOR_UNTIL_SUCCESS = 105,

    /// <summary>
    /// 条件节点
    /// </summary>
    [Description("条件节点")]
    CONDITION = 200,

    /// <summary>
    /// 行为节点
    /// </summary>
    [Description("行为节点")]
    ACTION = 300,

    /// <summary>
    /// 子树
    /// </summary>
    [Description("子树")]
    SUB_TREE = 1000,
}
namespace BehaviorTree
{
    /// <summary>
    /// 节点执行结果
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// 失败
        /// </summary>
        Fail = 0,

        /// <summary>
        /// 成功
        /// </summary>
        Success = 1,

        /// <summary>
        /// 执行中
        /// </summary>
        Running = 2,
    }

     public enum NODE_STATUS
    {
        /// <summary>
        /// 准备
        /// </summary>
        READY = 0,

        /// <summary>
        /// 运行中
        /// </summary>
        RUNNING = 1,

        /// <summary>
        /// 执行中
        /// </summary>
        Running = 2,
    }
}
