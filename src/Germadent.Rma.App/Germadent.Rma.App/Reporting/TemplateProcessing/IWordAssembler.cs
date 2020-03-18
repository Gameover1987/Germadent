namespace Germadent.Rma.App.Reporting.TemplateProcessing
{
    public interface IWordAssembler
    {
        byte[] Assembly(byte[] templateDoc, string jsonString);
    }
}

