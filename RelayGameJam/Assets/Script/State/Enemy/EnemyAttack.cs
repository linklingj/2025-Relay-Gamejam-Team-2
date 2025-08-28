using CardData;
using UnityEngine;

public class EnemyAttack : IState<EnemyController>
{
    private EnemyController _enemyController;

    public void OperateEnter(EnemyController sender)
    {
        _enemyController = sender;
        var skill = CardDataManager.Inst.GetRandomSkill();
        Debug.Log(skill.data.CardName+" 스킬을 사용했습니다");
        skill.SkillAction(TurnManager.Inst.player,_enemyController.GetUnit());//임시 적용
        _enemyController.ChangeState(State.Idle);
        TurnManager.Inst.EnemyTurnEnd();
    }

    public void OperateUpdate(EnemyController sender)
    {
       
    }

    public void OperateExit(EnemyController sender)
    {
       
    }
}
