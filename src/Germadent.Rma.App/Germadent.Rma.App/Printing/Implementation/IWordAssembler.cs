namespace Germadent.Rma.App.Printing.Implementation
{
    public interface IWordAssembler
    {
        byte[] Assembly(byte[] templateDoc, string jsonString);
    }
}

