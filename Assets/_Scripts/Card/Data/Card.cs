public class Card
{
    public readonly CardColor color;
    public readonly CardValue value;

    public Card(CardColor color, CardValue value)
    {
        this.color = color;
        this.value = value;
    }
}