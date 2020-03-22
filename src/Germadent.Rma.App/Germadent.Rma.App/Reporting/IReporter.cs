using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.Reporting
{
    public interface IReporter
    {
        void CreateReport(ReportListDto[] reports);
    }

    public class ClipboardReporter : IReporter
    {
        private readonly IClipboardWorks _clipboard;

        public ClipboardReporter(IClipboardWorks clipboard)
        {
            _clipboard = clipboard;
        }

        public void CreateReport(ReportListDto[] reports)
        {
            throw new NotImplementedException();
        }
    }
}
