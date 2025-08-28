using UnityEngine;

public class IncreaseMana : SkillDetails
{
    public override void DetailAction(Unit targetUnit,Unit ownerUnit)
    {
        PlayerCardController.Inst.AddMana((int)value);
    }
}
