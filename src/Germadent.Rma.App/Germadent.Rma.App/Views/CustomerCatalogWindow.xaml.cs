using System.Windows;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for CustomerCatalogWindow.xaml
    /// </summary>
    public partial class CustomerCatalogWindow : Window
    {
        public static readonly DependencyProperty IsOpenedFromMainWindowProperty = DependencyProperty.Register(
            "IsOpenedFromMainWindow", typeof(bool), typeof(CustomerCatalogWindow), new PropertyMetadata(default(bool)));

        public bool IsOpenedFromMainWindow
        {
            get { return (bool) GetValue(IsOpenedFromMainWindowProperty); }
            set { SetValue(IsOpenedFromMainWindowProperty, value); }
        }

        public CustomerCatalogWindow()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            DataContextChanged+= OnDataContextChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchComboBox.FocusTextBox();

            IsOpenedFromMainWindow = Owner is MainWindow;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var customerCatalogViewModel = (ICustomerCatalogViewModel)DataContext;
            customerCatalogViewModel.Initialize();
        }
    }
}
