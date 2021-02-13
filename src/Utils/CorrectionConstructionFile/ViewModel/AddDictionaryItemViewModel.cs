using System;
using System.Collections.Generic;
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

    public interface IAddDictionaryItemViewModel
    {
        void Initialize(CorrectionDictionaryItem[] items, CorrectionDictionaryItem item);

        CorrectionDictionaryItem GetItem();
    }



    public class AddDictionaryItemViewModel : ValidationSupportableViewModel, IAddDictionaryItemViewModel
    {
        private string _name;
        private string _value;

        private List<CorrectionDictionaryItem> _existedItems;

        public AddDictionaryItemViewModel()
        {
            AddValidationFor(() => Name)
                .When(
                    () => _existedItems.Select(x => x.Name.ToLower()).Contains(Name.ToLower()),
                    () => "Такой ключ уже есть в словаре")
                .When(() => Name.IsNullOrWhiteSpace(), () => "Введите значение ключа");

            AddValidationFor(() => Value)
                .When(() => Value.IsNullOrWhiteSpace(), () => "Введите значение");

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

        public string Value
        {
            get { return _value; }
            set
            {
                if (_value == value)
                    return;
                _value = value;
                OnPropertyChanged(() => Value);
            }
        }

        public ICommand OkCommand { get; }

        private bool CanOK()
        {
            return !Name.IsNullOrEmpty() &&
                   !Value.IsNullOrEmpty() &&
                   !HasErrors &&
                   !_existedItems.Select(x => x.Name.ToLower()).Contains(Name.ToLower());
        }

        public void Initialize(CorrectionDictionaryItem[] existedItems, CorrectionDictionaryItem item)
        {
            _existedItems = existedItems.ToList();
            _name = string.Empty;
            _value = string.Empty;

            if (item == null)
            {
                Title = "Добавление элемента словаря";
            }
            else
            {
                Title = "Редактирование элемента словаря";
                _existedItems.Remove(item);
                _name = item.Name;
                _value = item.Value;
            }
        }

        public CorrectionDictionaryItem GetItem()
        {
            return new CorrectionDictionaryItem {Name = Name, Value = Value};
        }
    }
}
