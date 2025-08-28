using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;
using Random = UnityEngine.Random;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private Button turnEndBtn; // 턴 종료 버튼
   
    [SerializeField] private Transform playerSpawnTrans, enemySpawnTrans; //플레이어 팀과 적 팀의 소환 위치
    [SerializeField] private float spawnRange = 6f; //플레이어 , 적 팀 화면상에 생성 반경
    
    [Foldout("Debug")]
    public Unit player; //현재 플레이어 팀들을 저장
    [SerializeField] private List<Unit> enemyTeams; //현재 적 팀의 캐릭터들을 저장
    
    [SerializeField] private GameObject playerUnit,enemyUnit;
    
    private int enemyAttackCount = 0;   // 현재 턴 내 적 공격 횟수

    private void Awake()
    {
        // 턴 종료 버튼 연결
        turnEndBtn.onClick.AddListener(()=>PlayerTurnEnd()); // 플레이어 턴 종료
        turnEndBtn.onClick.AddListener(() => PlayerCardController.Inst.ResetMana()); //마나 초기화
    }
    
    [Button]
    private void StartPlayerTurn()
    {
        // 플레이어 스폰
        // TODO: 플레이어 데이터 로컬 저장 후 로드방식으로 변경
        SpawnUnits(UnitData.Data.DataList[DataManager.Inst.Data.playerCharacterId],true);
        
        // 랜덤 적 데이터 호출
        // TODO: 스테이지별로 적 생성 방식으로 변경
        var randomEnemies = UnitData.Data.DataList.Where(d => d.Team == Team.EnemyTeam).ToList()
            .DupRandomT(Random.Range(1, 4 + 1));
        Debug.Log(randomEnemies.Count());
        
        // 적 데이터 기반 플레이어 생성
        SpawnUnits(randomEnemies,false);
        
        // 턴 시작 애니메이션
        // TODO: 플레이어 턴 시작 표시
        
        // 플레이어 공격 시작
        ChangeUnitState(player,State.Attack);
    }
    

    #region Spawn
    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitData">생성할 유닛 정보</param>
    /// <param name="isAlly">우호 여부</param>
    private void SpawnUnits(UnitData.Data unitData, bool isAlly)
    {
        List<UnitData.Data> dataList = new List<UnitData.Data>(){unitData};
        SpawnUnits(dataList, isAlly);
    }
    
    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitData">생성할 유닛 정보 리스트</param>
    /// <param name="isAlly">우호 여부</param>
    private void SpawnUnits (List<UnitData.Data> unitData,bool isAlly)
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            //보간을 위한 비율 나누기
            //ex) 혼자일 때 0.5f(중간거리) 두명일 때 0.25f,0.75f(25%거리, 75%거리 위치) 세명 이상 부터는 꽉 채워서
            // 역대급 똥 가독성 - Jaehyun9912
            float space = unitData.Count == 1 ? 0.5f : unitData.Count == 2 ? 0.25f+0.5f*i: (float)i / (float)(unitData.Count-1);
            
            //플레이어 팀 진영인지 확인
            Transform trans = isAlly ? playerSpawnTrans : enemySpawnTrans;
            GameObject prefab = isAlly ? playerUnit : enemyUnit;
            
            //보간을 통해 생성 위치 연산
            float xPos = Mathf.Lerp(-spawnRange/2, spawnRange/2, space);
            Vector3 pos = trans.position + new Vector3(xPos, 0f);
            
            //소환
            var obj = Instantiate(prefab, pos, Quaternion.identity);
            Unit unit = obj.GetComponent<Unit>();
            unit.Init(unitData[i]);
            
            //각각에 맞는 팀에 추가
            if (isAlly)
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

    /// <summary>
    /// Enemy 사망 처리
    /// </summary>
    /// <param name="enemy">적 transform</param>
    public void RemoveEnemy(Unit enemy)
    {
        enemyTeams.Remove(enemy); //적 팀에서 죽은 적을 뺍니다.
        if(enemyTeams.Count <= 0)
        {
            // 현재 스테이지 종료
            // TODO: 스테이지 매니저의 클리어 이벤트 실행
            FadeInFadeOutManager.Inst.FadeOut(SceneManager.GetActiveScene().buildIndex,true); //적의 개수가 0이라면 라운드를 종료합니다.
        }
    }
    
    
    #region TurnState
    /// <summary>
    /// 유닛의 state를 변경
    /// </summary>
    /// <param name="unit">해당 유닛</param>
    /// <param name="state">변화시킬 상태</param>
    private void ChangeUnitState (Unit unit, State state)
    {
        unit.GetIChangeState().ChangeState(state);
    }
    
    /// <summary>
    /// 유닛의 state를 변경
    /// </summary>
    /// <param name="units">해당 유닛 리스트</param>
    /// <param name="state">변화시킬 상태</param>
    private void ChangeUnitState (List<Unit> units, State state)
    {
        foreach (var unit in units)unit.GetIChangeState().ChangeState(state);
    }

    /// <summary>
    /// 플레이어 턴 종료
    /// </summary>
    private void PlayerTurnEnd()    // TODO: 멀티플레이어시 턴 구분 필요
    {
        //플레이어 팀의 모든 캐릭터를 IdleState상태로
        ChangeUnitState(player, State.Idle);
        //적 팀의 캐릭터들이 차례대로 공격을 시전
        ChangeUnitState(enemyTeams, State.Attack);
    }

    /// <summary>
    /// 적 턴 종료
    /// </summary>
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
