namespace CardData
{
    public class GhostSkill : SkillBase
    {
        public GhostSkill(Data data) : base(data)
        {
        }

        public override void SkillAction(Unit targetUnit, Unit ownerUnit)
        {
            throw new System.NotImplementedException();
        }
    }
}