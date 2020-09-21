using UnityEngine;
using Common;

namespace SkillSystem {

    public class CharacterSkillSystem : MonoBehaviour {
        private CharacterSkillManager skillManager;
        private Animator anim;
        private void Awake () {
            skillManager = GetComponent<CharacterSkillManager> ();
            anim = GetComponentInChildren<Animator> ();
            // GetComponentInChildren<AnimationEventBehaviour>().;
        }
    
        public void AttackUseSkill (int skillID) {
            // 准备技能
            SkillData skill = skillManager.PrepareSkill (skillID);
            if (skill == null) return;

            // 播放动画
            anim.SetBool (skill.animationName,true);
            // 生成技能
            skillManager.GenerateSkill(skill);
        }
        // 随机使用技能(NPC)
        public void UseRandomSkill () {
            SkillData[] usableSkills = skillManager.skills.FindAll (s => skillManager.PrepareSkill (s.skillID) != null);
            if (usableSkills.Length == 0) return;

            int index = Random.Range (0, usableSkills.Length);
            AttackUseSkill (usableSkills[index].skillID);

        }
    }
}