using System.Windows;
using Germadent.Rma.App.ViewModels;

namespace Germadent.Rma.App.Views
{
    /// <summary>
    /// Interaction logic for OrdersFilterWindow.xaml
    /// </summary>
    public partial class OrdersFilterWindow : Window
    {
        public OrdersFilterWindow()
        {
            InitializeComponent();
        }

        private void OrdersFilterWindow_OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var filter = (IOrdersFilterViewModel) DataContext;
            filter.Initialize();
        }
    }
}
