using System;
using System.Collections.Generic;
using GoogleSheet.Core.Type;
using UnityEngine;

namespace CardData
{
    [UGS(typeof(CardAttackType))]
    public enum CardAttackType
    {
        Str,Spirit,All
    }

    public class CardDataManager : SingletonDontDestroyOnLoad<CardDataManager>
    {
        private void Awake()
        {
            Data.Load(); //구글 시트 에셋을 사용할 때 데이터를 불러올 때 이렇게 Load해줘야 합니다.
        }
    }
}