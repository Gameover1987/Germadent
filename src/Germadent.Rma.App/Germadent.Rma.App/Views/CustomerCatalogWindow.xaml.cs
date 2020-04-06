using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for CustomerCatalogWindow.xaml
    /// </summary>
    public partial class CustomerCatalogWindow : Window
    {
        public CustomerCatalogWindow()
        {
            InitializeComponent();
        }

        private void CustomerCatalogWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var customerCatalogViewModel = (ICustomerCatalogViewModel)DataContext;
            customerCatalogViewModel.Initialize();
        }
    }
}
