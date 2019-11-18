using System;
using System.Windows.Media.Imaging;

namespace Germadent.UI.ViewModels.DesignTime
{
	internal class DesignMockAboutViewModel: AboutViewModel
	{
		public DesignMockAboutViewModel()
		{
			ApplicationIcon = new BitmapImage(new Uri(@"../Resources/AppIcon.ico", UriKind.Relative));
			ApplicationName = "Имя приложения";
			ApplicationVersion = "1.2.3";
			AdditionalInfo = "Версия ядра 1.2";
			Copyright = "2014 DesignTimeCompany";
			Telephone = "+7 (888) 333-88-99 (многоканальный)";
			TelephoneSupport = "+7 (888) 363-20-02 (техподдержка)";
			SiteUrl = @"http://www.site.com";
			Email = "mail@site.com";
		}
	}
}
