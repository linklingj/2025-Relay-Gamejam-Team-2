using System;
using System.Collections;
using System.Collections.Generic;
using CardData;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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

public class PlayerCardController : Singleton<PlayerCardController>, ICardService
{
    #region PlayerCard

    public Unit owner;
    public Queue<SkillBase> cardDeck { get; private set; }
    public List<Card> handCards { get; private set; }
    
    public Card selectedCard { get; private set; }
    private bool isCardSelected => selectedCard;
    private bool onShowCard; //카드를 보여주는가
    private bool activeInput;
    #endregion
    
    #region ManaStatus
    private int curMana;
    private int maxMana;
    public event Action<int> OnManaChange;
    #endregion
    
    
    #region Object Setting
    [Foldout("Object Setting")]
    [SerializeField] private RectTransform left, right; //카드 hand의 끄트머리 위치 및 회전값
    [SerializeField] private RectTransform cardSpawnParent; //카드를 소환할 부모
    [SerializeField] private Transform cardSpawnPos; //카드를 소환할 위치
    [SerializeField] private Card cardPrefab;
    [SerializeField] private TextMeshProUGUI manaText;
    [SerializeField] private Button DrawButton;
    #endregion

    private void Awake()
    {
        ActiveInput();
        handCards = new();
    }

    public void Init(Unit owner, int _maxMana, List<SkillBase> _cardDeck)
    {
        this.owner = owner;
        // 마나 초기설정
        maxMana = _maxMana;
        curMana = _maxMana;
        
        // 덱 설정
        _cardDeck.Shuffle();
        cardDeck = new Queue<SkillBase>(_cardDeck);
        OnManaChange += UpdateManaUI;
    }

    void Update()
    {
        if (!isCardSelected || !activeInput)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Targeting();
        }
        else if (Input.GetMouseButtonUp(1))
        {
            UnSelectCard();
        }
    }

    public void ActiveInput()
    {
        activeInput = true;
        DrawButton.gameObject.SetActive(true);
        DrawButton.onClick.AddListener(() => DrawCard(1));
    }

    public void InactiveInput()
    {
        activeInput = false;
        DrawButton.gameObject.SetActive(false);
        DrawButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// 카드 불러오기
    /// </summary>
    /// <param name="amount">불러올 카드 수량</param>
    [Button]
    public void DrawCard(int amount)
    {
        StartCoroutine(IESpawnCard(amount));
    }

    public void HighlightCard(Card card)
    {
        if (!isCardSelected)
        {
            card.SetHighlight(true);
            card.onClick += SelectCard;
        }
    }

    public void UnHighlightCard(Card card)
    {
        if (!isCardSelected)
        {
            card.SetHighlight(false);
            card.onClick -= SelectCard;
        }
    }

    public void SelectCard(Card card)
    {
        Debug.Log($"Card selected: {card}");
        card.onClick -= SelectCard;
        card.SetHighlight(true);
        CursorController.Inst.Targeting(card.transform as RectTransform);
        selectedCard = card;
    }
    
    public void UnSelectCard()
    {
        if (isCardSelected)
        {
            selectedCard.SetHighlight(false);
            selectedCard = null;
            CursorController.Inst.UnTargeting();
        }
    }

    public void RemoveCard(Card card)
    {
        UnSelectCard(); // 스킬 선택 해제
            
        handCards.Remove(card); //손에 있는 카드 없애기
        Destroy(card.gameObject); //오브젝트 삭제
        CardAlignment(); //남은 카드 정렬
    }

    public void ExecuteCard(Card card, Unit target)
    {
        // 스킬 사용
        card.skill.SkillAction(target,owner); //스킬 사용
        AddMana(-card.skill.data.cost); // 마나 소모
        
        // 스킬 삭제
        RemoveCard(card);
    }

    /// <summary>
    /// 마나 변화
    /// </summary>
    /// <param name="value">변화할 마나량</param>
    public void AddMana(int value)
    {
        curMana += value;   
        OnManaChange?.Invoke(curMana);;
    }
    
    /// <summary>
    /// 마나를 최대치로 초기화
    /// </summary>
    public void ResetMana(int maxMana)
    {
        curMana = maxMana;
        
        OnManaChange?.Invoke(curMana);
    }

    /// <summary>
    /// 마나표시 갱신
    /// </summary>
    private void UpdateManaUI(int _curMana)
    {
        manaText.text = $"{_curMana}/{maxMana}";
    }

    #region Targeting and Using
    /// <summary>
    /// 스킬 대상 타게팅
    /// </summary>
    private void Targeting()
    {
        if (!isCardSelected) return;
        
        // Ray로 타겟 대상 확인
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
        //Unit오브젝트를 선택했다면 해당 Unit에세 스킬 사용
        if (!hit.collider || !hit.transform.TryGetComponent(out Unit u)) return;
        if (selectedCard.skill.data.cost > curMana)
        {
            UnSelectCard();
            return;
        }
            
        // 카드 실행
        Debug.Log("Card Execute");
        ExecuteCard(selectedCard, u);
        selectedCard = null;
    }

    #endregion

    /// <summary>
    /// 카드 생성 코루틴
    /// </summary>
    /// <param name="count">반복 생성 개수</param>
    IEnumerator IESpawnCard(int count)
    {
        if (cardDeck.Count == 0 || count <= 0)
        {
            Debug.Log("No card selected");
            yield break;
        }
        
        Debug.Log($"카드 드로우");
        var skill = cardDeck.Dequeue();
        
        // 카드 객체 생성
        var spawnCard= Instantiate(cardPrefab,cardSpawnPos.position,Quaternion.identity,cardSpawnParent);
        spawnCard.Init(skill);
        handCards.Add(spawnCard);
        spawnCard.onHighlight += HighlightCard;
        spawnCard.exitHighlight += UnHighlightCard;
        
        if (skill is GhostSkill)
        {
            // TODO: JumpScare 실행
            // 깜짝 놀라는 무언가를 실행
            // GhostSkill은 드로우 즉시 카드 실행
            // ExecuteCard(spawnCard, owner);
        }
        
        CardAlignment(); //카드 정렬

        yield return new WaitForSeconds(0.2f);
        StartCoroutine(IESpawnCard(count - 1));
    }

    #region Card Alignment
    //카드 정렬
    private void CardAlignment()
    {
        List<PRS> newPRS = new List<PRS>();
        newPRS = RoundAlignment(); //원형 위치를 가져옴
        for (int i = 0; i < handCards.Count; i++)
        {
            handCards[i].originPrs = newPRS[i];
            handCards[i].MoveToPrs(newPRS[i],true,0.35f); //새로운 위치로 이동
        }
    }

    private List<PRS> RoundAlignment() //원형 모양으로 카드의 위치를 다시 선정
    {
        float[] objLerps = new float[handCards.Count];
        List<PRS> result = new List<PRS>();
        switch (handCards.Count) 
        {
            case 1:
                objLerps = new float[] { 0.5f }; break;
            case 2:
                objLerps = new float[] { 0.27f, 0.73f }; break;
            case 3:
                objLerps = new float[] { 0.1f, 0.5f ,0.9f}; break;
            default:
                float interval = 1f/(handCards.Count - 1);
                for (int i = 0; i < handCards.Count; i++)
                    objLerps[i] = interval * i;
                break;
            
        }

        //위치, 회전 보간
        for (int i = 0; i < handCards.Count; i++)
        {
            var targetPos = Vector2.Lerp(left.anchoredPosition, right.anchoredPosition, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (handCards.Count >= 4)
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
