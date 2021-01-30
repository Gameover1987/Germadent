using System;
using System.Windows.Input;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.Rma.Model.Pricing;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.Pricing
{
    public interface IAddPriceViewModel
    {
        void Initialize(PriceDto price);

        PriceDto GetPrice();
    }

    public class AddPriceViewModel : ValidationSupportableViewModel, IAddPriceViewModel
    {
        private DateTime _dateBeginning;
        private decimal _priceStl;
        private decimal _priceModel;

        public AddPriceViewModel()
        {
            AddValidationFor(() => PriceModel)
                .When(() => PriceModel <= 0, () => "Укажите цену отличную от нуля");

            OkCommand = new DelegateCommand(CanOK);
        }

        public string Title
        {
            get { return GetTitle(ViewMode); }
        }

        public CardViewMode ViewMode { get; set; }

        public DateTime DateBeginning
        {
            get { return _dateBeginning; }
            set
            {
                if (_dateBeginning == value)
                    return;
                _dateBeginning = value;
                OnPropertyChanged(() => DateBeginning);
            }
        }

        public decimal PriceStl
        {
            get { return _priceStl; }
            set
            {
                if (_priceStl == value)
                    return;
                _priceStl = value;
                OnPropertyChanged(() => PriceStl);
            }
        }

        public decimal PriceModel
        {
            get { return _priceModel; }
            set
            {
                if (_priceModel == value)
                    return;
                _priceModel = value;
                OnPropertyChanged(() => PriceModel);
            }
        }

        public ICommand OkCommand { get; }

        public void Initialize(PriceDto price)
        {
            ViewMode = price.PriceId == 0 ? CardViewMode.Add : CardViewMode.Edit;

            _dateBeginning = price.DateBeginning;
            _priceStl = price.PriceStl;
            _priceModel = price.PriceModel;
        }

        public PriceDto GetPrice()
        {
            return new PriceDto
            {
                DateBeginning = DateBeginning,
                PriceModel = PriceModel,
                PriceStl = PriceStl
            };
        }

        private string GetTitle(CardViewMode cardViewMode)
        {
            switch (cardViewMode)
            {
                case CardViewMode.Add:
                    return "Добавление цены";

                case CardViewMode.Edit:
                    return "Редактирование цены";

                default:
                    throw new NotImplementedException("Неизвестный режим представления");
            }
        }

        private bool CanOK()
        {
            return PriceModel > 0;
        }
    }
}
