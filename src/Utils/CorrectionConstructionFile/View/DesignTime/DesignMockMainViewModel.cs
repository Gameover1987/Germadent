using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Common.FileSystem;
using Germadent.CorrectionConstructionFile.App.Model;
using Germadent.CorrectionConstructionFile.App.ViewModel;
using Germadent.UI.Infrastructure;
using Germadent.UI.ViewModels.DesignTime;

namespace Germadent.CorrectionConstructionFile.App.View.DesignTime
{
    public class DesignMockXmlDocumentProcessor : IXmlDocumentProcessor
    {
        public void Process(string sourceFileName, string destFileName, CorrectionDictionaryItem[] correctionDictionary)
        {
            throw new NotImplementedException();
        }

        public string ProcessInfo { get; }
    }

    public class DesignMockMainViewModel : MainViewModel
    {
        public DesignMockMainViewModel()
            : base(new DesignMockShowDialogAgent(), new FileManager(), new DesignMockXmlDocumentProcessor(), new DesignMockAddDictionaryItemViewModel())
        {
        }
    }

    public class DesignMockAddDictionaryItemViewModel : AddDictionaryItemViewModel
    {
        public DesignMockAddDictionaryItemViewModel()
        {
            Initialize(new CorrectionDictionaryItem[]
            {
                new CorrectionDictionaryItem{Name = "Preved", Value = "Medved"},
            }, null);
        }
    }
}
