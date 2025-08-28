using UnityEngine;

public class EnergyCharge : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        ownerUnit.GetStatEnergy().AddValue(value);
    }
}

public class EnergyEvolve : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage((ownerUnit.GetStatEnergy().baseValue*value) +5);
        ownerUnit.GetStatEnergy().SetBaseValue(0);
    }
}