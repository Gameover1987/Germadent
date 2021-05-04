using System;
using DocumentFormat.OpenXml;
using Germadent.Rma.Model.Production;
using Germadent.UI.Commands;
using Germadent.UI.ViewModels.Validation;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class AddRateViewModel : ValidationSupportableViewModel, IAddRateViewModel
    {
        private int _technologyOperationId;

        private decimal _rate;
        private int _qualifyingRank;

        private DateTime _dateBeginning;
        private ViewMode _viewMode;

        public AddRateViewModel()
        {
            AddValidationFor(() => Rate)
                .When(() => Rate <= 0, () => "Укажите расценку отличную от нуля");

            OkCommand = new DelegateCommand(CanOK);
        }

        public string Title
        {
            get
            {
                if (_viewMode == ViewMode.Add)
                    return "Добавление расценки";

                return "Редактирование расценки";
            }
        }

        public decimal Rate
        {
            get { return _rate; }
            set
            {
                if (_rate == value)
                    return;
                _rate = value;
                OnPropertyChanged(() => Rate);
            }
        }

        public int QualifyingRank
        {
            get { return _qualifyingRank; }
            set
            {
                if (_qualifyingRank == value)
                    return;
                _qualifyingRank = value;
                OnPropertyChanged(() => QualifyingRank);
            }
        }

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

        public IDelegateCommand OkCommand { get; }

        public void Initialize(ViewMode viewMode, RateDto rateDto)
        {
            _viewMode = viewMode;
            _technologyOperationId = rateDto.TechnologyOperationId;
            _rate = rateDto.Rate;
            _qualifyingRank = rateDto.QualifyingRank;
            _dateBeginning = rateDto.DateBeginning;
        }

        public RateDto GetRate()
        {
            return new RateDto
            {
                TechnologyOperationId = _technologyOperationId,
                Rate = Rate,
                QualifyingRank = QualifyingRank,
                DateBeginning = DateBeginning,
            };
        }

        private bool CanOK()
        {
            return Rate > 0;
        }
    }
}