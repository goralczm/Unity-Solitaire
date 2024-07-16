using System;
using UnityEngine;
using UnityEngine.Events;

public class GameSetup : Singleton<GameSetup>
{
    [Header("Instances")]
    [SerializeField] private Card _cardPrefab;
    [SerializeField] private DeckPile _deckPile;
    [SerializeField] private CardPile[] _foundationPiles;
    [SerializeField] private Transform _cardDragParent;
    [SerializeField] private FinalPile[] _finalPiles;

    [Header("Cards Visuals")]
    [SerializeField] private Sprite[] _suitSprites;
    [SerializeField] private Sprite[] _valueSprites;

    private Deck _deck;

    public UnityEvent OnWinEvent;

    private void Start()
    {
        _deck = new Deck();
        _deck.Shuffle();

        SetupFoundationPiles();
        SetupDeckPile();
    }

    private void SetupFoundationPiles()
    {
        for (int i = 0; i < _foundationPiles.Length; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                CardData card = _deck.Pop();
                Card newCardDisplay = CreateCard(card, _foundationPiles[i]);
                newCardDisplay.ShowReverse();

                if (j == i)
                    newCardDisplay.ShowFace();
            }
        }
    }

    private void SetupDeckPile()
    {
        while (_deck.Cards.Count > 0)
        {
            CardData card = _deck.Pop();
            Card newCardDisplay = CreateCard(card, _deckPile);
            newCardDisplay.ShowReverse();

            if (_deck.Cards.Count == 0)
                newCardDisplay.ShowFace();
        }
    }

    private Card CreateCard(CardData card, CardPile pile)
    {
        Card newCardDisplay = Instantiate(_cardPrefab, pile.transform.position, Quaternion.identity);
        newCardDisplay.SetCard(card, _suitSprites[(int)card.Suit], _valueSprites[(int)card.Value]);
        newCardDisplay.SetDragParent(_cardDragParent);
        newCardDisplay.OnMovedHandler += CheckWinCondition;
        pile.ForceAddCardToPile(newCardDisplay);

        return newCardDisplay;
    }

    private void CheckWinCondition()
    {
        foreach (FinalPile pile in _finalPiles)
        {
            if (!pile.HasCards)
                return;

            if (pile.PeekCard().GetValue() != CardValue.King)
                return;
        }

        OnWinEvent?.Invoke();
    }

    public void TryAutoPlaceCard(Card card)
    {
        foreach (CardPile pile in _finalPiles)
        {
            if (pile.CanAddCard(card))
            {
                MoveCardCommand moveCardCommand = new MoveCardCommand(card.GetPile(), pile, card);
                GameFlow.Instance.RegisterCommand(moveCardCommand);
                return;
            }
        }

        foreach (CardPile pile in _foundationPiles)
        {
            if (pile.CanAddCard(card))
            {
                MoveCardCommand moveCardCommand = new MoveCardCommand(card.GetPile(), pile, card);
                GameFlow.Instance.RegisterCommand(moveCardCommand);
                return;
            }
        }
    }
}
