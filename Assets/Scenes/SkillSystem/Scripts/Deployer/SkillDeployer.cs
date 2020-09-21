using System;
using UnityEngine;
namespace SkillSystem {
    public abstract class SkillDeployer : MonoBehaviour {

        private SkillData skillData;
        private IAttackSelector selector;
        private IImpactEffect[] impactEffects;

        public SkillData SkillData {
            get {
                return skillData;
            }
            set {
                skillData = value;
                InitDeployer ();
            }
        }

        // 创建算法对象
        private void InitDeployer () {
            // 选区
            selector = DeployerConfigFactory.CreateAttackSelector (skillData);
            // 影响
            impactEffects = DeployerConfigFactory.CreateImpactEffects (skillData);

        }

        // 执行算法对象
        public void CalculateTargets () {
            skillData.attackTargets = selector.SelectTarget (skillData, transform);
            //测试
            // foreach (var item in skillData.attackTargets) {
            //     Debug.Log ("攻击目标: " + item.name);
            // }
        }

        public void ImpactTargets(){
            for (int i = 0; i < impactEffects.Length; i++)
            {
                impactEffects[i].Execute(this);
            }
        }


        // 释放方式
        // 供技能管理器调用,由子类实现,定义具体释放策略
        public abstract void DeploySkill ();


    }
}