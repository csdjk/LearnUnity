using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[Serializable]
public class SkillData {
    public int skillID;
    public string name;
    ///技能描述
    public string description;
    ///冷却时间
    public int coolTime;

    ///冷却剩余
    public int coolRemain;

    ///魔法消耗
    public int costSP;
    //攻击距离
    public float attackDistance;
    // 攻击角度
    public float attackAngle;

    // 攻击目标tags
    public AttackTargetType[] attackTargetTags;
    // 攻击目标对象数组
    [HideInInspector]
    public Transform[] attackTargets;
    // 技能影响类型
    public ImpactType[] impactType;
    // 连击的下一个技能编号
    public int nextBatterId;
    // 伤害比率
    public float atkRatio;

    // 持续时间
    public float durationTime;

    // 伤害间隔
    public float atkInterval;

    // 技能所属
    [HideInInspector]
    public GameObject owner;
    // 技能预制体名字
    public string prefabName;
    // 预制体
    [HideInInspector]
    public GameObject skillPrefab;
    // 动画名字
    public string animationName;

    // 受击特效名字
    public string hitFxName;

    // 受击特效预制体
    [HideInInspector]
    public GameObject hitFxPrefab;

    // 技能等级
    public int level;

    // 攻击类型 单攻 群攻
    public SkillAttackType attackType;

    // 选择类型 扇形(圆形) 矩形
    public SelectorType selectorType;
}