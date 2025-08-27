using UnityEngine;

public class PlayerAttack : IState<PlayerController>
{
    private PlayerController _playerController;
    public void OperateEnter(PlayerController sender)
    {
        _playerController = sender;
        Debug.Log("플레이어 공격턴");
       
    }

    public void OperateUpdate(PlayerController sender)
    {
        //카드를 드로우 및 사용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _playerController.ChangeState(State.Idle);
        }
    }

    public void OperateExit(PlayerController sender)
    {
        
    }
}
