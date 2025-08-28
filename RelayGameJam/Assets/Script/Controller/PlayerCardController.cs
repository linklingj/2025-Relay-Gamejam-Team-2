using System;
using System.Collections;
using System.Collections.Generic;
using CardData;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using VInspector;

public class PRS //위치,회전,스케일 저장
{
    public Vector2 pos;
    public Quaternion rot;
    public Vector3 scale;
    public int siblingIndex;

    public PRS(Vector2 pos, Quaternion rot, Vector3 scale, int siblingIndex)
    {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
        this.siblingIndex = siblingIndex;
    }
}

public class PlayerCardController : Singleton<PlayerCardController>
{
    #region PlayerCard
    [SerializeField] private List<SkillBase> cardDict = new(); //플레이어가 보유한 전체 카드
    private Queue<SkillBase> cardDeck = new(); //남아 있는 카드
    private List<Card> cardHold = new(); //현재 손에 들고 있는 카드
    #endregion
    
    #region CardPlay
    [SerializeField] private RectTransform left, right; //카드 hand의 끄트머리 위치 및 회전값
    [SerializeField] private RectTransform cardSpawnParent; //카드를 소환할 부모
    [SerializeField] private Transform cardSpawnPos; //카드를 소환할 위치
    [SerializeField] private Card cardPrefab;
    private Card selectedCard; //현재 선택한 카드
    private bool isCardSelected = false; //카드가 선택 되었는가
    private bool isShow = false; //카드를 보여주는가

    private int maxHandCardCount = 7; //한 손에 들 수 있는 최대 카드의 양    // TODO: 게임모드 설정값으로 제외
    private int curMana = 3;
    private int maxMana = 3;    // TODO: 게임모드 설정값으로 제외
    #endregion
    
    [FormerlySerializedAs("costTxt")] [SerializeField] private TextMeshProUGUI manaTxt;
    [SerializeField] private CardAction inputAction; 

    private void Start()
    {
        // 액션 활성화
        // TODO : 조작 가능시점으로 옮기기
        inputAction = new CardAction();
        
        inputAction.CardControl.Enable();
        inputAction.CardControl.Select.started += (_ => Targeting());
        inputAction.CardControl.UnSelect.started += (_ => UnSelect());
        
        SetDeck();
        UpdateManaUI();
    }

    /// <summary>
    /// 카드 불러오기
    /// </summary>
    /// <param name="amount">불러올 카드 수량</param>
    [Button]
    public void GetCards(int amount = 3)
    {
        StartCoroutine(IESpawnCards(amount));
    }

    /// <summary>
    /// 마나 추가
    /// </summary>
    /// <param name="value">추가할 마나량</param>
    public void AddMana(int value)
    {
        curMana += value;   
        UpdateManaUI();
    }
    
    /// <summary>
    /// 마나를 최대치로 초기화
    /// </summary>
    public void ResetMana()
    {
        curMana = maxMana;
        
        // TODO: 마나 변화 이벤트 추가
        UpdateManaUI();
    }

    /// <summary>
    /// 마나표시 갱신
    /// </summary>
    private void UpdateManaUI()
    {
        manaTxt.text = curMana.ToString() + "/" + maxMana.ToString();
    }

    //마우스가 위에 있을 때는 카드 전체가 내려가고 마우스가 아래에 있을 때는 카드 전체가 올라가는 코드
    private void ShowCardsPos() 
    {
        float checkY = -2.5f;
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (mousePos.y < checkY && !isShow)
        {
            cardSpawnParent.DOAnchorPos(new Vector2(0,0),0.25f);
            isShow = true;
        }
        else if(isShow && mousePos.y >= checkY)
        {
            cardSpawnParent.DOAnchorPos(new Vector2(0,-200),0.25f);
            isShow = false;
        }
    }

    #region Targeting and Using
    /// <summary>
    /// 카드 선택
    /// </summary>
    private void Targeting() //카트를 선택했을 때 카드가 타겟팅 스킬이라면 Arc로 타겟팅
    {
        if (isCardSelected)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            //Unit오브젝트를 선택했다면 해당 Unit에세 스킬 사용
            if (hit.collider != null && hit.transform.TryGetComponent<Unit>( out Unit u) )
            {
                if (selectedCard.skill.data.cost > curMana)
                {
                    UnSelect();
                    return;
                }
                curMana -= selectedCard.skill.data.cost;
                UpdateManaUI();
                selectedCard.skill.SkillAction(u,TurnManager.Inst.player); //스킬 사용
                cardHold.Remove(selectedCard); //손에 있는 카드 없애기
                isCardSelected = false; //선택 취소
                CursorController.Inst.UnTargeting(); //Arc타겟팅 취소
                Destroy(selectedCard.gameObject); //오브젝트 삭제
                CardAlignment(); //남은 카드 정렬
                
            }
        }
    }

    /// <summary>
    /// 카드 선택 해제
    /// </summary>
    private void UnSelect()
    {
        if (isCardSelected)
        {
            isCardSelected = false;
            CursorController.Inst.UnTargeting();
            foreach (var card in cardHold)
            {
                card.MoveToPrs(card.originPrs,true); //모든 카드를 원래 자리로
            }
        }
    }
    #endregion
    
    public void SetCardSelected(bool selected, Card selectedCard = null)
    {
        isCardSelected = selected;
        if(selectedCard != null && selected) this.selectedCard = selectedCard;
    }
    public bool GetIsCardSelected() => isCardSelected;

    IEnumerator IESpawnCards(int count)
    {
        for (int i = 0; i < count; i++)
        {
            if (cardHold.Count >= maxHandCardCount) yield break;
            Debug.Log($"덱에서 카드 생성 시작 : {cardDeck.Count}");
            var skill = cardDeck.Dequeue();
            var spawnCard= Instantiate(cardPrefab,cardSpawnPos.position,Quaternion.identity,cardSpawnParent);
            spawnCard.Inject(this);
            spawnCard.Init(skill);
            Debug.Log(skill);
            cardHold.Add(spawnCard);
            
            CardAlignment(); //카드 정렬
            if(cardDeck.Count <= 0) SetDeck();
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// 카드덱 설정
    /// </summary>
    private void SetDeck()
    {
        // 카드 정보 호출
        cardDict = new List<SkillBase>(CardDataManager.Inst.Skills); // TODO: 임시로 모든 스킬을 가져옴
        Debug.Log($"도감 설정 완료 : {cardDict.Count}");
        cardDict.Shuffle(); //카드 셔플
        
        // 카드덱 설정
        cardDeck = new Queue<SkillBase>(cardDict);
        Debug.Log($"도감에서 호출 후 덱 설정 완료 : {cardDeck.Count}");
    }

    #region Card Alignment
    //카드 정렬
    private void CardAlignment()
    {
        List<PRS> newPRS = new List<PRS>();
        newPRS = RoundAlignment(); //원형 위치를 가져옴
        for (int i = 0; i <cardHold.Count; i++)
        {
            cardHold[i].originPrs = newPRS[i];
            cardHold[i].MoveToPrs(newPRS[i],true,0.35f); //새로운 위치로 이동
        }
    }

    private List<PRS> RoundAlignment() //원형 모양으로 카드의 위치를 다시 선정
    {
        float[] objLerps = new float[cardHold.Count];
        List<PRS> result = new List<PRS>();
        switch (cardHold.Count) 
        {
            case 1:
                objLerps = new float[] { 0.5f }; break;
            case 2:
                objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3:
                objLerps = new float[] { 0.1f, 0.5f ,0.9f}; break;
            default:
                float interval = 1f/(cardHold.Count - 1);
                for (int i = 0; i < cardHold.Count; i++)
                    objLerps[i] = interval * i;
                break;
            
        }

        //위치, 회전 보간
        for (int i = 0; i < cardHold.Count; i++)
        {
            var targetPos = Vector2.Lerp(left.anchoredPosition, right.anchoredPosition, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (cardHold.Count >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(0.5f, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                targetPos.y += curve*50; //50 = offset
                targetRot = Quaternion.Slerp(left.rotation, right.rotation, objLerps[i]);
            }
            result.Add(new PRS(targetPos, targetRot, Vector3.one,i));
           
        }
        return result;
    }
    #endregion
    
}
