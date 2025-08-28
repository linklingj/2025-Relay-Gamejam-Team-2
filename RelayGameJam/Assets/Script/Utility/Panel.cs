using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public enum PanelStates
{
    Show,Hide,Custom
}

public class Panel : MonoBehaviour
{
    [Serializable]
    public class Position
    {
        public PanelStates state;
       // [DrawIf("state", PanelStates.Custom)] 
        public string stateName;

        public Vector2 offset;
        [Range(0,1)]public float alpha;
        public bool blockRay;
        
        public Position(PanelStates state)
        {
            this.state = state;
        }
        public Position(PanelStates state, Vector2 offset) : this(state)
        {
            this.offset = offset;
        }
        public Position(PanelStates state, Vector2 offset, float alpha, bool blockRay) : this(state, offset)
        {
            this.alpha = alpha;
            this.blockRay = blockRay;
        }
    }
    
    
    public Position this[PanelStates state]
    {
        get
        {
            return positionList.FirstOrDefault(p => p.state == state);
        }
    }
    [SerializeField] private List<Position> positionList;
    public Position CurrentPosition;
    private CanvasGroup _canvasGroup;
    private RectTransform _rect;

    
    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SetPosition(positionList[0]);
    }

    public void SetPosition(PanelStates state, bool useDotween = false, float dotweenTime = 0.2f,Ease ease = Ease.Unset)
    {
        SetPosition(this[state],useDotween,dotweenTime,ease);
    }

    public void SetPosition(Position p,bool useDotween = false,float dotweenTime = 0.2f,Ease ease = Ease.Unset)
    {
        DOTween.Complete(_canvasGroup);
        DOTween.Complete(_rect);

        CurrentPosition = p;

        if (useDotween)
        {
            _rect.DOAnchorPos(p.offset, dotweenTime).SetEase(ease).SetUpdate(true);
            if (_canvasGroup)
            {
                _canvasGroup.DOFade(p.alpha, dotweenTime).SetUpdate(true);
                _canvasGroup.blocksRaycasts = p.blockRay;
            }
        }
        else
        {
            _rect.anchoredPosition = p.offset;
            if (_canvasGroup)
            {
                _canvasGroup.alpha = p.alpha;
                _canvasGroup.blocksRaycasts = p.blockRay;
            }
        }
    }
}
