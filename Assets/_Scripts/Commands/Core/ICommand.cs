namespace Utilities.CommandPattern
{
    public interface ICommand
    {
        /// <summary>
        /// Executed once a command is initiated.
        /// </summary>
        public void Execute();

        /// <summary>
        /// Executed every frame until the command is finished.
        /// </summary>
        public void Tick();

        /// <summary>
        /// Undos the command procedure.
        /// </summary>
        public void Undo();

        /// <summary>
        /// Has the command been executed and finished?
        /// </summary>
        public bool IsFinished { get; }
    }
}
