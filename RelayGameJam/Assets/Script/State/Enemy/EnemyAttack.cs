using CardData;
using UnityEngine;

public class EnemyAttack : IState<EnemyController>
{
    private EnemyController _enemyController;

    public void OperateEnter(EnemyController sender)
    {
        _enemyController = sender;
        // TODO : ghost 유닛별로 데이터를 불러와 해당 스킬 실행하도록 수정
        var skill = SkillFactory.Inst.GetSkill(0);
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
