using System;
using System.Linq;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.Rma.App.Infrastructure;
using Germadent.Rma.App.Properties;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IRmaServiceClient _serviceClient;
        private readonly IRmaUserSettingsManager _userSettingsManager;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmaServiceClient serviceClient, IRmaUserSettingsManager userSettingsManager)
            : base(agent)
        {
            _serviceClient = serviceClient;
            _userSettingsManager = userSettingsManager;

            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();

            UserName = userSettingsManager.LastLogin;
        }

        private BitmapImage GetApplicationIcon()
        {
            return new BitmapImage(new Uri(
                "pack://application:,,,/Germadent.Rma.App;component/logo.png",
                UriKind.Absolute));
        }

        protected override string[] GetUserNames()
        {
            return _userSettingsManager.UserNames.OrderBy(x => x).ToArray();
        }

        protected override bool Authorize()
        {
            try
            {
                _serviceClient.Authorize(UserName, Password);
                _userSettingsManager.LastLogin = UserName;
                _userSettingsManager.UserNames.Add(UserName);
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
