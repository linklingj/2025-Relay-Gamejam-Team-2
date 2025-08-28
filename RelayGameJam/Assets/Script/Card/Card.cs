using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace CardData
{
    public class Card : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
    {
        public PRS originPrs;
        private RectTransform rect;
        [SerializeField] private TextMeshProUGUI nameTxt, inforTxt,costTxt; //카드이름, 카드정보
        [SerializeField] private Image cardImage; //카드 이미지
        public SkillBase skill; //SkillData
        private PlayerCardController playerCardController; 
        bool onMouse = false; //현재 이 카드에 마우스가 올라가 있는가

        private string _path = "CardImage/";

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }
        
        
        //의존성 주입
        public void Inject(PlayerCardController playerCardController) => this.playerCardController = playerCardController;

        private void Update()
        {
            SelectCard();
        }

        //카드 선택
        private void SelectCard() 
        {
            if (playerCardController.GetIsCardSelected()) return; //이미 카드가 선택되어있다면 리턴
            if (Input.GetMouseButtonDown(0) && onMouse)
            {
                playerCardController.SetCardSelected(true,this); //playerController에 카드가 선택됐다고 전달
                switch (skill.data.IsTargeting)
                {
            
                    case 1:
                        CursorController.Inst.Targeting(rect); //타겟팅이 필요한 skill이라면 타겟팅 실행
                        break;
                }
            }
          
        }
        

        //지정한 위치로 이동
        public void MoveToPrs(PRS prs, bool useDotween = false,float duration = 0.2f)
        {
            if (useDotween)
            {
                rect.DOAnchorPos(prs.pos,duration);
                rect.DOScale(prs.scale,duration);
                rect.DORotateQuaternion(prs.rot,duration);
            }
            else
            {
                rect.anchoredPosition = prs.pos;
                rect.localScale = prs.scale;
                rect.rotation = prs.rot;
            }
            transform.SetSiblingIndex(originPrs.siblingIndex);//카드를 레이어를 원래 위치로 (sorting Layer같은 느낌)
        } 

        public void Init(SkillBase skill) //Skill Data 입력
        {
            this.skill = skill;
            nameTxt.text = skill.data.CardName;
            inforTxt.text = skill.data.CardInfor;
            costTxt.text = skill.data.cost.ToString();
            cardImage.sprite = Resources.Load<Sprite>(_path + skill.data.ImageName);//이미지 받아오기
        }

        public void OnPointerEnter(PointerEventData eventData) //카드에 마우스가 닿았을 때
        {
            if (playerCardController.GetIsCardSelected()) return;
            onMouse = true;
            transform.DOScale(Vector3.one * 1.1f, 0.2f); //스케일 증가
            transform.DORotateQuaternion(Quaternion.identity, 0.2f);
            rect.DOAnchorPos(originPrs.pos+new Vector2(0,100f), 0.2f);
            transform.SetAsLastSibling(); //카드의 위치를 맨 위로
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onMouse = false;
            if (playerCardController.GetIsCardSelected()) return;
            MoveToPrs(originPrs,true,0.2f); //기존 크기, 위치, 회전으로
        }
        
    }
}