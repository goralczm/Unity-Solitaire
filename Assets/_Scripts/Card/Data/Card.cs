public class Card
{
    public readonly CardSuit Suit;
    public readonly CardValue Value;

    public Card(CardSuit color, CardValue value)
    {
        this.Suit = color;
        this.Value = value;
    }
}