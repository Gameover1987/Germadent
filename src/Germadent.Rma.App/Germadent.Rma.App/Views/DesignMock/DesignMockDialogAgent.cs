using System.Windows;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockDialogAgent : IShowDialogAgent
    {
        public string DefaultWindowTitle { get; set; }
        public bool? ShowDialog<T>(object dialogViewModel) where T : Window, new()
        {
            throw new System.NotImplementedException();
        }

        public bool? ShowDialog<T>(object dialogViewModel, IWindow owner) where T : Window, new()
        {
            throw new System.NotImplementedException();
        }

        public IWindow Show<T>(object viewModel) where T : Window, IWindow, new()
        {
            throw new System.NotImplementedException();
        }

        public IWindow Show<T>(object viewModel, IWindow owner) where T : Window, IWindow, new()
        {
            throw new System.NotImplementedException();
        }

        public MessageBoxResult ShowMessageDialog(string message, string caption, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Asterisk)
        {
            throw new System.NotImplementedException();
        }

        public MessageBoxResult ShowMessageDialog(string message, MessageBoxButton button = MessageBoxButton.OK,
            MessageBoxImage icon = MessageBoxImage.Asterisk)
        {
            throw new System.NotImplementedException();
        }

        public MessageBoxResult ShowMessageDialog(string message, string caption, MessageBoxButton button, MessageBoxImage icon,
            MessageBoxResult defaultButton, MessageBoxOptions options)
        {
            throw new System.NotImplementedException();
        }

        public MessageBoxResult ShowMessageDialog(IWindow owner, string message, string caption = null,
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Asterisk,
            MessageBoxResult defaultButton = MessageBoxResult.OK, MessageBoxOptions options = MessageBoxOptions.None)
        {
            throw new System.NotImplementedException();
        }

        public void ShowErrorMessageDialog(string message, string details, string caption = null)
        {
            throw new System.NotImplementedException();
        }

        public void ShowErrorMessageDialog(IWindow owner, string message, string details, string caption = null)
        {
            throw new System.NotImplementedException();
        }

        public bool? ShowOpenFileDialog(string filter, out string fileName, IWindow owner = null)
        {
            throw new System.NotImplementedException();
        }

        public bool? ShowSaveFileDialog(string filter, string defFileName, out string fileName, IWindow owner = null)
        {
            throw new System.NotImplementedException();
        }
    }
}