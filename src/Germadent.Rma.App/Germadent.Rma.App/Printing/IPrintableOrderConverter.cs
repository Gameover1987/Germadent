using Germadent.Rma.Model;

namespace Germadent.Rma.App.Printing
{
    public interface IPrintableOrderConverter
    {
        PrintableOrder ConvertFrom(Order order);
    }

    public class PrintableOrderConverter : IPrintableOrderConverter
    {
        public PrintableOrder ConvertFrom(Order order)
        {
            return new PrintableOrder();
        }
    }
}