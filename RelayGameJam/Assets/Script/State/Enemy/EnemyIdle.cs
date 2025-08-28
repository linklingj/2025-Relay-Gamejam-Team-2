using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdle : MonoBehaviour,IState<EnemyController>
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
