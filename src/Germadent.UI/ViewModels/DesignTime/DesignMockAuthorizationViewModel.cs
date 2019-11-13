using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Germadent.UI.ViewModels.DesignTime
{
	internal class DesignMockAuthorizationViewModel : AuthorizationViewModelBase
	{
		public DesignMockAuthorizationViewModel() 
            : base(new DesignMockShowDialogAgent())
		{
			ApplicationName = "Новое приложение";
			ApplicationIcon =
				new BitmapImage(new Uri("pack://application:,,,/Wpf.DemoApp;component/Resources/AppIcon.ico", UriKind.Absolute));
			UserName = "admin";
			Password = "123456";
		}

		protected override bool Authorize()
		{
			return true;
		}
	}
}
