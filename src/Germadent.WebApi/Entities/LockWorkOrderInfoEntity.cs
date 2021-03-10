using System;

namespace Germadent.WebApi.Entities
{
    public class LockWorkOrderInfoEntity
    {
        public int WorkOrderId { get; set; }

        public int UserId { get; set; }
        public DateTime OccupancyDateTime { get; set; }
    }
}