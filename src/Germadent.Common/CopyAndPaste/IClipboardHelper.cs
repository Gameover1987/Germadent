using System.Windows;

namespace Germadent.Common.CopyAndPaste
{
    public interface IClipboardHelper
    {
        void CopyToClipboard(string text);
    }


    public class ClipboardHelper : IClipboardHelper
    {
        public void CopyToClipboard(string text)
        {
            Clipboard.SetText(text);
        }
    }
}
