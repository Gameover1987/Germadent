using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Germadent.UI.Commands;

namespace Germadent.UI.ViewModels
{
    /// <summary>
    /// вью-модель 'О программе'
    /// </summary>
    public class AboutViewModel: ViewModelBase, IAboutViewModel
    {
        public AboutViewModel()
        {
            UiScale = 1.0;
            GoToEmailCommand = new DelegateCommand(GoToEmailCommandHandler, CanGoToEmailCommandHandler);
            GoToSiteUrlCommand = new DelegateCommand(GoToSiteUrlCommandHandler, CanGoToSiteUrlCommandHandler);
        }

        /// <summary>
        /// Масштаб окна, по умолчанию 1.0
        /// </summary>
        public double UiScale { get; set; }

        /// <summary>
        /// Иконка приложения
        /// </summary>
        public ImageSource ApplicationIcon { get; set; }

        /// <summary>
        /// Название приложения
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Версия приложения
        /// </summary>
        public string ApplicationVersion { get; set; }

        /// <summary>
        /// Дополнительная информация
        /// </summary>
        public string AdditionalInfo { get; set; }

        /// <summary>
        /// год и название компании
        /// </summary>
        public string Copyright { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Телефон техподдержки
        /// </summary>
        public string TelephoneSupport { get; set; }

        /// <summary>
        /// Ссылка на сайт компании
        /// </summary>
        public string SiteUrl { get; set; }

        /// <summary>
        /// е-mail
        /// </summary>
        public string Email { get; set; }

        public ICommand GoToEmailCommand { get; private set; }

        public ICommand GoToSiteUrlCommand { get; private set; }

        private bool CanGoToSiteUrlCommandHandler(object o)
        {
            return !string.IsNullOrWhiteSpace(SiteUrl);
        }

        /// <summary>
        /// Обработка вызова перехода к сайту
        /// </summary>
        /// <param name="o"></param>
        protected virtual void GoToSiteUrlCommandHandler(object o)
        {
            if (!CanGoToSiteUrlCommandHandler(o))
                return;
            GoToUrl(SiteUrl);
        }

        private bool CanGoToEmailCommandHandler(object o)
        {
            return !string.IsNullOrWhiteSpace(Email);
        }

        /// <summary>
        /// Обработка вызова перехода к email
        /// </summary>
        /// <param name="o"></param>
        protected virtual void GoToEmailCommandHandler(object o)
        {
            if (!CanGoToEmailCommandHandler(o))
                return;
            GoToUrl("mailto:" + Email);
        }

        /// <summary>
        /// Переход по ссылке при помощи класса Process
        /// </summary>
        /// <param name="url"></param>
        protected virtual void GoToUrl(string url)
        {
            try
            {
                var process = new Process { StartInfo = new ProcessStartInfo(url) };
                process.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Не удалось открыть адрес {0}\r\n\r\n{1}", url, ex.Message),
                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}