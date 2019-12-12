using Germadent.Rma.Model;

namespace Germadent.Rma.App.Printing
{
    public interface IPrintableOrderConverter
    {
        PrintableOrder ConvertFrom(OrderDto order);
    }

    public class PrintableOrderConverter : IPrintableOrderConverter
    {
        public PrintableOrder ConvertFrom(OrderDto order)
        {
            return new PrintableOrder();
        }
    }
}