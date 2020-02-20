using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IMainViewModel _mainViewModel;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnOrderRowDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var mainViewModel = (IMainViewModel) DataContext;
            mainViewModel.OpenOrderCommand.TryExecute();
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mainViewModel = (IMainViewModel) DataContext;
            _mainViewModel.Initialize();
        }

        private void OrdersGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OrdersGrid.ScrollIntoView(_mainViewModel.SelectedOrder);
        }
    }
}
