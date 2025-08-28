using UnityEngine;

public class Heal : SkillDetails
{
    public override void DetailAction(Unit targetUnit,Unit ownerUnit)
    {
        ownerUnit.OnDamage(-value);
    }
}

