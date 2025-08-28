using System;
using UnityEngine;

public enum Team
{
    PlayerTeam,EnemyTeam
}
public class Unit : MonoBehaviour
{
    [SerializeField] Team team;
    private Animator animator;
    private Stat hp,ghostStack;
    private UnitData.Data data;
    private IChangeState changeState;
    [SerializeField] private HpBar hpBarPrefab;
    private HpBar hpBar;
    
    private PlayerController _playerController;
    private EnemyController _enemyController;
    
    
    private void Awake()
    {
        changeState = GetComponent<IChangeState>();
        animator = GetComponent<Animator>();
        
        if(team == Team.PlayerTeam) _playerController = GetComponent<PlayerController>();
        else _enemyController = GetComponent<EnemyController>();
    }
    
    
    public IChangeState GetIChangeState()=>changeState;
    
    
    public void Init(UnitData.Data data) //캐릭터 데이터 받기
    {
        this.data = data;
        hp = Stat.Create(data.Hp); //스탯 생성
        ghostStack = Stat.Create(0); //흡입 스킬 스택
        hp.SetMaxValue(data.Hp);
        hp.OnValueChanged += OnDead; //hp의 값이 변경될때 실행되게 됩니다
        hpBar = Instantiate(hpBarPrefab, UIController.Inst.GetCanvasTrans());  //hpbar 생성
        hpBar.Init(data.Hp,transform);
    }

    public Stat GetStatHp() => hp;
    public Stat GetStatGhost() => ghostStack;

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
                //게임오버
            }
            else //일단 적만 처리 해놨습니다.
            {
                Destroy(hpBar.gameObject);
                TurnManager.Inst.CheckEnemy(transform);
                Destroy(gameObject);
            }
        }
    }
    
    
    
}
