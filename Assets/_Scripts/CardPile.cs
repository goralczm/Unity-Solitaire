using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPile : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    [SerializeField] protected Vector2 _cardsOffset;

    protected List<CardDisplay> _cardsOnPile;

    private void Awake() => _cardsOnPile = new List<CardDisplay>();

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        CardDisplay draggedCard = eventData.pointerDrag.GetComponent<CardDisplay>();
        if (draggedCard == null)
            return;

        AddCardToPile(draggedCard);
    }

    public virtual void AddCardToPile(CardDisplay card)
    {
        if (_cardsOnPile.Contains(card))
            return;

        if (!CanAddCard(card.GetSuit(), card.GetValue()))
            return;

        if (!HasCards)
            card.SetParent(transform, _cardsOffset);
        else
        {
            if (!PeekCard().IsShown)
                return;

            card.SetParent(PeekCard().transform, _cardsOffset);
        }

        AddCard(card);
    }

    public CardDisplay PeekCard() => _cardsOnPile.Count == 0 ? null : _cardsOnPile[_cardsOnPile.Count - 1];

    public bool HasCards => _cardsOnPile.Count > 0;

    public void ForceAddCardToPile(CardDisplay card)
    {
        if (!HasCards)
            card.SetParent(transform, _cardsOffset);
        else
            card.SetParent(_cardsOnPile[_cardsOnPile.Count - 1].transform, _cardsOffset);

        AddCard(card);
    }

    private void AddCard(CardDisplay card)
    {
        _cardsOnPile.Add(card);

        List<CardDisplay> cardsBelow = card.GetAllCardsBelow();
        foreach (CardDisplay cardBelow in cardsBelow)
        {
            _cardsOnPile.Add(cardBelow);
            cardBelow.SetPile(this);
        }

        card.SetPile(this);
    }

    public void RemoveCard(CardDisplay card)
    {
        _cardsOnPile.Remove(card);

        if (!HasCards)
            return;

        PeekCard().ShowFace();
    }

    public List<CardDisplay> GetCardsBelow(CardDisplay card)
    {
        List<CardDisplay> cardsBelow = new List<CardDisplay>();

        int cardIndex = _cardsOnPile.IndexOf(card);
        for (int i = cardIndex + 1; i < _cardsOnPile.Count; i++)
            cardsBelow.Add(_cardsOnPile[i]);

        return cardsBelow;
    }

    protected bool CanAddCard(CardSuit suit, CardValue value)
    {
        return IsValidValue(value) && IsValidSuit(suit);
    }

    protected virtual bool IsValidValue(CardValue value)
    {
        if (!HasCards)
            return true;

        return value + 1 == PeekCard().GetValue();
    }

    protected virtual bool IsValidSuit(CardSuit suit)
    {
        if (!HasCards)
            return true;

        CardSuit lastCardSuit = PeekCard().GetSuit();
        return (int)suit != ((int)lastCardSuit + 2) % 4 &&
               suit != lastCardSuit;
    }
}
