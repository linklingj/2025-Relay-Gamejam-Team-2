using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyController : CharacterController<EnemyController>
{
    protected override void Awake()
    {
        base.Awake();
        IState<EnemyController> Idle = new EnemyIdle();
        IState<EnemyController> Attack = new EnemyAttack();
        
        AddStates(State.Idle, Idle);
        AddStates(State.Attack,Attack);
       
        m_stateMachine = new StateMachine<EnemyController>(this,m_states[State.Idle]);
    }


    protected override void Update()
    {
        base.Update();

    }
    
}
