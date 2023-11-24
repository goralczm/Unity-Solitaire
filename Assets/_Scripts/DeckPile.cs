using UnityEngine.EventSystems;

public class DeckPile : CardPile, IPointerClickHandler
{
    protected override bool IsValidValue(Card card)
    {
        return false;
    }

    protected override bool IsValidSuit(Card card)
    {
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (_cardsOnPile.Count == 0 || _cardsOnPile.Count == 1)
            return;

        CardDisplay lastCard = _cardsOnPile[_cardsOnPile.Count - 1];
        RemoveCard(lastCard);
        lastCard.ShowReverse();
        lastCard.SetParent(transform, _cardOffset);
        _cardsOnPile[0].SetParent(lastCard.transform, _cardOffset);
        _cardsOnPile.Insert(0, lastCard);
    }
}
