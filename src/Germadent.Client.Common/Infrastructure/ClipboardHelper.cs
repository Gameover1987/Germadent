namespace Germadent.Client.Common.Infrastructure
{
    public class ClipboardHelper : IClipboardHelper
    {
        public void CopyToClipboard(string text)
        {
            System.Windows.Clipboard.SetText(text);
        }
    }
}