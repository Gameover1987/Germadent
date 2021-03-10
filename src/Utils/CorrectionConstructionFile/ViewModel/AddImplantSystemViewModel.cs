using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Germadent.Common.Extensions;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels;
using Germadent.UI.ViewModels.Validation;
using NLog.LayoutRenderers.Wrappers;

namespace Germadent.CorrectionConstructionFile.App.ViewModel
{
    public enum ViewMode
    {
        Add, Edit
    }

    public interface IAddImplantSystemViewModel
    {
        void Initialize(ImplantSystemViewModel[] existingItems, ImplantSystemViewModel implantSystem);

        ImplantSystemViewModel GetItem();
    }

    public class AddImplantSystemViewModel : ValidationSupportableViewModel, IAddImplantSystemViewModel
    {
        private string _name;

        private List<ImplantSystemViewModel> _existedItems;

        public AddImplantSystemViewModel()
        {
            AddValidationFor(() => Name)
                .When(
                    () => _existedItems.Select(x => x.Name.ToLower()).Contains(Name.ToLower()),
                    () => "Такой ключ уже есть в словаре")
                .When(() => Name.IsNullOrWhiteSpace(), () => "Введите значение ключа");

            OkCommand = new DelegateCommand(CanOK);
        }

        public string Title { get; set; }

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value)
                    return;
                _name = value;
                OnPropertyChanged(() => Name);
            }
        }

        public ObservableCollection<CorrectionDictionaryItem> Values { get; set; } = new ObservableCollection<CorrectionDictionaryItem>();

        public ICommand OkCommand { get; }

        private bool CanOK()
        {
            if (Name.IsNullOrEmpty())
                return false;

            if (Values.Count == 0)
                return false;
            
            if (Values.Where(x => !x.IsEmpty).Any(x => x.Value.IsNullOrWhiteSpace()))
                return false;

            if (HasErrors)
                return false;

            if (_existedItems.Select(x => x.Name.ToLower()).Contains(Name.ToLower()))
                return false;

            return true;
        }

        public void Initialize(ImplantSystemViewModel[] existedItems, ImplantSystemViewModel implantSystem)
        {
            _existedItems = existedItems.ToList();
            _name = string.Empty;

            Values.Clear();

            if (implantSystem == null)
            {
                Title = "Добавление системы имплантов";
            }
            else
            {
                Title = "Редактирование системы имплантов";
                _existedItems.Remove(implantSystem);
                _name = implantSystem.Name;
                foreach (var item in implantSystem.ToModel().CorrectionDictionary)
                {
                    Values.Add(item);
                }
            }
        }

        public ImplantSystemViewModel GetItem()
        {
            return new ImplantSystemViewModel(
                new ImplantSystem
                {
                    Name = Name,
                    CorrectionDictionary = Values.Where(x => !x.IsEmpty).ToArray()
                });
        }
    }
}
