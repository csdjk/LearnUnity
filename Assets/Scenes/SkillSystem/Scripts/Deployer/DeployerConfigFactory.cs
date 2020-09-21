using System;
using System.Reflection;
using UnityEngine;

namespace SkillSystem {
    /// <summary>
    /// 释放器配置工厂: 提供创建释放器各种算法对象的功能.
    /// 作用: 将对象的创建 与 使用分离
    /// </summary>
    public class DeployerConfigFactory {

        /// <summary>
        /// 创建选区对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IAttackSelector CreateAttackSelector (SkillData data) {
            // 命名规则:  SkillSystem. + 枚举名 + AttackSelector
            string className = $"SkillSystem.{data.selectorType}AttackSelector";
            return CreateObject<IAttackSelector> (className);
        }

        /// <summary>
        /// 创建影响效果对象
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static IImpactEffect[] CreateImpactEffects (SkillData data) {
            ImpactType[] impactTypes = data.impactType;
            IImpactEffect[] impacts = new IImpactEffect[impactTypes.Length];
            for (int i = 0; i < impactTypes.Length; i++) {
                ImpactType impactType = impactTypes[i];
                string classNameImpact = $"SkillSystem.{impactType}ImpactEffect";
                impacts[i] = CreateObject<IImpactEffect> (classNameImpact);
            }
            return impacts;
        }

        private static T CreateObject<T> (string className) where T : class {
            Type type = Type.GetType (className);
            if (type == null) {
                Debug.LogError ($"释放器配置工厂 创建对象失败! 请检查该类是否存在: {className}，请规范命名！");
                return null;
            }
            return Activator.CreateInstance (type) as T;
        }
    }
}