using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.TechnologyOperation;

namespace Germadent.Rma.App.Views.TechnologyOperation
{
    /// <summary>
    /// Interaction logic for AddTechnologyOperationWindow.xaml
    /// </summary>
    public partial class AddTechnologyOperationWindow : Window
    {
        public AddTechnologyOperationWindow()
        {
            InitializeComponent();
        }

        private void OnRateMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var addTechOperationViewModel = (IAddTechnologyOperationViewModel) DataContext;
            addTechOperationViewModel.EditRateCommand.TryExecute();
        }
    }
}
