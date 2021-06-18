using System.Windows.Controls;
using System.Windows.Input;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views
{
    /// <summary>
    /// Interaction logic for RolesView.xaml
    /// </summary>
    public partial class RolesView : UserControl
    {
        public RolesView()
        {
            InitializeComponent();
        }

        private void ListBoxItem_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var usersManager = (IRolesManagerViewModel)DataContext;
                usersManager.EditRoleCommand.TryExecute();
            }
        }
    }
}
