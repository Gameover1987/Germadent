using System;
using System.Collections;
using Germadent.Rma.App.ViewModels.Wizard.Catalogs;
using Germadent.UI.Controls;

namespace Germadent.Rma.App.Views.DesignMock
{
   public class DesignMockCustomerSuggestionProvider : ICustomerSuggestionProvider
   {
       public IEnumerable GetSuggestions(string filter)
       {
           throw new NotImplementedException();
       }
   }

   public class DesignMockResponsiblePersonsSuggestionProvider : IResponsiblePersonsSuggestionsProvider
   {
       public IEnumerable GetSuggestions(string filter)
       {
           throw new NotImplementedException();
       }
   }
}