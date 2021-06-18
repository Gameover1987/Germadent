using System;
using Germadent.Model;

namespace Germadent.Client.Common.ServiceClient.Notifications
{
    public class OrderLockedEventArgs : EventArgs
    {
        public OrderLockedEventArgs(OrderLockInfoDto lockInfo)
        {
            Info = lockInfo;
        }

        public OrderLockInfoDto Info { get; }
    }
}