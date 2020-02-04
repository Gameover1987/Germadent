using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.ToothCard
{
    public interface IToothCardContainer
    {
        IToothCardViewModel ToothCard { get; }
    }

    public interface IToothCardViewModel
    {
        /// <summary>
        /// Возвращает описание
        /// </summary>
        string Description { get; }

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
        /// Возникает когда надо перерисовать зубную карту
        /// </summary>
        event EventHandler<EventArgs> RenderRequest;

        /// <summary>
        /// Копировать описание работ в буфер обмена
        /// </summary>
        ICommand CopyDescriptionCommand { get; }
    }
}