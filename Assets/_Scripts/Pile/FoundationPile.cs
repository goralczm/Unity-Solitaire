public class FoundationPile : CardPile
{
    public override bool CanAddCard(Card card)
    {
        if (!HasCards && card.GetValue() != CardValue.King)
            return false;

        return base.CanAddCard(card);
    }
}
