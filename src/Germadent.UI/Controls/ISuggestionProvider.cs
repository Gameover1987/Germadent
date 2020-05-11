using System;
using System.Collections;
using System.Linq;

namespace Germadent.UI.Controls
{
    public interface ISuggestionProvider
    {
        IEnumerable GetSuggestions(string filter);
    }
}
