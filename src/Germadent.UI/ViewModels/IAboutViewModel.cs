using System.Windows.Input;
using System.Windows.Media;

namespace Germadent.UI.ViewModels
{
	/// <summary>
	/// Интерфейс вью-модели 'О программе'
	/// </summary>
	public interface IAboutViewModel
	{
		/// <summary>
		/// Масштаб окна, по умолчанию 1.0
		/// </summary>
		double UiScale { get; set; }

		/// <summary>
		/// Иконка приложения
		/// </summary>
		ImageSource ApplicationIcon { get; set; }

		/// <summary>
		/// Название приложения
		/// </summary>
		string ApplicationName { get; set; }

		/// <summary>
		/// Версия приложения
		/// </summary>
		string ApplicationVersion { get; set; }

		/// <summary>
		/// Дополнительная информация
		/// </summary>
		string AdditionalInfo { get; set; }

		/// <summary>
		/// год и название компании
		/// </summary>
		string Copyright { get; set; }

		/// <summary>
		/// Телефон
		/// </summary>
		string Telephone { get; set; }

		/// <summary>
		/// Телефон техподдержки
		/// </summary>
		string TelephoneSupport { get; set; }

		/// <summary>
		/// Ссылка на сайт компании
		/// </summary>
		string SiteUrl { get; set; }

		/// <summary>
		/// е-mail
		/// </summary>
		string Email { get; set; }

		/// <summary>
		/// Перейти к email
		/// </summary>
		ICommand GoToEmailCommand { get; }

		/// <summary>
		/// Перйти к сайту
		/// </summary>
		ICommand GoToSiteUrlCommand { get; }
	}
}
