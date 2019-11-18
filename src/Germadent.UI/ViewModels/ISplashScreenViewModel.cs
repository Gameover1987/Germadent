using System.Windows.Media;

namespace Germadent.UI.ViewModels
{
    /// <summary>
    /// Интерфейс вьюмодели заставки приложения при запуске
    /// </summary>
    public interface ISplashScreenViewModel
    {
        /// <summary>
        /// Название приложения
        /// </summary>
        string ApplicationName { get; }

        /// <summary>
        /// Версия приложения
        /// </summary>
        string ApplicationVersion { get; }

        /// <summary>
        /// Иконка приложения
        /// </summary>
        ImageSource ApplicationIcon { get; }

        /// <summary>
        /// год и название компании
        /// </summary>
        string Copyright { get; }

        /// <summary>
        /// Название и версия комплекса
        /// </summary>
        string ApkVersion { get; }

        /// <summary>
        /// Текущий загружаемый модуль
        /// </summary>
        string CurrentLoadingModuleName { get; }
    }
}