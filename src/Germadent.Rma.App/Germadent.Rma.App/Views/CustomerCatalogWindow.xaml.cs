using System.Windows;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for CustomerCatalogWindow.xaml
    /// </summary>
    public partial class CustomerCatalogWindow : Window
    {
        public CustomerCatalogWindow()
        {
            InitializeComponent();

            Loaded+= OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchComboBox.FocusTextBox();
        }

        private void CustomerCatalogWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var customerCatalogViewModel = (ICustomerCatalogViewModel)DataContext;
            customerCatalogViewModel.Initialize();
        }
    }
}
