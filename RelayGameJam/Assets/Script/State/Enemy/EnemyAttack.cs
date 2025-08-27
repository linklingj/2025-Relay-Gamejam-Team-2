using UnityEngine;

public class EnemyAttack : MonoBehaviour,IState<EnemyController>
{
    private IState<EnemyController> _enemyController;

    public void OperateEnter(EnemyController sender)
    {
        
    }

    public void OperateUpdate(EnemyController sender)
    {
       
    }

    public void OperateExit(EnemyController sender)
    {
       
    }
}
