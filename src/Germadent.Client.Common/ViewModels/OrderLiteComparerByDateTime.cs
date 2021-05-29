using System;
using System.Collections;

namespace Germadent.Client.Common.ViewModels
{
    public class OrderLiteComparerByDateTime : IComparer
    {
        public int Compare(object x, object y)
        {
            if (x == null || y == null)
                return 0;

            var order1 = (OrderLiteViewModel) x;
            var order2 = (OrderLiteViewModel)y;

            return DateTime.Compare(order2.Created, order1.Created);
        }
    }
}