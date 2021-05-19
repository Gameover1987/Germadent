using System.Windows.Controls;
using System.Windows.Input;
using Germadent.UserManagementCenter.App.ViewModels;

namespace Germadent.UserManagementCenter.App.Views
{
    /// <summary>
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : UserControl
    {
        public UsersView()
        {
            InitializeComponent();
        }

        private void ListBoxItem_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                var usersManager = (IUsersManagerViewModel) DataContext;
                usersManager.EditUSerCommand.TryExecute();
            }
        }
    }
}
