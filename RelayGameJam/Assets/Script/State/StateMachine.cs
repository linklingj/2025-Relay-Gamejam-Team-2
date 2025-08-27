using UnityEngine;

public class StateMachine<T>
{
    private T m_sender;

    public IState<T> CurState { get; set; }

    public StateMachine(T sender, IState<T> state)
    {
        m_sender = sender;
        SetState(state);
    }

    public void SetState(IState<T> state)
    {
        // null에러출력
        if (m_sender == null)
        {
            Debug.LogError("m_sender ERROR");
            return;
        }

        if (CurState == state)
        {
            Debug.LogWarningFormat("Same state : ", state);
            return;
        }

        if (CurState != null)
            CurState.OperateExit(m_sender);
        CurState = state;
        CurState.OperateEnter(m_sender);
    }
    
    //State용 Update 함수.
    public void DoOperateUpdate()
    {
        if (m_sender == null)
        {
            Debug.LogError("invalid m_sener");
            return;
        }
        CurState.OperateUpdate(m_sender);
    }
}
