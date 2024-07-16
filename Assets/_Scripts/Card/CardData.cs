public class CardData
{
    public readonly CardSuit Suit;
    public readonly CardValue Value;

    public CardData(CardSuit color, CardValue value)
    {
        this.Suit = color;
        this.Value = value;
    }
}