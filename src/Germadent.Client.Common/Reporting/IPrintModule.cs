using Germadent.Model;

namespace Germadent.Client.Common.Reporting
{
    public interface IPrintModule
    {
        void PrintOrder(OrderDto order);

        void PrintSalaryReport(SalaryReport salaryReport);
    }
}