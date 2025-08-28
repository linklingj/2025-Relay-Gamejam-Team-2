using System;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Idle,Attack
}

public interface IChangeState
{
    public void ChangeState(State newState);
    public State GetState();
}
public class CharacterController<T> : MonoBehaviour,IChangeState where T : CharacterController<T>
{
    protected Dictionary<State,IState<T>> m_states = new Dictionary<State, IState<T>>();
    protected StateMachine<T> m_stateMachine;
    protected State curState;
    protected Unit unit;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }
    
    public Unit GetUnit() => unit;


    protected virtual void Update()
    {
        m_stateMachine.DoOperateUpdate();
    }
    

    public void ChangeState(State newState)
    {
        m_stateMachine.SetState(m_states[newState]);
        curState = newState;
    }

    public State GetState()
    {
        return curState;
    }

    protected void AddStates(State state, IState<T> stateInstance)
    {
        if(!m_states.ContainsKey(state))m_states.Add(state,stateInstance);
    }
}
