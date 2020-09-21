using System.Collections.Generic;
using Common;
using UnityEngine;

namespace SkillSystem {
    /// <summary>
    /// 扇形/圆形选区
    /// </summary>
    public class SectorAttackSelector : IAttackSelector {
        List<Transform> targets = new List<Transform> ();
        public Transform[] SelectTarget (SkillData data, Transform skillTF) {

            for (int i = 0; i < data.attackTargetTags.Length; i++) {
                GameObject[] tempGoArrary = GameObject.FindGameObjectsWithTag (data.attackTargetTags[i].ToString ());
                targets.AddRange (tempGoArrary.Select (g => g.transform));
            }
            Debug.Log ($"技能起始位置:{skillTF.position}");

            // 判断攻击范围
            targets = targets.FindAll (t =>
                Vector3.Distance (t.position, skillTF.position) < data.attackDistance &&
                Vector3.Angle (skillTF.forward, t.position - skillTF.position) < data.attackAngle
            );

            // 筛选出活的
            targets = targets.FindAll (t => t.GetComponent<CharacterStatus> ().HP > 0);

            Transform[] result = targets.ToArray ();
            // 返回目标(群攻)
            if (data.attackType == SkillAttackType.Group)
                return result;
            // 单攻
            Transform min = result.GetMin (t => Vector3.Distance (t.position, skillTF.position));

            return new Transform[] { min };
        }

    }
}