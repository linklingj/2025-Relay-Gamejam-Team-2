using CardData;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class BuildCardDeck : MonoBehaviour
{
    [SerializeField] private Transform[] CardDeckPos;       //카드을 정렬할 위치

    [SerializeField] private Card cardPrefab;               //카드 프리펩
    [SerializeField] private Card GhostCardPrefab;

    [SerializeField] private SkillFactory skillData;        //카드 프리펩을 생성할 카드 정보

    public Transform[] CardParent;                          
    public GameObject[] Page;                               //페이지 기능

    public List<int> DeckList;                              //현재 저장한 덱 정보 저장
    public Button[] DeckButtons;                            //카드 버튼 정보
    public int currentPage = 0;
    public TextMeshProUGUI[] SelectedCardText;              //저장한 덱을 보여주는 UI
    public int MaxCardCnt = 4;

    public int PlayerCardIndex = 10;
    public int TotalCardIndex = 15;
    public int playerCardListCnt;

    public void ShowCardList()              //모든 카드 정보를 보여주는 기능
    {
        playerCardListCnt = DataManager.Inst.playerInfo.cardList.Count;
        ShowCard(playerCardListCnt);

        UpdateSelectCard();
    }


    public void UpdatePage(int p)       //page 1, 2 클릭 시 다른 정보가 나오는 기능
    {
        currentPage = p;

        Page[p].SetActive(true);
        if (p == 0)
            Page[1].SetActive(false);
        else
            Page[0].SetActive(false);
    }

    public void SelectCard(int id)      //선택한 카드를 덱에 저장, 최대 4개 저장 가능
    {
        if (DeckList.Count >= MaxCardCnt)
            return;
        DeckList.Add(id);
        UpdateSelectedCardList();
    }

    public void UpdateSelectCard()      //카드의 버튼에 함수 연결
    {
        Debug.Log(playerCardListCnt);
        for (int i = 0; i < playerCardListCnt; i++)
        {
            int index = i;
            DeckButtons[i].onClick.AddListener(() => SelectCard(index));
        }
    }

    public void UpdateSelectedCardList()        //저장한 덱을 UI에 업데이트
    {
        for(int i = 0;i < DeckList.Count;i++)
        {
            SelectedCardText[i].text = DeckList[i].ToString();
        }
        DataManager.Inst.playerInfo.cardDeck = DeckList;
    }

    public void ListClear()         //저장한 덱을 비울 경우
    {
        for (int i = 0; i < DeckList.Count; i++)
        {
            SelectedCardText[i].text = ".";
        }
        DeckList.Clear();
        UpdateSelectedCardList();
    }

    public void ShowCard(int CardListCnt)
    {
        for(int i = 0; i < TotalCardIndex; i++)
        {
            spawnCard(i, CardListCnt);
        }
    }

    public void spawnCard(int cardIndex, int cardListCnt)
    {
        Transform parent = (cardIndex < 10) ? CardParent[0] : CardParent[1];

        if (cardIndex > PlayerCardIndex) // 귀신 카드
        {
            var spawnCard = Instantiate(GhostCardPrefab, CardDeckPos[cardIndex].position, Quaternion.identity, parent);

            if (cardIndex < cardListCnt) // 해금된 귀신 카드
            {
                DeckButtons[cardIndex] = spawnCard.GetComponent<Button>();
                spawnCard.Init(skillData.GetSkill(cardIndex));
            }
            else // 미해금 귀신 카드
            {
                spawnCard.Init();
            }
        }
        else // 일반 카드
        {
            var spawnCard = Instantiate(cardPrefab, CardDeckPos[cardIndex].position, Quaternion.identity, parent);

            if (cardIndex < cardListCnt) // 해금된 일반 카드
            {
                DeckButtons[cardIndex] = spawnCard.GetComponent<Button>();
                spawnCard.Init(skillData.GetSkill(cardIndex));
            }
            else // 미해금 일반 카드
            {
                spawnCard.Init();
            }
        }
    }
}
