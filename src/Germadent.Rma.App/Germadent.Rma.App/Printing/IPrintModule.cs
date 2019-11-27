using Germadent.Rma.Model;

namespace Germadent.Rma.App.Printing
{
    public interface IPrintModule
    {
        void Print(Order order);
    }
}