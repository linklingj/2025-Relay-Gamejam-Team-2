using CardData;
using UnityEngine;

public class AttackSkill : SkillBase
{

    public AttackSkill(Data data) : base(data){}


    public override void SkillAction(Unit unit)
    {
        unit.OnDamage(data.Value);
        //공격
    }
}
