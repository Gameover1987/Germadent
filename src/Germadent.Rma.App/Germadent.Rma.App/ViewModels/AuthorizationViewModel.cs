﻿using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels;
using System;
using System.Windows.Media.Imaging;
using Germadent.Common;
using Germadent.Rma.App.Properties;
using Germadent.Rma.App.ServiceClient;

namespace Germadent.Rma.App.ViewModels
{
    public interface IAuthorizationViewModel
    { }

    public class AuthorizationViewModel : AuthorizationViewModelBase, IAuthorizationViewModel
    {
        private readonly IRmaServiceClient _serviceClient;

        public AuthorizationViewModel(IShowDialogAgent agent, IRmaServiceClient serviceClient)
            : base(agent)
        {
            _serviceClient = serviceClient;
            
            ApplicationName = Resources.AppTitle;
            ApplicationIcon = GetApplicationIcon();
        }

        private BitmapImage GetApplicationIcon()
        {
            try
            {
                return null;
                //return new BitmapImage(new Uri(
                //    "pack://application:,,,/Germadent.Rma.App;component/logo.png",
                //    UriKind.Absolute));
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
