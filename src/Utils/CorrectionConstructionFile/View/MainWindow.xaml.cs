using System.Windows;
using System.Windows.Input;
using Germadent.CorrectionConstructionFile.App.ViewModel;

namespace Germadent.CorrectionConstructionFile.App.View
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

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            var mainViewModel = (IMainViewModel) DataContext;
            mainViewModel.EditDictionaryItemCommand.TryExecute();
        }
    }
}
