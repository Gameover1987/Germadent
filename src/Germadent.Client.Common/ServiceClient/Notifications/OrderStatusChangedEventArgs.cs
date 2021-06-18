using System;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient.Notifications
{
    public class OrderStatusChangedEventArgs : EventArgs
    {
        public OrderStatusChangedEventArgs(OrderStatusNotificationDto orderStatusNotification)
        {
            WorkOrderId = orderStatusNotification.WorkOrderId;
            UserId = orderStatusNotification.UserId;
            Status = orderStatusNotification.Status;
            StatusChanged = orderStatusNotification.StatusChangeDateTime;
        }

        public int WorkOrderId { get; set; }

        public int UserId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime StatusChanged { get; set; }
    }
}