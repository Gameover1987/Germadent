using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for ResponsiblePersonCatalogWindow.xaml
    /// </summary>
    public partial class ResponsiblePersonCatalogWindow : Window
    {
        public ResponsiblePersonCatalogWindow()
        {
            InitializeComponent();
            
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            SearchComboBox.FocusTextBox();
        }

        private void ResponsiblePersonCatalogWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var responsiblePersonCatalog = (IResponsiblePersonCatalogViewModel)DataContext;
            responsiblePersonCatalog.Initialize();
        }
    }
}
