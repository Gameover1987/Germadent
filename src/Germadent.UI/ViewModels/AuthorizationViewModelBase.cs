using Germadent.Common;
using Germadent.UI.Commands;
using Germadent.UI.Infrastructure;
using Germadent.UI.Properties;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Germadent.UI.ViewModels
{
    /// <summary>
    /// Базовый класс вьюмодели окна авторизации пользователя
    /// </summary>
    public abstract class AuthorizationViewModelBase : ViewModelBase, IAuthorizationViewModel
	{
		private string _userName;
		private string _password;
		private string _inputLanguage;
		private InputLanguageManager _languageManager;
		private bool? _dialogResult;
		private readonly IShowDialogAgent _agent;

        protected AuthorizationViewModelBase(IShowDialogAgent agent)
		{
			if (agent == null) throw new ArgumentNullException("agent");
			_agent = agent;

            OkCommand = new DelegateCommand(OkCommandHandler, CanOkCommandHandler);
		}

		/// <summary>
		/// Логин пользователя
		/// </summary>
		public string UserName
		{
			get { return _userName; }
			set
			{
				if (_userName == value)
					return;

				_userName = value;
				OnPropertyChanged(() => UserName);
			}
		}

		/// <summary>
		/// Список логинов для выпадашки
		/// </summary>
		public ObservableCollection<string> UserNames { get; } = new ObservableCollection<string>();

		/// <summary>
		/// Пароль пользователя
		/// </summary>
		public string Password
		{
			get { return _password; }
			set
			{
				if (_password == value)
					return;

				_password = value;
				OnPropertyChanged(() => Password);
			}
		}

		/// <summary>
		/// Язык ввода
		/// </summary>
		public string InputLanguage
		{
			get { return _inputLanguage; }
			set
			{
				if (_inputLanguage == value) return;
				_inputLanguage = value;
				OnPropertyChanged(() => InputLanguage);    
			}
		}

		/// <summary>
		/// Иконка приложения
		/// </summary>
		public ImageSource ApplicationIcon { get; set; }

		/// <summary>
		/// Название приложения
		/// </summary>
		public string ApplicationName { get; set; }

		/// <summary>
		/// Признак управления закрытием окна в случае успешной авторизации 
		/// </summary>
		public bool? DialogResult
		{
			get
			{
				return _dialogResult;
			}
			set
			{
				if (_dialogResult == value)
					return;
				_dialogResult = value;
				Close();
				OnPropertyChanged(() => DialogResult);
			}
		}

		public ICommand OkCommand { get; private set; }

        public void Initialize()
        {
            InitializeLanguage();

            foreach (var userName in GetUserNames())
            {
                UserNames.Add(userName);
            }
		}

        protected abstract string[] GetUserNames();

        private bool CanOkCommandHandler(object obj)
		{
			if (string.IsNullOrWhiteSpace(UserName))
				return false;

			return true;
		}

		private void OkCommandHandler(object obj)
		{
			if (!CanOkCommandHandler(obj)) 
				return;

			try
			{
				var result = Authorize();
				DialogResult = result;
			}
			catch (UserMessageException uex)
			{
				_agent.ShowMessageDialog(uex.Message, Resources.AuthorizationWindow_Title, MessageBoxButton.OK, MessageBoxImage.Error);
			}
			catch (Exception ex)
			{
				_agent.ShowMessageDialog(Resources.AuthorizationWindow_FaultExceptionMessage + "\r\n" + ex.Message, 
					Resources.AuthorizationWindow_Title, MessageBoxButton.OK, MessageBoxImage.Error);
				DialogResult = false;
			}
		}

		/// <summary>
		/// Выполнить авторизацию (аутентификацию)
		/// Всё что завёрнуто в UserMessageException обрабатывается, оторажается сообщение с текстом Message, закрытие окна авторизации отменяется 
		/// Остальные Exception тоже обрабатываются, окно авторизации закрывается
		/// </summary>
		/// <returns>true - авторизация прошла успешно, false - закрыть оконо авторизации с DialogResult = false</returns>
		protected abstract bool Authorize();

		/// <summary>
		/// Инициализация языка
		/// </summary>
		private void InitializeLanguage()
		{
			_languageManager = InputLanguageManager.Current;
			_languageManager.InputLanguageChanged += InputLanguageChanged;

			InputLanguageChanged(this, null);
		}

        private void InputLanguageChanged(object sender, InputLanguageEventArgs e)
		{
			InputLanguage = _languageManager.CurrentInputLanguage.TwoLetterISOLanguageName.ToUpper();
		}

		private void Close()
		{
			if (_languageManager != null)
				_languageManager.InputLanguageChanged -= InputLanguageChanged;
		}
	}
}
