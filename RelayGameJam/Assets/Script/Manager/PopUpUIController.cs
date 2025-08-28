using DamageNumbersPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PopUpUIController : Singleton<PopUpUIController>
{
    [SerializeField] private DamageNumber damagePrefab,healPrefab;
    
    public void SpawnDamagePopUp(float value,Transform target) //데미지 입을 값 + 피격자의 Transform 넣으면 데미지 표시가 나옵니다
    {
        if (value == 0) return;
        if(value >= 0) damagePrefab.Spawn(target.position+new Vector3(0,2f), value);
        else healPrefab.Spawn(target.position+new Vector3(0,2f), Mathf.Abs(value));
    }
}
