using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Animator animator;
    private Stat hp,mana;
    private UnitData.Data data;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    
    public void Init(UnitData.Data data)
    {
        this.data = data;
        hp = Stat.Create(data.Hp);
        mana = Stat.Create(data.Mana);
    }

    public void OnDamage(float value)
    {
        hp.AddValue(-value);
        PopUpUIController.Inst.SpawnDamagePopUp(value,transform);
    }
    
    
    
}
