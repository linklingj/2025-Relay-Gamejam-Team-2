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
        
        public event Action<Card> onHighlight; //현재 이 카드에 마우스가 올라가 있는가
        public event Action<Card> onClick;
        public event Action<Card> exitHighlight;    // 이 카드에서 내려갔다

        private static string _path = "CardImage/";

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void Update()
        {
            if (onClick == null)
                return;
            
            if (Input.GetMouseButtonDown(0))
            {
                onClick?.Invoke(this);
            }
        }
        
        public void Init(SkillBase skill) //Skill Data 입력
        {
            this.skill = skill;
            nameTxt.text = skill.data.CardName;
            inforTxt.text = skill.data.CardInfor;
            costTxt.text = skill.data.cost.ToString();
            cardImage.sprite = Resources.Load<Sprite>(_path + skill.data.ImageName);//이미지 받아오기
        }

        public void Init()  //skill Data가 비어있는 경우
        {
            this.skill = null;
            nameTxt.text = "??";
            inforTxt.text = "??";
            costTxt.text = "??";
            cardImage.sprite = Resources.Load<Sprite>(_path + "test");//이미지 받아오기
        }

        /// <summary>
        /// 카드 하이라이트 설정
        /// </summary>
        /// <param name="highlight">하이라이트 여부</param>
        public void SetHighlight(bool highlight)
        {
            if (highlight)
            {
                /* //기존 코드
                transform.DOScale(Vector3.one * 1.1f, 0.2f); //스케일 증가
                transform.DORotateQuaternion(Quaternion.identity, 0.2f);
                rect.DOAnchorPos(originPrs.pos+new Vector2(0,100f), 0.2f);
                transform.SetAsLastSibling(); //카드의 위치를 맨 위로
                */
                
                //새로 수정한 코드

                float targetScale = 0.75f;
                transform.DOScale(Vector3.one * targetScale * 1.1f, 0.2f); //스케일 증가
                transform.DORotateQuaternion(Quaternion.identity, 0.2f);
                rect.DOAnchorPos(originPrs.pos+new Vector2(0,50f), 0.2f);
                transform.SetAsLastSibling(); //카드의 위치를 맨 위로

                
            }
            else
            {
                MoveToPrs(originPrs,true,0.2f); //기존 크기, 위치, 회전으로
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData) //카드에 마우스가 닿았을 때
        {
            onHighlight?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            exitHighlight?.Invoke(this);
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
    }
}