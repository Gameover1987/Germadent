using System;
using System.Collections;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Controls;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class ResponsiblePersonSuggestionProvider : SuggestionsProviderBase
    {
        private readonly IRmaServiceClient _rmaOperations;

        public ResponsiblePersonSuggestionProvider(IRmaServiceClient rmaOperations, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _rmaOperations = rmaOperations;
        }

        protected override IEnumerable GetSuggestionsImpl(string filter)
        {
            throw new NotImplementedException();
        }
    }
}