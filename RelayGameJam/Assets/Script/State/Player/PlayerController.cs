using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class PlayerController : CharacterController<PlayerController>
{
    private void Awake()
    {
        IState<PlayerController> Idle = new PlayerIdle();
        IState<PlayerController> Attack = new PlayerAttack();
        AddStates(State.Idle,Idle);
        AddStates(State.Attack,Attack);
        
        m_stateMachine = new StateMachine<PlayerController>(this,m_states[State.Idle]);
    }




    protected override void Update()
    {
        base.Update();
        if(Input.GetKeyDown(KeyCode.F1))ChangeState(State.Idle);
        if(Input.GetKeyDown(KeyCode.F2))ChangeState(State.Attack);
    }
}
