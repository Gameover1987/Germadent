using System;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using Germadent.UserManagementCenter.App.Properties;
using Germadent.UserManagementCenter.App.ServiceClient;

namespace Germadent.UserManagementCenter.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IUmcServiceClient _serviceClient;

        public AuthorizationViewModel(IShowDialogAgent agent, IUmcServiceClient serviceClient)
            : base(agent)
        {
            _serviceClient = serviceClient;

            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();
        }

        private BitmapImage GetApplicationIcon()
        {
            return new BitmapImage(new Uri(
                "pack://application:,,,/Germadent.UserManagementCenter.App;component/logo_umc.png",
                UriKind.Absolute));
        }

        protected override string[] GetUserNames()
        {
            return new string[0];
        }

        protected override bool Authorize()
        {
            try
            {
                _serviceClient.Authorize(UserName, Password);
            }
            catch (Exception e)
            {
                throw new UserMessageException(e.Message, e);
            }

            return true;
        }
    }
}
