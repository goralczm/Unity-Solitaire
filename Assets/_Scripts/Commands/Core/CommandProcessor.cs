using System.Collections.Generic;
using UnityEngine;

namespace Utilities.CommandPattern
{
    public class CommandProcessor : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool _allowUndoAndRedo = true;
        [SerializeField] private int _maxHistoryCount = 15;

        private Queue<ICommand> _queuedCommands = new Queue<ICommand>();
        private OpenStack<ICommand> _executedHistory = new OpenStack<ICommand>();
        private Stack<ICommand> _undoHistory = new Stack<ICommand>();

        private ICommand _currentCommand;

        public virtual void Update()
        {
            ProcessCommands();
        }

        protected virtual void ProcessCommands()
        {
            if (_queuedCommands.Count == 0)
                return;

            if (!IsCurrentCommandFinished())
            {
                _currentCommand.Tick();
                return;
            }

            SetAndExecuteCommand(_queuedCommands.Dequeue());
        }

        private void SetAndExecuteCommand(ICommand command)
        {
            _currentCommand = command;
            ExecuteCommand(_currentCommand);
        }

        private void ExecuteCommand(ICommand command)
        {
            command.Execute();
            command.Tick();

            if (_executedHistory.Count == _maxHistoryCount)
                _executedHistory.PopLast();

            _executedHistory.PushOnTop(command);
        }

        public void CancelCurrentCommand()
        {
            _currentCommand = null;
        }

        public void EnqueueCommand(ICommand command)
        {
            _queuedCommands.Enqueue(command);
            _undoHistory.Clear();
        }

        public void UndoLastCommand()
        {
            if (!_allowUndoAndRedo)
                return;

            if (_executedHistory.Count == 0)
                return;

            _currentCommand = _executedHistory.PopFirst();
            _currentCommand.Undo();
            _undoHistory.Push(_currentCommand);
        }

        public void RedoLastUndo()
        {
            if (!_allowUndoAndRedo)
                return;

            if (_undoHistory.Count == 0)
                return;

            SetAndExecuteCommand(_undoHistory.Pop());
        }

        public bool IsCurrentCommandFinished()
        {
            return _currentCommand == null || _currentCommand.IsFinished;
        }

        public bool HasQueuedCommands()
        {
            return _queuedCommands.Count > 0;
        }
    }
}