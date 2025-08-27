using System;
using System.Collections;
using System.Collections.Generic;
using CardData;
using DG.Tweening;
using TMPro;
using UnityEngine;
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
    [SerializeField] private List<SkillBase> allSkills = new List<SkillBase>(); //플레이어가 보유한 전체 카드
    private Queue<SkillBase> remainSkills = new Queue<SkillBase>(); //남아 있는 카드
    private List<Card> handCards = new List<Card>(); //현재 손에 들고 있는 카드
    
    [SerializeField] private RectTransform left, right; //카드 hand의 끄트머리 위치 및 회전값
    [SerializeField] private RectTransform cardSpawnParent; //카드를 소환할 부모
    [SerializeField] private Transform cardSpawnPos; //카드를 소환할 위치
    [SerializeField] private Card cardPrefab;
    private Card selectedCard; //현재 선택한 카드
    private int maxHandCardCount = 7; //한 손에 들 수 있는 최대 카드의 양
    private bool isCardSelected = false; //카드가 선택 되었는가
    private bool isShow = false; //카드를 보여주는가

    [SerializeField] private TextMeshProUGUI costTxt;
    private int curMana = 3;

    private void Start()
    {
        FillSkills();
    }

    //카드를 가져옴
    [Button]
    public void GetCards(int value = 3)
    {
        StartCoroutine(IESpawnCards(value));
    }

    private void Update()
    {
       UnSelect();
       Targeting();
       ShowCardsPos();
    }

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

    private void Targeting() //카트를 선택했을 때 카드가 타겟팅 스킬이라면 Arc로 타겟팅
    {
        if (isCardSelected && Input.GetMouseButtonDown(0))
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
                selectedCard.skill.SkillAction(u); //스킬 사용
                handCards.Remove(selectedCard); //손에 있는 카드 없애기
                isCardSelected = false; //선택 취소
                ArcController.Inst.UnTargeting(); //Arc타겟팅 취소
                Destroy(selectedCard.gameObject); //오브젝트 삭제
                CardAlignment(); //남은 카드 정렬
                
            }
        }
    }

    private void UnSelect() //카드 선택한 거 취소
    {
        if (isCardSelected && Input.GetMouseButtonDown(1))
        {
            isCardSelected = false;
            ArcController.Inst.UnTargeting();
            foreach (var card in handCards)
            {
                card.MoveToPrs(card.originPrs,true); //모든 카드를 원래 자리로
            }
        }
    }

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
            if (handCards.Count >= maxHandCardCount) yield break;
            var skill = remainSkills.Dequeue();
            var spawnCard= Instantiate(cardPrefab,cardSpawnPos.position,Quaternion.identity,cardSpawnParent);
            spawnCard.Inject(this);
            spawnCard.Init(skill);
            handCards.Add(spawnCard);
            
            CardAlignment(); //카드 정렬
            if(remainSkills.Count <=0)FillSkills();
            yield return new WaitForSeconds(0.1f);
        }
    }

    //남아있는 카드에 카드 채우기
    private void FillSkills()
    {
        allSkills = new List<SkillBase>(CardDataManager.Inst.Skills); //todo 임시로 모든 스킬을 가져옴
        allSkills.Shuffle(); //카드 셔플
        remainSkills = new Queue<SkillBase>(allSkills);
    }

    //카드 정렬
    private void CardAlignment()
    {
        List<PRS> newPRS = new List<PRS>();
        newPRS = RoundAlignment(); //원형 위치를 가져옴
        for (int i = 0; i <handCards.Count; i++)
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
    
    
}
