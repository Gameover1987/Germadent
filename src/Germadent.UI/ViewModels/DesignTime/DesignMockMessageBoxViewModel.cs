using System.Windows;

namespace Germadent.UI.ViewModels.DesignTime
{
    public class DesignMockMessageBoxViewModel : MessageBoxViewModel
    {
        public DesignMockMessageBoxViewModel()
            : base("Название приложения", "Текст сообщения", MessageBoxButton.YesNoCancel, MessageBoxImage.Information)
        {
           
        }
    }
}