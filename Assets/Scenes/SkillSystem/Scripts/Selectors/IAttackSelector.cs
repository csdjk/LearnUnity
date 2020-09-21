using UnityEngine;

namespace SkillSystem {
    /// <summary>
    /// 攻击选区接口
    /// </summary>
    public interface IAttackSelector {
        // 目标
        Transform[] SelectTarget(SkillData data,Transform skillTF);
    }
}