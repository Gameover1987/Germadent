namespace Germadent.Client.Common.Reporting.TemplateProcessing
{
    public interface IWordAssembler
    {
        byte[] Assembly(byte[] templateDoc, string jsonString);
    }
}

