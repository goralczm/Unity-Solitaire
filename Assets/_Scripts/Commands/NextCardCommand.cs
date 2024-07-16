using UnityEngine;
using Utilities.CommandPattern;

public class NextCardCommand : ICommand
{
    public bool IsFinished => true;

    private readonly DeckPile _deckPile;

    public NextCardCommand(DeckPile deckPile)
    {
        _deckPile = deckPile;
    }

    public void Execute()
    {
        _deckPile.NextCard();
    }

    public void Tick()
    {

    }

    public void Undo()
    {
        _deckPile.LastCard();
    }
}
