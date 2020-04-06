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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Germadent.Rma.App.Views.UserControls
{
    /// <summary>
    /// Interaction logic for CustomerUserControl.xaml
    /// </summary>
    public partial class CustomerUserControl : UserControl
    {
        public static readonly DependencyProperty ShowCustomerCardCommandProperty = DependencyProperty.Register(
            "ShowCustomerCardCommand", typeof(ICommand), typeof(CustomerUserControl), new PropertyMetadata(default(ICommand)));

        public CustomerUserControl()
        {
            InitializeComponent();
        }

        public ICommand ShowCustomerCardCommand
        {
            get { return (ICommand) GetValue(ShowCustomerCardCommandProperty); }
            set { SetValue(ShowCustomerCardCommandProperty, value); }
        }
    }
}
