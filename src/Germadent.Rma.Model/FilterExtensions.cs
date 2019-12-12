using Germadent.Common.Extensions;

namespace Germadent.Rma.Model
{
    public static class FilterExtensions
    {
        public static bool IsNullOrEmpty(this OrdersFilter filter)
        {
            if (filter == null)
                return true;

            return filter.Laboratory == OrdersFilter.Empty.Laboratory &&
                   filter.MillingCenter == OrdersFilter.Empty.MillingCenter &&
                   filter.PeriodBegin == OrdersFilter.Empty.PeriodBegin &&
                   filter.PeriodEnd == OrdersFilter.Empty.PeriodEnd &&
                   filter.Patient == OrdersFilter.Empty.Patient &&
                   filter.Employee == OrdersFilter.Empty.Employee &&
                   filter.Customer == OrdersFilter.Empty.Customer &&
                   filter.Materials == OrdersFilter.Empty.Materials;
        }
    }
}