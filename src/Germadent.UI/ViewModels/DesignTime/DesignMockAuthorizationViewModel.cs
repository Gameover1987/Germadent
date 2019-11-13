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
			UserName = "admin";
			Password = "123456";
		}

		protected override bool Authorize()
		{
			return true;
		}
	}
}
