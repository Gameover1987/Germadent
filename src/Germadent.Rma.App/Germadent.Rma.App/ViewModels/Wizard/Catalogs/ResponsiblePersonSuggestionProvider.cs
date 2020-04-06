using System.Collections;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonSuggestionProvider : ISuggestionProvider
    {
        private readonly IRmaOperations _rmaOperations;

        public ResponsiblePersonSuggestionProvider(IRmaOperations rmaOperations)
        {
            _rmaOperations = rmaOperations;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            return null;
        }
    }
}