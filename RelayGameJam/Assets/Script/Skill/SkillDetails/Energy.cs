using UnityEngine;

public class EnergyCharge : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        ownerUnit.GetStatGhost().AddValue(value);
    }
}

public class EnergyEvolve : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage((ownerUnit.GetStatGhost().baseValue*value) +5);
        ownerUnit.GetStatGhost().SetBaseValue(0);
    }
}