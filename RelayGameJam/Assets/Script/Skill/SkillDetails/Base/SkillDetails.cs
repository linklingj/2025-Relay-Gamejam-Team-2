using UnityEngine;

public abstract class SkillDetails //세부 스킬 Heal이라는 스크립트가 어떻게 작동하는 지 보면 좋습니다.
{
    public float value;

    public void Init(float value)
    {
        this.value = value;
    }
    public abstract void DetailAction(Unit targetUnit,Unit ownerUnit);
}
