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
        [SerializeField] private TextMeshProUGUI nameTxt, inforTxt;
        [SerializeField] private Image cardImage;

        private string _path = "CardImage/";

        private void Awake()
        {
            rect = GetComponent<RectTransform>();
        }

        private void Start()
        {
            Init(Data.DataList[0]);
        }

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
        }

        public void Init(Data data)
        {
            nameTxt.text = data.CardName;
            inforTxt.text = data.CardInfor;
            cardImage.sprite = Resources.Load<Sprite>(_path + data.ImageName);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
            transform.DOScale(Vector3.one * 1.1f, 0.2f);
            transform.SetAsLastSibling();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(Vector3.one , 0.2f);
            transform.SetSiblingIndex(originPrs.siblingIndex);
        }
    }
}