using System.Windows;

namespace Germadent.UI.Infrastructure
{
    /// <summary>
    /// Интерфейс агента, поднимающего диалоги.
    /// </summary>
    public interface IShowDialogAgent
	{
		/// <summary>
		/// Заголовок окна по умолчанию для отображения в ShowMessageDialog, если не удалось получить активное окно
		/// </summary>
		string DefaultWindowTitle { get; set; }

		/// <summary>
		/// Показать диалог.
		/// </summary>
		/// <param name="dialogViewModel">ViewModel диалога.</param>		
		/// <typeparam name="T">Тип View диалога.</typeparam>
		/// <returns>
		/// Возвращаемое значение такое же как у System.Windows.Window.ShowDialog().
		/// </returns>
		bool? ShowDialog<T>(object dialogViewModel) where T : Window, new();

		/// <summary>
		/// Показать диалог.
		/// </summary>
		/// <param name="dialogViewModel">ViewModel диалога.</param>
		/// <param name="owner">Владелец создаваемого окна, возможно null</param>
		/// <typeparam name="T">Тип View диалога.</typeparam>
		/// <returns>
		/// Возвращаемое значение такое же как у System.Windows.Window.ShowDialog().
		/// </returns>
		bool? ShowDialog<T>(object dialogViewModel, IWindow owner) where T : Window, new();

		/// <summary>
		/// Показать окно в немодальном режиме, owner у окна указывается текущее активное окно
		/// </summary>
		/// <param name="viewModel">ViewModel окна.</param>
		/// <returns>Интерфейс окна для управления закрытием</returns>
		IWindow Show<T>(object viewModel)
			where T : Window, IWindow, new();

		/// <summary>
		/// Показать окно в немодальном режиме
		/// </summary>
		/// <typeparam name="T">Тип View для окна.</typeparam>
		/// <param name="viewModel">ViewModel окна.</param>
		/// <param name="owner">Владелец создаваемого окна</param>
		/// <returns>Интерфейс окна для управления закрытием</returns>
		IWindow Show<T>(object viewModel, IWindow owner)
			where T : Window, IWindow, new();

        /// <summary>
        /// Показать MessageBox.
        /// </summary>
        /// <param name="message">Текст для отображения.</param>
        /// <param name="caption">Заголовок. если пустой то берётся заголовок активного окна</param>
        /// <param name="button">
        /// Параметр, определяющий какие кнопки должен содержать MessageBox.
        /// </param>
        /// <param name="icon">Иконка для отображения.</param>
        /// <returns>
        /// MessageBoxResult определяет какую кнопку нажал пользователь.
        /// </returns>
        MessageBoxResult ShowMessageDialog(string message, string caption,
            MessageBoxButton button = MessageBoxButton.OK, MessageBoxImage icon = MessageBoxImage.Information);

		/// <summary>
		/// Показать MessageBox. с установкой Caption из текущего активного окна
		/// </summary>
		/// <param name="message">Текст для отображения.</param>
		/// <param name="button">
		/// Параметр, определяющий какие кнопки должен содержать MessageBox.
		/// </param>
		/// <param name="icon">Иконка для отображения.</param>
		/// <returns>
		/// MessageBoxResult определяет какую кнопку нажал пользователь.
		/// </returns>
		MessageBoxResult ShowMessageDialog(string message, MessageBoxButton button = MessageBoxButton.OK,
			MessageBoxImage icon = MessageBoxImage.Information);

        /// <summary>
        /// Показать MessageBox.
        /// </summary>
        /// <param name="message">Текст для отображения.</param>
        /// <param name="caption">Заголовок. если пустой то берётся заголовок активного окна</param>
        /// <param name="button">
        /// Параметр, определяющий какие кнопки должен содержать MessageBox.
        /// </param>
        /// <param name="icon">Иконка для отображения.</param>
        /// <param name="defaultButton">Параметр определяющий какая кнопка будет выбрана по умолчанию</param>
        /// <param name="options">Specifies special display options for a message box.</param>
        /// <returns>
        /// MessageBoxResult определяет какую кнопку нажал пользователь.
        /// </returns>
        MessageBoxResult ShowMessageDialog(string message, string caption,
            MessageBoxButton button, MessageBoxImage icon,
            MessageBoxResult defaultButton, MessageBoxOptions options);

		/// <summary>
		/// Показать MessageBox.
		/// </summary>
		/// <param name="owner">Владелец окна, возможно null</param>
		/// <param name="message">Текст для отображения.</param>
		/// <param name="caption">Заголовок. если пустой то берётся заголовок активного окна</param>
		/// <param name="button">
		/// Параметр, определяющий какие кнопки должен содержать MessageBox.
		/// </param>
		/// <param name="icon">Иконка для отображения.</param>
		/// <param name="defaultButton">Параметр определяющий какая кнопка будет выбрана по умолчанию</param>
		/// <param name="options">Specifies special display options for a message box.</param>
		/// <returns>
		/// MessageBoxResult определяет какую кнопку нажал пользователь.
		/// </returns>
		MessageBoxResult ShowMessageDialog(
			IWindow owner, 
			string message, 
			string caption = null,
			MessageBoxButton button = MessageBoxButton.OK, 
			MessageBoxImage icon = MessageBoxImage.Information, 
			MessageBoxResult defaultButton = MessageBoxResult.OK,
			MessageBoxOptions options = MessageBoxOptions.None);

		/// <summary>
		/// Показать диалог с сообщением об ошибке для активного окна.
		/// </summary>
		void ShowErrorMessageDialog(string message, string details, string caption = null);

		/// <summary>
		/// Показать диалог с сообщением об ошибке с указанием owner.
		/// </summary>
		void ShowErrorMessageDialog(IWindow owner, string message, string details, string caption = null);

		/// <summary>
		/// Показать диалог открытия файла
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="fileName"></param>
		/// <param name="owner">Владелец создаваемого окна</param>
		/// <returns></returns>
		bool? ShowOpenFileDialog(string filter, out string fileName, IWindow owner = null);

		/// <summary>
		/// Показать диалог сохранения файла
		/// </summary>
		/// <param name="filter"></param>
		/// <param name="defFileName"></param>
		/// <param name="fileName"></param>
		/// <param name="owner">Владелец создаваемого окна</param>
		/// <returns></returns>
		bool? ShowSaveFileDialog(string filter, string defFileName, out string fileName, IWindow owner = null);

		/// <summary>
		/// Простое окно для ввода строки
		/// </summary>
		/// <param name="title">Заголовок окна</param>
		/// <param name="parameterName">Название параметра</param>
		/// <param name="inputString">Начальное значение параметра</param>
		/// <returns></returns>
        string ShowInputBox(string title, string parameterName, string inputString = null);
    }
}
