using System;
using System.Collections.Generic;
using CardData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VInspector;

public class TurnManager : Singleton<TurnManager>
{
    [SerializeField] private Stage stageData;
    [SerializeField] private int wave = 0;
    
    [SerializeField] private Button turnEndBtn; // 턴 종료 버튼
   
    [SerializeField] private Transform playerSpawnTrans, enemySpawnTrans; //플레이어 팀과 적 팀의 소환 위치
    [SerializeField] private float spawnRange = 6f; //플레이어 , 적 팀 화면상에 생성 반경

    private event Action OnPlayerTurnStart;
    private event Action OnPlayerTurnEnd;
    
    [Foldout("Debug")]
    public Unit player; //현재 플레이어 팀들을 저장
    [SerializeField] private List<Unit> enemyTeams; //현재 적 팀의 캐릭터들을 저장
    
    [SerializeField] private GameObject playerUnit,enemyUnit;
    
    private int enemyAttackCount = 0;   // 현재 턴 내 적 공격 횟수

    private void Awake()
    {
        // 턴 종료 버튼 연결
        turnEndBtn.onClick.AddListener(PlayerTurnEnd); // 플레이어 턴 종료
    }

    private void Start()
    {
        if (ServiceProvider.Inst.levelService != null)
        {
            stageData = ServiceProvider.Inst.levelService.GetStage();
            OnPlayerTurnStart += () => PlayerCardController.Inst.ResetMana(stageData.maxMana);
            StartWave(wave);
        }
    }
    
    [Button]
    private void StartWave(int num)
    {
        // 웨이브 정보 호출
        if (num < 0 || num > stageData.waves.Count)
        {
            return;
        }
        Wave wave = stageData.waves[num];
        
        // 플레이어 스폰
        PlayerInfo playerData = DataManager.Inst.playerInfo;
        SpawnUnits(playerData);
        
        
        // 랜덤 적 데이터 호출
        List<UnitData.Data> enemies = new();
        foreach (var id in wave.enemies)
        {
            enemies.Add(UnitData.Data.DataList.Find(d => d.ID == id));
        }
        SpawnUnits(enemies);
        
        // 턴 시작 애니메이션
        // TODO: 플레이어 턴 시작 표시
        
        // 플레이어 공격 시작
        PlayerCardController.Inst.Init(player, stageData.maxMana, SkillFactory.Inst.GetSkill(playerData.cardDeck));
        OnPlayerTurnStart?.Invoke();
        ChangeUnitState(player,State.Attack);
    }
    

    #region Spawn
    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitData">생성할 유닛 정보</param>
    /// <param name="isAlly">우호 여부</param>
    private void SpawnUnits(PlayerInfo _player)
    {
        float xPos = Mathf.Lerp(-spawnRange/2, spawnRange/2, 0.5f);
        
        //소환
        var obj = Instantiate(playerUnit, playerSpawnTrans.position + new Vector3(xPos, 0f), Quaternion.identity);
        Unit unit = obj.GetComponent<Unit>();
        unit.Init(_player);
        unit.CardService = PlayerCardController.Inst;
        
        player = unit;
    }
    
    /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitData">생성할 유닛 정보 리스트</param>
    /// <param name="isAlly">우호 여부</param>
    private void SpawnUnits (List<UnitData.Data> unitData)
    {
        for (int i = 0; i < unitData.Count; i++)
        {
            //보간을 위한 비율 나누기
            float space = unitData.Count == 1 ? 0.5f 
                        : unitData.Count == 2 ? 0.25f + 0.5f * i
                                                : i / (float)(unitData.Count-1);
            
            //보간을 통해 생성 위치 연산
            float xPos = Mathf.Lerp(-spawnRange/2, spawnRange/2, space);
            
            //소환
            var obj = Instantiate(enemyUnit, enemySpawnTrans.position + new Vector3(xPos, 0f), Quaternion.identity);
            Unit unit = obj.GetComponent<Unit>();
            unit.Init(unitData[i]);
            
            enemyTeams.Add(unit);
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
        OnPlayerTurnEnd?.Invoke();
        ChangeUnitState(player, State.Idle);

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
            OnPlayerTurnStart?.Invoke();

            ChangeUnitState(enemyTeams, State.Idle);
            ChangeUnitState(player, State.Attack);
            enemyAttackCount = 0;
        }
    }
    #endregion
}
