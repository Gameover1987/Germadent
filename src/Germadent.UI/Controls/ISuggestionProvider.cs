using System;
using System.Collections;
using System.Linq;

namespace Germadent.UI.Controls
{
    public class SuggestionsEventArgs
    {
        public SuggestionsEventArgs(IEnumerable suggestions)
        {
            Suggestions = suggestions;
        }

        public IEnumerable Suggestions { get; }

        public bool IsEmpty
        {
            get { return Suggestions == null || !Suggestions.Cast<object>().Any(); }
        }
    }

    public interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }
}
