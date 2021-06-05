using System;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.Rms.App.Infrastructure;
using Germadent.Rms.App.Properties;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IRmsServiceClient _serviceClient;
        private readonly IRmsUserSettingsManager _userSettingsManager;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmsServiceClient serviceClient, IRmsUserSettingsManager userSettingsManager)
            : base(agent)
        {
            _serviceClient = serviceClient;
            _userSettingsManager = userSettingsManager;

            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();

            UserName = _userSettingsManager.LastLogin;
        }

        private BitmapImage GetApplicationIcon()
        {
            return new BitmapImage(new Uri(
                "pack://application:,,,/Germadent.Rms.App;component/logo.png",
                UriKind.Absolute));
        }

        protected override bool Authorize()
        {
            try
            {
                _serviceClient.Authorize(UserName, Password);
                _userSettingsManager.LastLogin = UserName;
                _userSettingsManager.Save();
            }
            catch (Exception e)
            {
                throw new UserMessageException(e.Message, e);
            }

            return true;
        }
    }
}
