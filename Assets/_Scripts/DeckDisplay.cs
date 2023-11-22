using UnityEngine;

public class DeckDisplay : MonoBehaviour
{
    [Header("Instances")]
    [SerializeField] private CardDisplay _cardPrefab;
    [SerializeField] private Transform[] _piles;

    private Deck _deck;

    private void Start()
    {
        _deck = new Deck();
        _deck.Shuffle();

        for (int i = 0; i < 7; i++)
        {
            CardDisplay lastCard = null;
            for (int j = 0; j <= i; j++)
            {
                Card card = _deck.Pop();
                CardDisplay newCardDisplay = Instantiate(_cardPrefab, _piles[i].transform.position, Quaternion.identity, _piles[i]);
                newCardDisplay.SetCard(card);

                if (lastCard != null)
                    lastCard.AddCardOnTop(newCardDisplay);

                lastCard = newCardDisplay;
            }
        }

        while (_deck.Cards.Count > 0)
        {
            Card card = _deck.Pop();
            CardDisplay newCardDisplay = Instantiate(_cardPrefab, _piles[7].transform.position, Quaternion.identity, _piles[7]);
            newCardDisplay.SetCard(card);
        }
    }
}
