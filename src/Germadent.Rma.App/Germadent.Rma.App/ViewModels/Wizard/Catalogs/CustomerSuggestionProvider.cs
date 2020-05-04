using System;
using System.Collections;
using System.Linq;
using Germadent.Rma.App.ServiceClient;
using Germadent.UI.Controls;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ViewModels.Wizard.Catalogs
{
    public class CustomerSuggestionProvider : SuggestionsProviderBase
    {
        private readonly IRmaOperations _rmaOperations;
        private readonly IDispatcher _dispatcher;

        public CustomerSuggestionProvider(IRmaOperations rmaOperations, IDispatcher dispatcher)
            : base(dispatcher)
        {
            _rmaOperations = rmaOperations;
            _dispatcher = dispatcher;
        }

        protected override IEnumerable GetSuggestionsImpl(string filter)
        {
            return _rmaOperations.GetCustomers(filter).Select(x => new CustomerViewModel(x)).ToArray();
        }
    }
}
