using CardData;
using UnityEngine;

public class UtilitySkill : SkillBase
{
    public UtilitySkill(Data data) : base(data) { }

    public override void SkillAction(Unit targetUnit,Unit ownerUnit)
    {
        DetailAction(targetUnit,ownerUnit);
    }
}
