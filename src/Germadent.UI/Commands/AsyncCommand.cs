using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Germadent.UI.Commands
{
    /// <summary>
    /// Класс описывает базовую реализацию асинхроной команды
    /// </summary>
    public class AsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> _command;

        public AsyncCommand(Func<Task> command)
        {
            _command = command;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// Определяет метод, асинхроного исполения команды
        /// </summary>
        public Task ExecuteAsync(object parameter)
        {
            try
            {
                return _command();
            }
            catch (Exception exception)
            {
                var aaa = exception.StackTrace;
                return null;
            }
        }

        /// <summary>
        /// Реализация синхроного исполнения команды
        /// </summary>
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }

        /// <summary>
        /// Событие изменения источика команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Заставляет CommandManager сгенерировать событие RequerySuggested.
        /// </summary>
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
