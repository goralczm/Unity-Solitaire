using UnityEngine;
using UnityEngine.EventSystems;

public class DeckPile : CardPile, IPointerClickHandler
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            CycleCards();
    }

    protected override bool IsValidValue(CardValue value)
    {
        return false;
    }

    protected override bool IsValidSuit(CardSuit suit)
    {
        return false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Right)
            return;

        CycleCards();
    }

    public void CycleCards()
    {
        if (!HasCards || _cardsOnPile.Count == 1)
            return;

        NextCardCommand nextCardCommand = new NextCardCommand(this);
        GameFlow.Instance.RegisterCommand(nextCardCommand);
    }

    public void NextCard()
    {
        Card lastCard = PeekCard();
        RemoveCard(lastCard);
        lastCard.ShowReverse();
        lastCard.SetParent(transform, Vector2.zero);
        _cardsOnPile[0].SetParent(lastCard.transform, _cardsOffset);
        _cardsOnPile.Insert(0, lastCard);
    }

    public void LastCard()
    {
        PeekCard().ShowReverse();

        Card secondCard = _cardsOnPile[1];
        secondCard.SetParent(transform, Vector2.zero);

        Card firstCard = _cardsOnPile[0];
        _cardsOnPile.RemoveAt(0);
        firstCard.SetParent(PeekCard().transform, _cardsOffset);
        _cardsOnPile.Add(firstCard);
        firstCard.ShowFace();
    }
}
