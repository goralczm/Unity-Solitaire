using UnityEngine;
using Utilities.CommandPattern;

public class MoveCardCommand : ICommand
{
    public bool IsFinished => true;

    private readonly CardPile _originalCardPile;
    private readonly CardPile _newCardPile;
    private readonly Card _cardToMove;
    private readonly bool _wasParentShown;

    public MoveCardCommand(CardPile originalCardPile, CardPile newCardPile, Card cardToMove)
    {
        _originalCardPile = originalCardPile;
        _newCardPile = newCardPile;
        _cardToMove = cardToMove;
        if (cardToMove.GetParent().TryGetComponent(out Card parentCard))
            _wasParentShown = parentCard.IsShown;
    }

    public void Execute()
    {
        _newCardPile.AddCardToPile(_cardToMove);
    }

    public void Tick()
    {

    }

    public void Undo()
    {
        if (!_wasParentShown)
            _originalCardPile.PeekCard()?.ShowReverse();
        else
            _originalCardPile.PeekCard()?.ShowFace();
        _originalCardPile.AddCardToPile(_cardToMove);
    }
}
