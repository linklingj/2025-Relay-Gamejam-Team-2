
using CardData;

public class Hit : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage(value);
    }
}

/// <summary>
/// 새 카드 해금
/// </summary>
public class UnlockCard : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage(value);
        DataManager.Inst.playerInfo.UnlockCard((int)value);
    }
}

/// <summary>
///  제일 왼쪽의 카드 삭제
/// </summary>
public class LostCard : SkillDetails
{
    public override void DetailAction(Unit targetUnit, Unit ownerUnit)
    {
        if (targetUnit.CardService?.handCards.Count > 0)
        {
            targetUnit.CardService.RemoveCard(ownerUnit.CardService.handCards[0]);
        }
    }
}
