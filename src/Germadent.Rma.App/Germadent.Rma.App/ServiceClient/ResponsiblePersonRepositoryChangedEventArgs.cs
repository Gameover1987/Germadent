using System;
using Germadent.Rma.Model;

namespace Germadent.Rma.App.ServiceClient
{
    public class ResponsiblePersonRepositoryChangedEventArgs : EventArgs
    {
        public ResponsiblePersonRepositoryChangedEventArgs(ResponsiblePersonDto[] addedItems, ResponsiblePersonDto[] deletedItems)
        {
            AddedItems = addedItems;
            DeletedItems = deletedItems;
        }

        public ResponsiblePersonDto[] AddedItems { get; }

        public ResponsiblePersonDto[] DeletedItems { get; }
    }
}