using System;
using System.Collections.Generic;
using GoogleSheet.Core.Type;

namespace CardData
{
    [UGS(typeof(CardAttackType))]
    public enum CardAttackType
    {
        Str,Spirit,All
    }

    [UGS(typeof(CardAttribute))]
    public enum CardAttribute
    {
        Attack,Utility,Ghost
    }
    

    public class CardDataManager : SingletonDontDestroyOnLoad<CardDataManager>
    {
        public List<SkillBase> Skills = new List<SkillBase>();
        private void Awake()
        {
            base.Awake();
            Data.Load(); //구글 시트 에셋을 사용할 때 데이터를 불러올 때 이렇게 Load해줘야 합니다.
            foreach (var cardData in Data.DataList)
            {
                switch (cardData.CardAttribute)
                {
                    case CardAttribute.Attack:
                        Skills.Add(new AttackSkill(cardData));
                        break;
                    case CardAttribute.Utility:
                        Skills.Add(new UtilitySkill(cardData));
                        break;
                    case CardAttribute.Ghost:
                        // TODO: 귀신 카드 스킬 구현
                        // throw new  NotImplementedException();
                    default:
                        continue;
                }
            }
           
        }
        
        public SkillBase GetRandomSkill() //랜덤 스킬 반환
        {
            return Skills[UnityEngine.Random.Range(0, Skills.Count)];
        }
        
    }
}