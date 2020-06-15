using System;
using Germadent.Rma.Model;
using Germadent.UI.ViewModels;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonViewModel : ViewModelBase, IResponsiblePersonViewModel
    {
        private ResponsiblePersonDto _responsiblePersonDto;

        public ResponsiblePersonViewModel(ResponsiblePersonDto responsiblePersonDto)
        {
            _responsiblePersonDto = responsiblePersonDto;
        }

        public int ResponsiblePersonId => _responsiblePersonDto.Id;

        public string FullName => _responsiblePersonDto.FullName;

        public string Position => _responsiblePersonDto.Position;

        public string Phone => _responsiblePersonDto.Phone;

        public string Email => _responsiblePersonDto.Email;

        public string Description => _responsiblePersonDto.Description;

        public ResponsiblePersonDto ToDto()
        {
            return _responsiblePersonDto;
        }

        public void Update(ResponsiblePersonDto responsiblePersonDto)
        {
            _responsiblePersonDto = responsiblePersonDto;
            OnPropertyChanged();
        }
    }
}