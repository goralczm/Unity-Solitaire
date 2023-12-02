using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPile : CardPile, IPointerClickHandler
{
    protected override bool IsValidValue(CardValue value)
    {
        return false;
    }

    protected override bool IsValidSuit(CardSuit suit)
    {
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        if (!HasCards || _cardsOnPile.Count == 1)
            return;

        CardDisplay lastCard = _cardsOnPile[_cardsOnPile.Count - 1];
        RemoveCard(lastCard);
        lastCard.ShowReverse();
        lastCard.SetParent(transform, Vector2.zero);
        _cardsOnPile[0].SetParent(lastCard.transform, _cardsOffset);
        _cardsOnPile.Insert(0, lastCard);
    }
}
