namespace SkillSystem
{
    /// <summary>
    /// 法力消耗
    /// </summary>
    public class CostSPImpactEffect : IImpactEffect
    {
        public void Execute(SkillDeployer deployer)
        {
            var characterStatus = deployer.SkillData.owner.GetComponent<CharacterStatus>();
            characterStatus.SP -= deployer.SkillData.costSP;
        }
    }
}