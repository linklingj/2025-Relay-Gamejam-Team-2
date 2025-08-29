using System;
using System.Collections.Generic;
using CardData;


public interface ICardService
{
    public Queue<SkillBase> cardDeck { get; }
    public List<Card> handCards { get; }
    
    public Card selectedCard { get; }
    
    public void DrawCard(int amount);
    
    public void HighlightCard(Card card);
    
    public void UnHighlightCard(Card card);
    
    public void SelectCard(Card card);
    
    public void UnSelectCard();
    
    public void RemoveCard(Card card);
}
