using System;
using System.Collections.Generic;
using System.Text;
using Germadent.Rma.App.ViewModels.ToothCard;

namespace Germadent.Rma.App.Views.DesignMock
{
    public class DesignMockToothViewModel : ToothViewModel
    {
        public DesignMockToothViewModel() 
            : base(new DesignMockDictionaryRepository(), new DesignMockProductRepository())
        {
            Number = 18;
        }
    }
}
