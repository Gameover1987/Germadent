using System;
using System.Collections;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockSuggestionProvider : ISuggestionProvider
    {
        public IEnumerable GetSuggestions(string filter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<SuggestionsEventArgs> Loaded;
    }
}