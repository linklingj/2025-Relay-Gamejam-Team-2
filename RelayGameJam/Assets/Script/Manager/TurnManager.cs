using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

public class TurnManager : Singleton<TurnManager>
{
    
    [SerializeField] private Button turnEndBtn; // 턴 종료 버튼
   
    [SerializeField] private Transform playerSpawnTrans, enemySpawnTrans; //플레이어 팀과 적 팀의 소환 위치
    [SerializeField] private float spawnRange = 6f; //플레이어 , 적 팀 소환 반경
    
    [Foldout("Debug")]
    public Unit player; //현재 플레이어 팀들을 저장
    [SerializeField] private List<Unit> enemyTeams; //현재 적 팀의 캐릭터들을 저장
    
    [SerializeField] private Unit playerUnit,enemyUnit;

    private int enemyAttackCount = 0;

    private void Start()
    {
        turnEndBtn.onClick.AddListener(()=>PlayerTurnEnd()); //스크립트에서 turnEndBtn의 클릭 이벤트를 추가
        turnEndBtn.onClick.AddListener(() => PlayerCardController.Inst.ResetMana()); //마나 초기화 함수
        
        SpawnUnits(UnitData.Data.DataList[DataManager.Inst.Data.playerCharacterId],true);
        SpawnUnits(UnitData.Data.DataList[1],false);
        ChangeUnitState(player,State.Attack);
    }

    #region Spawn
    private void SpawnUnits(UnitData.Data unitData, bool isPlayer)
    {
        List<UnitData.Data> unitDatas = new List<UnitData.Data>(){unitData};
        SpawnUnits(unitDatas, isPlayer);
    }

    //플레이어 캐릭터와 적 캐릭터들을 소환
    private void SpawnUnits (List<UnitData.Data> unitDatas,bool isPlayerTeam)
    {
        for (int i = 0; i < unitDatas.Count; i++)
        {
            //보간을 위한 비율 나누기
            //ex) 혼자일 때 0.5f(중간거리) 두명일 때 0.25f,0.75f(25%거리, 75%거리 위치) 세명 이상 부터는 꽉 채워서
            float t = unitDatas.Count == 1?0.5f:unitDatas.Count ==2?0.25f+0.5f*i: (float)i / (float)(unitDatas.Count-1);
            //보간을 통해 x위치를 구함
            float xPos = Mathf.Lerp(-spawnRange/2, spawnRange/2, t);
            //플레이어 팀 진영인지 확인
            Transform trans = isPlayerTeam ? playerSpawnTrans : enemySpawnTrans;
            //소환
            var unit = Instantiate(isPlayerTeam?playerUnit:enemyUnit, trans.position + new Vector3(xPos,0), Quaternion.identity);
            unit.Init(unitDatas[i]);
            
            //각각에 맞는 팀에 추가
            if (isPlayerTeam)
            {
                player = unit;
            }
            else
            {
                enemyTeams.Add(unit);
            }
        }
    }
    #endregion

    public void CheckEnemy(Transform transform) //현재 적이 존재하는 지 확인하는 코드입니다.
    {
        enemyTeams.Remove(transform.GetComponent<Unit>()); //적 팀에서 죽은 적을 뺍니다.
        if(enemyTeams.Count <= 0)Debug.Log("라운드 종료"); //적의 개수가 0이라면 라운드를 종료합니다.
    }
    
    
    #region Turn and State
    //팀의 모든 캐릭터들의 State를 변경
    private void ChangeUnitState (List<Unit> units, State state)
    {
        foreach (var unit in units)unit.GetIChangeState().ChangeState(state);
    }
    
    private void ChangeUnitState (Unit unit, State state)
    {
        unit.GetIChangeState().ChangeState(state);
    }

    private void PlayerTurnEnd()
    {
        //플레이어 팀의 모든 캐릭터를 IdleState상태로
        ChangeUnitState(player, State.Idle);
        //적 팀의 캐릭터들이 차례대로 공격을 시전
        ChangeUnitState(enemyTeams, State.Attack);
    }

    public void EnemyTurnEnd()
    {
        enemyAttackCount += 1;
        if (enemyAttackCount >= enemyTeams.Count)
        {
            ChangeUnitState(enemyTeams, State.Idle);
            ChangeUnitState(player, State.Attack);
            enemyAttackCount = 0;
        }
    }
    
    #endregion
}
