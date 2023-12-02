using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [Header("Instances")]
    [SerializeField] private CardDisplay _cardPrefab;
    [SerializeField] private DeckPile _deckPile;
    [SerializeField] private CardPile[] _piles;
    [SerializeField] private Transform _cardDragParent;
    [SerializeField] private FinalPile[] _finalPiles;

    [Header("Cards Visuals")]
    [SerializeField] private Sprite[] _suitSprites;
    [SerializeField] private Sprite[] _valueSprites;

    private Deck _deck;

    private void Start()
    {
        _deck = new Deck();
        _deck.Shuffle();

        for (int i = 0; i < _piles.Length; i++)
        {
            for (int j = 0; j <= i; j++)
            {
                Card card = _deck.Pop();
                CardDisplay newCardDisplay = CreateCard(card, _piles[i]);
                newCardDisplay.ShowReverse();

                if (j == i)
                    newCardDisplay.ShowFace();
            }
        }

        while (_deck.Cards.Count > 0)
        {
            Card card = _deck.Pop();
            CardDisplay newCardDisplay = CreateCard(card, _deckPile);
            newCardDisplay.ShowReverse();

            if (_deck.Cards.Count == 0)
                newCardDisplay.ShowFace();
        }
    }

    private CardDisplay CreateCard(Card card, CardPile pile)
    {
        CardDisplay newCardDisplay = Instantiate(_cardPrefab, pile.transform.position, Quaternion.identity);
        newCardDisplay.SetCard(card, _suitSprites[(int)card.Suit], _valueSprites[(int)card.Value]);
        newCardDisplay.SetDragParent(_cardDragParent);
        newCardDisplay.OnMovedHandler += CheckWinCondition;
        pile.ForceAddCardToPile(newCardDisplay);

        return newCardDisplay;
    }

    public void CheckWinCondition()
    {
        foreach (FinalPile pile in _finalPiles)
        {
            if (!pile.HasCards)
                return;

            if (pile.PeekCard().GetValue() != CardValue.King)
                return;
        }

        print("Win");
    }
}
