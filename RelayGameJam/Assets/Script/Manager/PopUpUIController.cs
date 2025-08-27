using DamageNumbersPro;
using UnityEngine;

public class PopUpUIController : Singleton<PopUpUIController>
{
    public DamageNumber numberPrefab;
    
    public void SpawnDamagePopUp(float value,Transform target) //데미지 입을 값 + 피격자의 Transform 넣으면 데미지 표시가 나옵니다
    {
        DamageNumber damageNumber = numberPrefab.Spawn(target.position+new Vector3(0,2f), value);
    }
}
