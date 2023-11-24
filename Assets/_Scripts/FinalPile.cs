using UnityEngine;

public class FinalPile : CardPile
{
    [SerializeField] private CardSuit _requiredSuit;

    protected override bool IsValidSuit(Card card)
    {
        return card.Suit == _requiredSuit;
    }

    protected override bool IsValidValue(Card card)
    {
        if (_cardsOnPile.Count == 0)
            return card.Value == 0;

        return card.Value == _cardsOnPile[_cardsOnPile.Count - 1].Card.Value + 1;
    }
}