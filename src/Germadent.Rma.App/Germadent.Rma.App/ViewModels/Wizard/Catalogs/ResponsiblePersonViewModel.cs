using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonViewModel : IResponsiblePersonViewModel
    {
        private readonly ResponsiblePersonDto _responsiblePersonDto;

        public ResponsiblePersonViewModel(ResponsiblePersonDto responsiblePersonDto)
        {
            _responsiblePersonDto = responsiblePersonDto;
        }

        public string FullName => _responsiblePersonDto.FullName;

        public string Position => _responsiblePersonDto.Position;

        public string Phone => _responsiblePersonDto.Phone;

        public string Email => _responsiblePersonDto.Email;

        public string Description => _responsiblePersonDto.Description;

        public ResponsiblePersonDto ToDto()
        {
            return _responsiblePersonDto;
        }
    }
}