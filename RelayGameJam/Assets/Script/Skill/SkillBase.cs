using System;
using UnityEngine;

namespace CardData
{
    [Serializable]
    public abstract class SkillBase 
    {
        public Data data;

        public SkillBase(Data data)
        {
            this.data = data;
        }

        public abstract void SkillAction(Unit unit);
    }
}