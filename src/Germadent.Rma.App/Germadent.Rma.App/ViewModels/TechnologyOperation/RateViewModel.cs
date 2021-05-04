using System;
using Germadent.Rma.Model.Production;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.TechnologyOperation
{
    public class RateViewModel : ViewModelBase
    {
        private RateDto _rateDto;

        public RateViewModel(RateDto rateDto)
        {
            _rateDto = rateDto;
        }

        public int TechnologyOperationId
        {
            get { return _rateDto.TechnologyOperationId; }
        }

        public int QualifyingRank
        {
            get { return _rateDto.QualifyingRank; }
        }

        public decimal Rate
        {
            get { return _rateDto.Rate; }
        }

        public DateTime DateBeginning
        {
            get { return _rateDto.DateBeginning; }
        }

        public RateDto ToModel() => _rateDto;

        public void Update(RateDto rateDto)
        {
            _rateDto = rateDto;
            OnPropertyChanged();
        }
    }
}