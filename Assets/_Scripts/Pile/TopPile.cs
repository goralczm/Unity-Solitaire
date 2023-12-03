public class TopPile : CardPile
{
    public override void AddCardToPile(CardDisplay card)
    {
        if (!HasCards)
        {
            if (card.GetValue() != CardValue.King)
                return;
        }

        base.AddCardToPile(card);
    }
}
