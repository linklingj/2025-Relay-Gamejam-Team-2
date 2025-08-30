
using CardData;

public class Hit : SkillBase
{
    public Hit(Data data) : base(data)
    {
    }

    public override void SkillAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage(data.Value);
    }
}

/// <summary>
/// 새 카드 해금
/// </summary>
public class UnlockCard : SkillBase
{
    public UnlockCard(Data data) : base(data)
    {
        DataManager.Inst.playerInfo.cardDeck.Add(data.DetailValue);
    }

    public override void SkillAction(Unit targetUnit, Unit ownerUnit)
    {
        targetUnit.OnDamage(data.Value);
        DataManager.Inst.playerInfo.UnlockCard(data.DetailValue);
    }
}

/// <summary>
///  제일 왼쪽의 카드 삭제
/// </summary>
public class LostCard : SkillBase
{
    public LostCard(Data data) : base(data)
    {
        PlayerCardController.Inst.RemoveCard(PlayerCardController.Inst.handCards[0]);
    }

    public override void SkillAction(Unit targetUnit, Unit ownerUnit)
    {
        if (targetUnit.CardService?.handCards.Count > 0)
        {
            targetUnit.CardService.RemoveCard(ownerUnit.CardService.handCards[0]);
        }
    }
}
