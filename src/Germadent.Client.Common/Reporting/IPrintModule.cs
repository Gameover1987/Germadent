using Germadent.Model;

namespace Germadent.Client.Common.Reporting
{
    public interface IPrintModule
    {
        void Print(OrderDto order);
    }
}