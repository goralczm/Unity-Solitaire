using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPile : MonoBehaviour, IDropHandler
{
    [SerializeField] protected Vector2 _cardOffset;

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

        if (!CanAddCard(card.Card))
            return;

        if (_cardsOnPile.Count == 0)
            card.SetParent(transform, _cardOffset);
        else
        {
            if (!_cardsOnPile[_cardsOnPile.Count - 1].IsShown)
                return;

            card.SetParent(_cardsOnPile[_cardsOnPile.Count - 1].transform, _cardOffset);
        }

        AddCard(card);
    }

    public void ForceAddCardToPile(CardDisplay card)
    {
        if (_cardsOnPile.Count == 0)
            card.SetParent(transform, _cardOffset);
        else
            card.SetParent(_cardsOnPile[_cardsOnPile.Count - 1].transform, _cardOffset);

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

        if (_cardsOnPile.Count == 0)
            return;

        _cardsOnPile[_cardsOnPile.Count - 1].ShowFace();
    }

    public List<CardDisplay> GetCardsBelow(CardDisplay card)
    {
        List<CardDisplay> cardsBelow = new List<CardDisplay>();

        int cardIndex = _cardsOnPile.IndexOf(card);
        for (int i = cardIndex + 1; i < _cardsOnPile.Count; i++)
            cardsBelow.Add(_cardsOnPile[i]);

        return cardsBelow;
    }

    protected bool CanAddCard(Card card)
    {
        return IsValidValue(card) && IsValidSuit(card);
    }

    protected virtual bool IsValidValue(Card card)
    {
        if (_cardsOnPile.Count == 0)
            return true;

        return card.Value + 1 == _cardsOnPile[_cardsOnPile.Count - 1].Card.Value;
    }

    protected virtual bool IsValidSuit(Card card)
    {
        if (_cardsOnPile.Count == 0)
            return true;

        CardSuit lastCardSuit = _cardsOnPile[_cardsOnPile.Count - 1].Card.Suit;
        return (int)card.Suit != ((int)lastCardSuit + 2) % 4 &&
               card.Suit != lastCardSuit;
    }

    public CardDisplay Peek()
    {
        if (_cardsOnPile.Count == 0)
            return null;

        return _cardsOnPile[_cardsOnPile.Count - 1];
    }
}
