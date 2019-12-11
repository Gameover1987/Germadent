namespace Germadent.Rma.App.Printing.TemplateProcessing
{
    public interface IWordAssembler
    {
        byte[] Assembly(byte[] templateDoc, string jsonString);
    }
}

