using System;
using System.Collections;
using UnityEngine;

namespace SkillSystem
{
    /// <summary>
    /// 法力消耗
    /// </summary>
    public class DamageImpactEffect : IImpactEffect
    {
        private SkillData data;
        public void Execute(SkillDeployer deployer)
        {
            data = deployer.SkillData;
            deployer.StartCoroutine(RepeatDamage(deployer));
        }

        /// <summary>
        /// 多次攻击
        /// </summary>
        /// <param name="deployer"></param>
        /// <returns></returns>
        private IEnumerator RepeatDamage(SkillDeployer deployer){
            float atkTime = 0;
            do
            {
                OnceDamage();
                yield return new WaitForSeconds(data.atkInterval);
                atkTime += data.atkInterval;
                // 重新计算攻击目标
                deployer.CalculateTargets();

            } while (atkTime < data.durationTime);
        }

        private void OnceDamage()
        {
            // 技能攻击力: 攻击比率 * 基础攻击力
            float atk = data.atkRatio * data.owner.GetComponent<CharacterStatus>().baseATK;
            for (int i = 0; i < data.attackTargets.Length; i++)
            {
                var status = data.attackTargets[i].GetComponent<CharacterStatus>();
                status.Damage(atk);
            }
        }
    }
}