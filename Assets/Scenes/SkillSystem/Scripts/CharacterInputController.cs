using SkillSystem;
using UnityEngine;

public class CharacterInputController : MonoBehaviour {

    private CharacterSkillSystem skillSystem;

    private void Awake () {
        skillSystem = GetComponent<CharacterSkillSystem> ();

    }

    public void OnSkillButtonDown () {
        // CharacterSkillManager skillManager = GetComponent<CharacterSkillManager> ();
        // SkillData data = skillManager.PrepareSkill (1001);
        // if (data != null)
        //     skillManager.GenerateSkill (data);
        skillSystem.AttackUseSkill(1001);
    }
}