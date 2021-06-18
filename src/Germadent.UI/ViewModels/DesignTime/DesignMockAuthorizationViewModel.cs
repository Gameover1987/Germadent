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

        protected override string[] GetUserNames()
        {
            return new string[0];
        }

        protected override bool Authorize()
		{
			return true;
		}
	}
}
