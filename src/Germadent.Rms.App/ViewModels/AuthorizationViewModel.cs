using System;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.Rms.App.Properties;
using Germadent.Rms.App.ServiceClient;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;

namespace Germadent.Rms.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IRmsServiceClient _serviceClient;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmsServiceClient serviceClient)
            : base(agent)
        {
            _serviceClient = serviceClient;

            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();

#if DEBUG
            UserName = "slava";
            Password = "123";
#endif
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
            }
            catch (Exception e)
            {
                throw new UserMessageException(e.Message, e);
            }

            return true;
        }
    }
}
