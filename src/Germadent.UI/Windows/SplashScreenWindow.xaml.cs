using Germadent.UI.Infrastructure;
using System.Windows;

namespace Germadent.UI.Windows
{
	/// <summary>
	/// Заставка приложения при запуске 
	/// Поддерживает ISplashScreenViewModel
	/// </summary>
	public partial class SplashScreenWindow : Window, IWindow
	{
		public SplashScreenWindow()
		{
			InitializeComponent();
		}
	}
}
