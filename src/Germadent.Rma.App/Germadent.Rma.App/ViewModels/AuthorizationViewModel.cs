using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using System;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.Rma.App.Properties;
using Germadent.Rma.App.ServiceClient;

namespace Germadent.Rma.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IRmaAuthorizer _authorizer;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmaAuthorizer authorizer)
            : base(agent)
        {
            _authorizer = authorizer;
            
            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();
        }

        private BitmapImage GetApplicationIcon()
        {
            try
            {
                return new BitmapImage(new Uri(
                    "pack://application:,,,/Germadent.Rma.App;component/logo.png",
                    UriKind.Absolute));
            }
            catch
            {
                return null;
            }
        }

        protected override bool Authorize()
        {
            try
            {
                _authorizer.Authorize(UserName, Password);
            }
            catch (Exception e)
            {
                throw new UserMessageException(e.Message, e);
            }

            return true;
        }
    }
}
