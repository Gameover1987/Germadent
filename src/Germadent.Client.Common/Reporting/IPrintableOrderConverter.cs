using Germadent.Model;

namespace Germadent.Client.Common.Reporting
{
    public interface IPrintableOrderConverter
    {
        PrintableOrder ConvertFrom(OrderDto order);
    }
}