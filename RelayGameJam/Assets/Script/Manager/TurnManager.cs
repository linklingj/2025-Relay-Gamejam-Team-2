using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VInspector;

public class TurnManager : MonoBehaviour
{
    
    [SerializeField] private Button turnEndBtn; // 턴 종료 버튼
   
    [SerializeField] private Transform playerSpawnTrans, enemySpawnTrans; //플레이어 팀과 적 팀의 소환 위치
    [SerializeField] private float spawnRange = 6f; //플레이어 , 적 팀 소환 반경
    
    [Foldout("Debug")]
    [SerializeField] private List<PlayerController> playerTeams; //현재 플레이어 팀들을 저장
    [SerializeField] private List<EnemyController> enemyTeams; //현재 적 팀의 캐릭터들을 저장
    
    [SerializeField] private Unit playerUnit,enemyUnit;

    private void Start()
    {
        turnEndBtn.onClick.AddListener(()=>PlayerTurnEnd()); //스크립트에서 turnEndBtn의 클릭 이벤트를 추가
        
        SpawnUnits(UnitData.Data.DataList[DataManager.Inst.Data.playerCharacterId],true);
        SpawnUnits(UnitData.Data.DataList[1],false);
    }

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
                if(unit.TryGetComponent(out PlayerController _player))
                    playerTeams.Add(_player);
            }
            else
            {
                if(unit.TryGetComponent(out EnemyController _enemy))
                    enemyTeams.Add(_enemy);
            }
        }
    }

    //팀의 모든 캐릭터들의 State를 변경
    private void ChangeUnitState<T>(List<T> units, State state) where T : IChangeState
    {
        foreach (var unit in units)unit.ChangeState(state);
    }

    private void PlayerTurnEnd()
    {
        //플레이어 팀의 모든 캐릭터를 IdleState상태로
        ChangeUnitState(playerTeams, State.Idle);
        //적 팀의 캐릭터들이 차례대로 공격을 시전
        ChangeUnitState(enemyTeams, State.Attack);
    }

    private void EnemyTurnEnd()
    {
        ChangeUnitState(enemyTeams, State.Idle);
        ChangeUnitState(playerTeams, State.Attack);
    }
}
