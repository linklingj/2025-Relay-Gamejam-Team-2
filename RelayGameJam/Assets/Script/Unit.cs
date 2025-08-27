using System;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private Animator animator;
    private Stat hp,mana;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        hp = Stat.Create(10);
        mana = Stat.Create(3);
    }

    public void Init()
    {
        
    }

    public void OnDamage(float value)
    {
        hp.AddValue(-value);
        PopUpUIController.Inst.SpawnDamagePopUp(value,transform);
    }
    
    
    
}
