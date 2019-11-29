using Germadent.Common.Extensions;

namespace Germadent.Rma.Model
{
    public static class FilterExtensions
    {
        public static bool MatchByFilter(this Order order, OrdersFilter filter)
        {
            if (!filter.Laboratory && order is LaboratoryOrder)
                return false;

            if (!filter.MillingCenter && order is MillingCenterOrder)
                return false;

            if (!filter.Customer.IsNullOrWhiteSpace() && filter.Customer.ToLower() != order.Customer.ToLower())
                return false;

            if (!filter.Patient.IsNullOrWhiteSpace() && filter.Patient.ToLower() != order.Patient.ToLower())
                return false;

            return true;
        }

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