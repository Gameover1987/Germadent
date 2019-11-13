using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using System;
using System.Windows.Media.Imaging;
using Germadent.Rma.App.Model;
using Germadent.Common;
using Germadent.Rma.App.Properties;

namespace Germadent.Rma.App.ViewModels
{
    public class AuthorizationViewModel : AuthorizationViewModelBase
    {
        private readonly IRmaOperations _rmaOperations;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmaOperations rmaOperations)
            : base(agent)
        {
            _rmaOperations = rmaOperations;
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
                _rmaOperations.Authorize(UserName, Password);
            }
            catch (Exception e)
            {
                throw new UserMessageException(e.Message, e);
            }

            return true;
        }
    }
}
