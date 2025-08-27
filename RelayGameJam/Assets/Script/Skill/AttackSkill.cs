using CardData;
using UnityEngine;

public class AttackSkill : SkillBase
{

    public AttackSkill(Data data) : base(data){}


    public override void SkillAction(Unit unit)
    {
        unit.OnDamage(data.Value);
        var effect = Resources.Load("Effect/Attack");
        var a = GameObject.Instantiate(effect,unit.transform.position + new Vector3(0,1.5f),Quaternion.identity);
        DetailAction(unit);
        GameObject.Destroy(a,2f);
        //공격
    }
}
