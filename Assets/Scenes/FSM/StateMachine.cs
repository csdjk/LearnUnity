using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateMachine
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public StateMachine()
    {
        mStateDic = new Dictionary<uint, IState>();
        CurState = null;
    }

    /// <summary>
    /// 所有状态集合
    /// </summary>
    private Dictionary<uint, IState> mStateDic = null;
    
    /// <summary>
    /// 当前正在执行的状态
    /// </summary>
    public IState CurState
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前的状态id
    /// </summary>
    public uint CurStateId
    {
        get
        {
            return CurState == null ? 0 : CurState.StateId;
        }
    }

    /// <summary>
    /// 注册一个状态
    /// </summary>
    /// <param name="state">状态</param>
    /// <returns>成功还是失败</returns>
    public bool RegisterState(IState state)
    {
        bool ret = false;
        if (state != null && !mStateDic.ContainsKey(state.StateId))
        {
            mStateDic[state.StateId] = state;
            ret = true;
        }
        else
        {
            if (state == null)
            {
                Debug.LogError("注册状态 不能为空");
            }
            else
            {
                Debug.LogError("这个状态已经注册过了 不能重复注册" + state.StateId);
            }
            ret = false;
        }
        return ret;
    }

    /// <summary>
    /// 移除一个状态
    /// </summary>
    /// <param name="stateId">状态id</param>
    /// <returns></returns>
    public bool RemoveState(uint stateId)
    {
        if (mStateDic.ContainsKey(stateId))
        {
            mStateDic.Remove(stateId);
            if (CurState != null && CurState.StateId == stateId) CurState = null;
            return true;
        }
        return false;
    }

    /// <summary>
    /// 获取一个状态
    /// </summary>
    /// <param name="stateId">状态id</param>
    /// <returns>状态实体</returns>
    public IState GetState(uint stateId)
    {
        IState state = null;
        mStateDic.TryGetValue(stateId,out state);
        return state;
    }

    /// <summary>
    /// 停止当前状态
    /// </summary>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    public void StopState(object param1 = null,object param2 = null)
    {
        if (null != CurState)
        {
            CurState.OnLeave(null, param1, param2);
            CurState = null;
        }
    }
    /// <summary>
    /// 状态切换的回调 类型
    /// </summary>
    /// <param name="from">当前的状态</param>
    /// <param name="to">目标状态</param>
    /// <param name="param1"></param>
    /// <param name="param2"></param>
    public delegate void BetweenSwitchState(IState from, IState to, object param1, object param2);

    /// <summary>
    /// 切换状态的回调
    /// </summary>
    public BetweenSwitchState BetweenSwitchStateCallBack = null;

    public bool SwitchState(uint newStateId,object param1 = null,object param2 = null)
    {
        bool ret = false;
        if (CurState != null && CurState.StateId == newStateId)
        {
            ret = false;
        }
        else
        {
            IState newState = null;
            mStateDic.TryGetValue(newStateId, out newState);
            if (newState != null)
            {
                IState oldState = CurState;
                CurState = newState;

                if (oldState != null)
                oldState.OnLeave(newState, param1, param2);

                if (BetweenSwitchStateCallBack != null)
                {
                    BetweenSwitchStateCallBack(oldState, newState, param1, param2);
                }

                CurState.OnEnter(oldState, param1, param2);
                ret = true;

            }
            else
            {
                ret = false;
            }
        }
        
        return ret;
    }

    /// <summary>
    /// 当前状态是否是某个状态
    /// </summary>
    /// <param name="stateId">状态id</param>
    /// <returns>是？</returns>
    public bool InState(uint stateId)
    {
        return CurState != null && CurState.StateId == stateId;
    }

    public void Update()
    {
        if (CurState != null) CurState.OnUpdate();
    }

    public void FixedUpdate()
    {
        if (CurState != null) CurState.OnFixedUpdate();
    }

    public void LateUpdate()
    {
        if (CurState != null) CurState.OnLateUpdate();
    }

    public void Release()
    {
       foreach (IState state in mStateDic.Values)
       {
            state.OnRelease();
       }
        mStateDic.Clear();
        StopState();
        mStateDic = null;
        CurState = null;
    }
}