using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonViewModel : IResponsiblePersonViewModel
    {
        public ResponsiblePersonViewModel(ResponsiblePersonDto responsiblePersonDto)
        {
            
        }

        public string DisplayName { get; set; }
    }
}