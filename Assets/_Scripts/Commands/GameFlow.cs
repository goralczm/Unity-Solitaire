using UnityEngine;
using Utilities.CommandPattern;

public class GameFlow : CommandProcessor
{
    public static GameFlow Instance;

    private void Awake()
    {
        Instance = this;
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
            UndoCommand();

        if (Input.GetKeyDown(KeyCode.Y))
            RedoCommand();

        base.Update();
    }

    public void UndoCommand()
    {
        UndoLastCommand();
    }

    public void RedoCommand()
    {
        RedoLastUndo();
    }

    public void RegisterCommand(ICommand command)
    {
        EnqueueCommand(command);
    }
}
