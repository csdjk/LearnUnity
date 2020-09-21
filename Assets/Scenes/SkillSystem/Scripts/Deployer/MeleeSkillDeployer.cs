namespace SkillSystem
{
    public class MeleeSkillDeployer:SkillDeployer
    {
        public override void DeploySkill(){
            // 执行选区算法
            CalculateTargets();
            // 执行影响算法
            ImpactTargets();
        }
    }
}