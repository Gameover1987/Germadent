using System.Windows;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for ResponsiblePersonCatalogWindow.xaml
    /// </summary>
    public partial class ResponsiblePersonCatalogWindow : Window
    {
        private IResponsiblePersonCatalogViewModel _responsiblePerson;

        public static readonly DependencyProperty IsOpenedFromMainWindowProperty = DependencyProperty.Register(
            "IsOpenedFromMainWindow", typeof(bool), typeof(ResponsiblePersonCatalogWindow), new PropertyMetadata(default(bool)));

        public bool IsOpenedFromMainWindow
        {
            get { return (bool)GetValue(IsOpenedFromMainWindowProperty); }
            set { SetValue(IsOpenedFromMainWindowProperty, value); }
        }

        public ResponsiblePersonCatalogWindow()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
            DataContextChanged += ResponsiblePersonCatalogWindow_OnDataContextChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchComboBox.FocusTextBox();

            IsOpenedFromMainWindow = Owner is MainWindow;
        }

        private void ResponsiblePersonCatalogWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            //var responsiblePersonCatalog = (IResponsiblePersonCatalogViewModel)DataContext;
            //responsiblePersonCatalog.Initialize();
            _responsiblePerson = (IResponsiblePersonCatalogViewModel)DataContext;
            _responsiblePerson?.Initialize();
        }

        private void ResponsiblePersonRowMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (IsOpenedFromMainWindow)
                return;

            DialogResult = true;
        }
    }
}
