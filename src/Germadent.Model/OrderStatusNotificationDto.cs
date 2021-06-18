using System;

namespace Germadent.Model
{
    public class OrderStatusNotificationDto
    {
        public int WorkOrderId { get; set; }

        public int UserId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime StatusChangeDateTime { get; set; }
    }
}