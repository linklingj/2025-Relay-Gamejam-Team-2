using UnityEngine;

public class PlayerIdle : IState<PlayerController>
{
    private PlayerController _playerController;
    
    public void OperateEnter(PlayerController sender)
    {
        Debug.Log("플레이어 Idle상태에 진입");
        _playerController = sender;
    }

    public void OperateUpdate(PlayerController sender)
    {
        
    }

    public void OperateExit(PlayerController sender)
    {
        
    }
}
