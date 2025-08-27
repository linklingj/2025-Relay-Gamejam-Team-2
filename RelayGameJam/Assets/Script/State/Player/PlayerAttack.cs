using UnityEngine;

public class PlayerAttack : IState<PlayerController>
{
    private PlayerController _playerController;
    public void OperateEnter(PlayerController sender)
    {
        _playerController = sender;
        PlayerCardController.Inst.GetCards();
        Debug.Log("플레이어 공격턴");
       
    }

    public void OperateUpdate(PlayerController sender)
    {
    }

    public void OperateExit(PlayerController sender)
    {
        
    }
}
