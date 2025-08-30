using System;
using GoogleSheet.Core.Type;
using UnityEngine;
using UnityEngine.SceneManagement;

[UGS(typeof(Team))]
public enum Team
{
    PlayerTeam,EnemyTeam
}
public class Unit : MonoBehaviour
{
    [SerializeField] Team team;
    private Animator animator;
    private Stat hp,energyStack;
    private IChangeState changeState;
    [SerializeField] private HpBar hpBarPrefab;
    [SerializeField] private Transform energyPrefab;
    [SerializeField] private Transform energyTrans;
    [SerializeField] private GameResultUI gameResult;   //게임 클리어/ 게임 오버 창
    private HpBar hpBar;
    
    private PlayerController _playerController;
    private EnemyController _enemyController;
    
    public ICardService CardService;
    
    
    private void Awake()
    {
        changeState = GetComponent<IChangeState>();
        animator = GetComponent<Animator>();
        animator?.Play("Idle");

        gameResult = TurnManager.Inst.gameResult;
        
        if(team == Team.PlayerTeam) _playerController = GetComponent<PlayerController>();
        else _enemyController = GetComponent<EnemyController>();
    }
    
    
    public IChangeState GetIChangeState()=>changeState;
    
    /// <summary>
    /// 유닛 생성하기
    /// </summary>
    /// <param name="data">유닛 데이터</param>
    public void Init(UnitData.Data data)
    {
        hp = Stat.Create(data.Hp); //스탯 생성
        energyStack = Stat.Create(0); //흡입 스킬 스택
        hp.SetMaxValue(data.Hp);
        hp.OnValueChanged += OnDead; //hp의 값이 변경될때 실행되게 됩니다
        energyStack.OnValueChanged += EnergyAlign;
        hpBar = Instantiate(hpBarPrefab, UIController.Inst.GetCanvasTrans());  //hpbar 생성
        hpBar.Init(data.Hp,transform);
    }

    /// <summary>
    /// 플레이어 생성하기
    /// </summary>
    /// <param name="playerInfo">플레이어 정보</param>
    public void Init(PlayerInfo data)
    {
        hp = Stat.Create(data.Hp); //스탯 생성
        energyStack = Stat.Create(0); //흡입 스킬 스택
        hp.SetMaxValue(data.Hp);
        hp.OnValueChanged += OnDead; //hp의 값이 변경될때 실행되게 됩니다
        energyStack.OnValueChanged += EnergyAlign;
        hpBar = Instantiate(hpBarPrefab, UIController.Inst.GetCanvasTrans());  //hpbar 생성
        hpBar.Init(data.Hp,transform);
    }

    private void EnergyAlign(float value) //에너지 표시 정렬
    {
        foreach (Transform child in energyTrans)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < value; i++)
        {
            var a = Instantiate(energyPrefab, energyTrans);
            a.position += i  * new Vector3(0, 0.65f);
        }
        
    }

    public Stat GetStatHp() => hp;
    public Stat GetStatEnergy() => energyStack;

    public void OnDamage(float value) //데미지 받았을 때 실행시키는 함수
    {
        hp.AddValue(-value); //현재 hp값 변경
        hpBar.SetHpFillAmount(hp.baseValue); //hpBar의 fillAmount,text바꾸는 함수
        PopUpUIController.Inst.SpawnDamagePopUp(value,transform); //데미지 팝업 띄우기
    }

    private void OnDead(float curHp) //hp의 value가 변경될 때 자동으로 실행됩니다.
    {
        if (curHp <= 0)
        {
            if (team == Team.PlayerTeam)
            {
                //FadeInFadeOutManager.Inst.FadeOut(SceneManager.GetActiveScene().buildIndex,true);
                //게임오버
                gameResult.setResultPhrase("Game Over");
            }
            else //일단 적만 처리 해놨습니다.
            {
                Destroy(hpBar.gameObject);
                TurnManager.Inst.RemoveEnemy(this);
                Destroy(gameObject);
            }
        }
    }
    
    
    
}
