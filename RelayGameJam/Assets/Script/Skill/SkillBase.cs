using System;
using System.Collections.Generic;
using UnityEngine;

namespace CardData
{
    [Serializable]
    public abstract class SkillBase 
    {
        public Data data;
        public SkillDetails skillDetail; //세부 스킬

        public SkillBase(Data data)
        {
            this.data = data;
            
            //세부 스킬을 이름을 통해 찾는 함수 
            //구글 시트에서 SkillDetail 부분을 스크립트 명과 똑같이 적어야함
            skillDetail =SkillDetailsFactory.GetSkillByName(data.SkillDetail); 
            skillDetail?.Init(data.DetailValue);
        }

        public abstract void SkillAction(Unit targetUnit,Unit ownerUnit);

        protected virtual void DetailAction(Unit targetUnit,Unit ownerUnit) //세부스킬 시전
        {
            skillDetail?.DetailAction( targetUnit,ownerUnit);
        }
    }
}

public static class SkillDetailsFactory
{
    private static Dictionary<string, SkillDetails> cache = new();

    public static SkillDetails GetSkillByName(string className)
    {
        if(className == "null")return null;
        // 캐시에 있으면 바로 반환
        if (cache.TryGetValue(className, out var cachedInstance))
        {
            return cachedInstance;
        }

        // 타입 찾기
        Type type = Type.GetType(className);

        if (type != null && type.IsSubclassOf(typeof(SkillDetails)))
        {
            // 인스턴스 생성 후 캐시에 저장
            var instance = Activator.CreateInstance(type) as SkillDetails;
            cache[className] = instance;
            return instance;
        }

        Debug.LogError($"{className} SkillDetails를 찾을 수 없습니다.");
        return null;
    }
}