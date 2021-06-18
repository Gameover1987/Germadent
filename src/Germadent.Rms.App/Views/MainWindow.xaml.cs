using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Germadent.Rms.App.ViewModels;

namespace Germadent.Rms.App.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
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
            var mainViewModel = (IMainViewModel)DataContext;
            mainViewModel.BeginWorkByWorkOrderCommand.TryExecute();
        }

        private void MainWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            _mainViewModel = (IMainViewModel)DataContext;
            _mainViewModel.Initialize();
        }

        private void OrdersGrid_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_mainViewModel.SelectedOrder == null)
                return;

            OrdersGrid.ScrollIntoView(_mainViewModel.SelectedOrder);
        }
    }
}