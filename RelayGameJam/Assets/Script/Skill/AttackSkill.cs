using CardData;
using UnityEngine;

public class AttackSkill : SkillBase
{
    public AttackSkill(Data data) : base(data){}
    
    public override void SkillAction(Unit targetUnit,Unit ownerUnit)
    {
        if (data.Value != 0)
        {
            targetUnit.OnDamage(data.Value);
            var effect = Resources.Load("Effect/Attack"); //이펙트 소환
            var a = GameObject.Instantiate(effect,targetUnit.transform.position + new Vector3(0,1.5f),Quaternion.identity);
            GameObject.Destroy(a,2f);
        }
       
        DetailAction(targetUnit, ownerUnit);
    }
}
