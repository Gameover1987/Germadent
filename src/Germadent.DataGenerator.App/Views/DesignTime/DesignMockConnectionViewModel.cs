using System;
using System.Collections.Generic;
using System.Text;
using Germadent.DataGenerator.App.ViewModels;

namespace Germadent.DataGenerator.App.Views.DesignTime
{
    public class DesignMockConnectionViewModel : ConnectionViewModel
    {
        public DesignMockConnectionViewModel()
        {
            Address = "89.189.186.48";
            User = "sa";
            Password = "asza";
            DbName = "Germadent_Dev";
            CreateNewDb = true;
        }
    }
}
