using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOrderRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mainViewModel = (IMainViewModel) DataContext;
            mainViewModel.OpenOrderCommand.TryExecute();
        }
    }
}
