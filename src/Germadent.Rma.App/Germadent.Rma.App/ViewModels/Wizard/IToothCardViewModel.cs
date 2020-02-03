using Germadent.Rma.Model;
using System.Collections.ObjectModel;

namespace Germadent.Rma.App.ViewModels.Wizard
{
    public interface IToothCardViewModel
    {
        /// <summary>
        /// Зубы
        /// </summary>
        ObservableCollection<ToothViewModel> Teeth { get; }

        /// <summary>
        /// Материалы
        /// </summary>
        ObservableCollection<MaterialViewModel> Materials { get; }

        /// <summary>
        /// Типы протезирования
        /// </summary>
        ObservableCollection<ProstheticsTypeViewModel> Prosthetics { get; }
        
        void Initialize(ToothDto[] toothCard);

        ToothDto[] ToModel();
    }
}