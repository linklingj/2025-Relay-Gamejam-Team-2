using UnityEngine;

public class Heal : SkillDetails
{
    public override void DetailAction(Unit targetUnit,Unit ownerUnit)
    {
        if(ownerUnit == null)Debug.LogError("Unit is null");
        ownerUnit.OnDamage(-value);
    }
}
