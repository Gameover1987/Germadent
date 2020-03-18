using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Germadent.Rma.App.Reporting
{
    public interface IClipboardWorks
    {
        void CopyToClipboard(string data);
    }

    public class ClipboardWorks : IClipboardWorks
    {
        public void CopyToClipboard(string data)
        {
            Clipboard.SetText(data);
        }
    }
}
