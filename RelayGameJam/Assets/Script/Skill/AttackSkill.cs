using CardData;
using UnityEngine;

public class AttackSkill : SkillBase
{

    public AttackSkill(Data data) : base(data){}


    public override void SkillAction(Unit targetUnit,Unit ownerUnit)
    {
        targetUnit.OnDamage(data.Value);
        var effect = Resources.Load("Effect/Attack");
        var a = GameObject.Instantiate(effect,targetUnit.transform.position + new Vector3(0,1.5f),Quaternion.identity);
        DetailAction(targetUnit, ownerUnit);
        GameObject.Destroy(a,2f);
        //공격
    }
}
