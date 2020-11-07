using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.Pricing;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public interface IToothCardContainer
    {
        IToothCardViewModel ToothCard { get; }
    }

    public class ToothSelectedEventArgs : EventArgs
    {
        public ToothSelectedEventArgs(ToothViewModel toothViewModel)
        {
            SelectedTooth = toothViewModel;
        }

        public ToothViewModel SelectedTooth { get; }
    }

    public interface IToothCardViewModel
    {
        /// <summary>
        /// Возвращает описание
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Зубы
        /// </summary>
        ObservableCollection<ToothViewModel> Teeth { get; }

        /// <summary>
        /// Выбранные зубы
        /// </summary>
        ToothViewModel[] SelectedTeeth { get; set; }

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="toothCard"></param>
        void Initialize(ToothDto[] toothCard);

        /// <summary>
        /// Возвращает модельный DTO
        /// </summary>
        /// <returns></returns>
        ToothDto[] ToDto();

        /// <summary>
        /// Копировать описание работ в буфер обмена
        /// </summary>
        ICommand CopyDescriptionCommand { get; }

        /// <summary>
        /// Выбрать мост
        /// </summary>
        ICommand SelectBridgeCommand { get; }

        /// <summary>
        /// Возвращает true если в зубной карте все указано правильно
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Возникает когда надо перерисовать зубную карту
        /// </summary>
        event EventHandler<ToothChangedEventArgs> ToothChanged;

        /// <summary>
        /// Происходит когда выбрали зуб
        /// </summary>
        event EventHandler<ToothSelectedEventArgs> ToothSelected;

        /// <summary>
        /// Происходит при очистке зуба
        /// </summary>
        event EventHandler<ToothCleanUpEventArgs> ToothCleanup; 

        /// <summary>
        /// Связывает прайс с выбранными зубами
        /// </summary>
        void AttachPricePositions(PricePositionViewModel[] pricePositions);
    }
}