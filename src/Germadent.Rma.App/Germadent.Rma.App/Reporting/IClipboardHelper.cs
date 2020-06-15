namespace Germadent.Rma.App.Reporting
{
    public interface IClipboardHelper
    {
        void CopyToClipboard(string text);
    }


    public class ClipboardHelper : IClipboardHelper
    {
        public void CopyToClipboard(string text)
        {
           System.Windows.Clipboard.SetText(text);
        }
    }
}
