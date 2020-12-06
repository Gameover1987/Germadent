using System.Threading.Tasks;
using System.Windows.Input;

namespace Germadent.UI.Commands
{
    /// <summary>
    /// Интерфейс асинхронной команды
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}
