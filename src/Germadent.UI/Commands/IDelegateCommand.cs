using System;
using System.Windows.Input;

namespace Germadent.UI.Commands
{
    public interface IDelegateCommand : ICommand
    {
        void NotifyCanExecuteChanged();
        event EventHandler Executed;

        bool TryExecute();

        bool TryExecute(object obj);

        bool CanExecute();

        void Execute();
    }
}
