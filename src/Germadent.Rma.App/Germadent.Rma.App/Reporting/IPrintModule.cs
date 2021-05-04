using Germadent.Model;

namespace Germadent.Rma.App.Reporting
{
    public interface IPrintModule
    {
        void Print(OrderDto order);
    }
}