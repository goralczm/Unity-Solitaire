using System.Collections.Generic;

public class Deck
{
    public List<Card> Cards { get; private set; }

    public Deck()
    {
        Cards = new List<Card>();
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 13; j++)
            {
                Card newCard = new Card((CardSuit)i, (CardValue)j);
                Cards.Add(newCard);
            }
        }
    }

    public void Shuffle()
    {
        System.Random random = new System.Random();
        for (int i = 0; i < 52; i++)
        {
            int randomCardIndex = random.Next(Cards.Count);

            SwapCards(randomCardIndex, i);
        }
    }

    private void SwapCards(int firstCardIndex, int secondCardIndex)
    {
        Card tmpCard = Cards[firstCardIndex];
        Cards[firstCardIndex] = Cards[secondCardIndex];
        Cards[secondCardIndex] = tmpCard;
    }

    public Card Pop()
    {
        Card card = Cards[Cards.Count - 1];
        Cards.RemoveAt(Cards.Count - 1);

        return card;
    }
}