using Germadent.UI.Helpers;
using Germadent.UI.ViewModels;
using System.ComponentModel;
using System.Windows;

namespace Germadent.UI.Windows
{
    /// <summary>
    /// Окно входа в систему с использованием логина и пароля
    /// </summary>
	public partial class AuthorizationWindow : BaseDialogWindow
    {
	    public AuthorizationWindow()
	    {
		    InitializeComponent();
	    }

	    private void UserNameTextBoxDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	    {
		    var authorizationViewModel = (AuthorizationViewModelBase) e.NewValue;
		    if (string.IsNullOrEmpty(authorizationViewModel.UserName))
			    UserNameTextBox.Focus();
		    else
			    PasswordTextBox.Focus();
	    }

	    private void OkButtonClick(object sender, RoutedEventArgs e)
	    {
		    UserNameTextBox.Focus();
	    }

	    private void AuthorizationWindowClosing(object sender, CancelEventArgs cancelEventArgs)
	    {
		    WindowProperties.SetDialogResult(this, DialogResult.HasValue && DialogResult.Value);
	    }
    }
}
