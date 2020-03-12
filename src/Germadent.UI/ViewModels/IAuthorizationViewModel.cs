using System.Windows.Media;

namespace Germadent.UI.ViewModels
{
	/// <summary>
	/// Интерфейс вьюмодели окна авторизации пользователя
	/// </summary>
	public interface IAuthorizationViewModel
	{
		/// <summary>
		/// Логин пользователя
		/// </summary>
		string UserName { get; set; }

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		string Password { get; set; }
		
		/// <summary>
		/// Иконка приложения
		/// </summary>
		ImageSource ApplicationIcon { get; set; }

		/// <summary>
		/// Название приложения
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// Признак управления закрытием окна в случае успешной авторизации 
		/// </summary>
		bool? DialogResult { get; set; }
	}
}