using System;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Common;
using UnityEngine;

using ResourceManager = Common.ResourceManager;

namespace SkillSystem {
    /// <summary>
    /// 技能管理器
    /// </summary>
    public class CharacterSkillManager : MonoBehaviour {
        public SkillData[] skills;

        private void Start () {
            for (int i = 0; i < skills.Length; i++)
                InitSkill (skills[i]);

        }

        /// <summary>
        /// 初始化技能
        /// </summary>
        /// <param name="data"></param>
        private void InitSkill (SkillData data) {
            data.skillPrefab = ResourceManager.Load<GameObject> (data.prefabName);

            data.owner = gameObject;
        }

        /// <summary>
        /// 准备技能
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SkillData PrepareSkill (int id) {
            // 查找技能
            SkillData data = Find (s => s.skillID == id);

            Debug.Log ("准备技能: " + data.name);

            float sp = GetComponent<CharacterStatus> ().SP;
            if (data != null && data.coolRemain <= 0 && data.costSP <= sp)
                return data;

            if (data.coolRemain > 0)
                Debug.LogWarning ($"【{data.name}】技能还未冷却,剩余时间:{data.coolRemain}" );
            if (data.costSP > sp)
                Debug.LogWarning ($"剩余魔法值太少, 不够是否该技能: sp: {sp}");

            return null;
        }

        /// <summary>
        /// 查找技能
        /// </summary>
        /// <param name="handler"></param>
        /// <returns></returns>
        private SkillData Find (Func<SkillData, bool> handler) {
            for (int i = 0; i < skills.Length; i++) {
                if (handler (skills[i]))
                    return skills[i];
            }
            return null;
        }

        /// <summary>
        /// 生成技能
        /// </summary>
        public void GenerateSkill (SkillData data) {
            Debug.Log ($"生成技能:{data.name}持续时间:{data.durationTime}");
            // GameObject skillGo = Instantiate (data.skillPrefab);
            // skillGo.transform.position = Vector3.forward ;

            // 采用对象池生成技能
            GameObject skillGo = GameObejctPool.Instance.CreateObject (data.prefabName, data.skillPrefab, transform.position, transform.rotation);

            // 释放器
            SkillDeployer deployer = skillGo.GetComponent<SkillDeployer> ();
            deployer.SkillData = data;
            deployer.DeploySkill ();
            // 销毁技能
            // Destroy (skillGo, data.durationTime);

            // 回收技能
            GameObejctPool.Instance.CollectObejct (skillGo, data.durationTime);

            // 开启技能冷却
            StartCoroutine ("CollTimeDown", data);
        }

        /// <summary>
        /// 技能冷却
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private IEnumerable CollTimeDown (SkillData data) {
            data.coolRemain = data.coolTime;
            while (data.coolRemain > 0) {
                yield return new WaitForSeconds (1);
                data.coolRemain--;
            }
        }
    }
}