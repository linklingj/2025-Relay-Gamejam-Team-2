using UnityEngine;

public class RandomDamage : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage((int)Random.Range(1,value));
    }
}

//처형
public class ExecutionDamage : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        var f = targetUnit.GetStatHp().maxValue / value;
        if(targetUnit.GetStatHp().baseValue <= f) targetUnit.OnDamage(targetUnit.GetStatHp().baseValue);
    }
}