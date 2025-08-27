using UnityEngine;

public class Heal : SkillDetails
{
    public override void DetailAction(Unit targetUnit)
    {
        targetUnit.OnDamage(-value);
    }
}
