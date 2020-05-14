using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class CustomerRepositoryChangedEventArgs : EventArgs
    {
        public CustomerRepositoryChangedEventArgs(CustomerDto[] addedItems, CustomerDto[] deletedItems)
        {
            AddedItems = addedItems;
            DeletedItems = deletedItems;
        }

        public CustomerDto[] AddedItems { get; }

        public CustomerDto[] DeletedItems { get; }
    }
}