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
    

    public class SkillFactory : SingletonDontDestroyOnLoad<SkillFactory>
    {
        public List<SkillBase> Skills = new();

        protected override void Awake()
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

        /// <summary>
        /// 모든 플레이어 스킬 반환
        /// </summary>
        /// <returns>플레이어 스킬 목록</returns>
        public List<SkillBase> GetAllSkills()
        {
            List<SkillBase> skills = new();
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
                    default:
                        continue;
                }
            }
            return skills;
        }

        /// <summary>
        /// 해당하는 스킬 반환
        /// </summary>
        /// <param name="id">스킬 ID</param>
        /// <returns>스킬 데이터</returns>
        public SkillBase GetSkill(int id)
        {
            var data = Data.DataList.Find(d => d.ID == id);
            switch (data.CardAttribute)
            {
                case CardAttribute.Attack:
                    return new AttackSkill(data);
                case CardAttribute.Utility:
                    return new UtilitySkill(data);
                case CardAttribute.Ghost:
                    return new GhostSkill(data);
                default:
                    return null;
            }
        }

        /// <summary>
        /// 해당하는 스킬 리스트 반환
        /// </summary>
        /// <param name="idList">스킬 ID 리스트</param>
        /// <returns>스킬 데이터 리스트</returns>
        public List<SkillBase> GetSkill(List<int> idList)
        {
            List<SkillBase> skills = new();
            foreach (var id in idList)
            {
                skills.Add(GetSkill(id));
            }
            return skills;
        }
        
        
        /// <summary>
        /// 모든 플레이어 스킬 반환
        /// </summary>
        /// <returns>플레이어 스킬 목록</returns>
        public List<SkillBase> GetGhostSkills()
        {
            List<SkillBase> skills = new();
            foreach (var cardData in Data.DataList)
            {
                if (cardData.CardAttribute == CardAttribute.Ghost)
                {
                    skills.Add(new GhostSkill(cardData));
                }
            }
            return skills;
        }
        
        
        /// <summary>
        /// 랜덤 스킬 반환
        /// </summary>
        /// <param name="start">랜덤 범위 시작</param>
        /// <param name="range">랜덤 범위 끝</param>
        /// <returns>랜덤 스킬 1개</returns>
        public SkillBase GetRandomSkill(int start = 0, int range = -1) //랜덤 스킬 반환
        {
            if (range < 0)
            {
                range = Skills.Count;
            }
            return Skills[UnityEngine.Random.Range(start, range)];
        }
        
    }
}