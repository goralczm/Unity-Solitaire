using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPile : MonoBehaviour, IDropHandler
{
    [Header("Settings")]
    [SerializeField] protected Vector2 _cardsOffset;

    protected List<Card> _cardsOnPile;

    private void Awake() => _cardsOnPile = new List<Card>();

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Card draggedCard = eventData.pointerDrag.GetComponent<Card>();
        if (draggedCard == null)
            return;

        if (!CanAddCard(draggedCard))
            return;

        MoveCardCommand moveCardCommand = new MoveCardCommand(draggedCard.GetPile(), this, draggedCard);
        GameFlow.Instance.RegisterCommand(moveCardCommand);
    }

    public virtual bool CanAddCard(Card card)
    {
        if (_cardsOnPile.Contains(card))
            return false;

        if (!IsValidSuitAndValue(card.GetSuit(), card.GetValue()))
            return false;

        if (HasCards && !PeekCard().IsShown)
            return false;

        return true;
    }

    public virtual void AddCardToPile(Card card)
    {
        if (!HasCards)
            card.SetParent(transform, Vector2.zero);
        else
            card.SetParent(PeekCard().transform, _cardsOffset);

        AddCard(card);
    }

    public Card PeekCard() => _cardsOnPile.Count == 0 ? null : _cardsOnPile[_cardsOnPile.Count - 1];

    public bool HasCards => _cardsOnPile.Count > 0;

    public void ForceAddCardToPile(Card card)
    {
        if (!HasCards)
            card.SetParent(transform, Vector2.zero);
        else
            card.SetParent(_cardsOnPile[_cardsOnPile.Count - 1].transform, _cardsOffset);

        AddCard(card);
    }

    private void AddCard(Card card)
    {
        _cardsOnPile.Add(card);

        List<Card> cardsBelow = card.GetAllCardsBelow();
        foreach (Card cardBelow in cardsBelow)
        {
            _cardsOnPile.Add(cardBelow);
            cardBelow.SetPile(this);
        }

        card.SetPile(this);
    }

    public void RemoveCard(Card card)
    {
        _cardsOnPile.Remove(card);

        if (!HasCards)
            return;

        PeekCard().ShowFace();
    }

    public List<Card> GetCardsBelow(Card card)
    {
        List<Card> cardsBelow = new List<Card>();

        int cardIndex = _cardsOnPile.IndexOf(card);
        for (int i = cardIndex + 1; i < _cardsOnPile.Count; i++)
            cardsBelow.Add(_cardsOnPile[i]);

        return cardsBelow;
    }

    protected bool IsValidSuitAndValue(CardSuit suit, CardValue value)
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
