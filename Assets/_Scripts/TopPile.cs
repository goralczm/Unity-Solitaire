public class TopPile : CardPile
{
    public override void AddCardToPile(CardDisplay card)
    {
        if (_cardsOnPile.Count == 0)
        {
            if ((int)card.Card.Value != 12)
                return;
        }    

        base.AddCardToPile(card);
    }
}
