using System;
using System.Collections.Generic;
using System.Text;

namespace Germadent.UI.ViewModels.DesignTime
{
    public class DesignMockInputBoxViewModel : InputBoxViewModel
    {
        public DesignMockInputBoxViewModel()
        {
            Title = "Заголовок";
            ParameterName = "Параметр";
            InputString = "Строка с данными";
        }
    }
}
