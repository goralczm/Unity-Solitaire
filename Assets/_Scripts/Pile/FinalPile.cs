using UnityEngine;

public class FinalPile : CardPile
{
    [SerializeField] private CardSuit _requiredSuit;

    public override bool CanAddCard(Card card)
    {
        bool hasCardsBelow = card.GetAllCardsBelow().Count > 0;
        if (hasCardsBelow)
            return false;

        return base.CanAddCard(card);
    }

    protected override bool IsValidSuit(CardSuit suit)
    {
        return suit == _requiredSuit;
    }

    protected override bool IsValidValue(CardValue value)
    {
        if (!HasCards)
            return value == CardValue.Ace;

        return value == _cardsOnPile[_cardsOnPile.Count - 1].GetValue() + 1;
    }
}
