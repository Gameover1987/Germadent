using System;
using System.Collections;
using Germadent.UI.Infrastructure;

namespace Germadent.UI.Controls
{
    public abstract class SuggestionsProviderBase : ISuggestionProvider
    {
        private readonly IDispatcher _dispatcher;

        public SuggestionsProviderBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public IEnumerable GetSuggestions(string filter)
        {
            var suggestions = GetSuggestionsImpl(filter);

            _dispatcher.BeginInvoke(() =>
            {
                Loaded?.Invoke(this, new SuggestionsEventArgs(suggestions));
            });

            return suggestions;
        }

        protected abstract IEnumerable GetSuggestionsImpl(string filter);

        public event EventHandler<SuggestionsEventArgs> Loaded;
    }
}