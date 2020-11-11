using System;
using Germadent.Rma.Model.Pricing;

namespace Germadent.Rma.App.ServiceClient
{
    public class RepositoryChangedEventArgs<T> : EventArgs
    {
        public RepositoryChangedEventArgs(T[] addedItems, T[] changedItems, int[] deletedItems)
        {
            AddedItems = addedItems ?? new T[0];
            ChangedItems = changedItems ?? new T[0];
            DeletedItems = deletedItems ?? new int[0];
        }
        
        public T[] AddedItems { get; }

        public T[] ChangedItems { get; }

        public int[] DeletedItems { get; }
    }

    public interface ISignalRClient : IDisposable
    {
        void Initialize();

        event EventHandler<RepositoryChangedEventArgs<PriceGroupDto>> PriceGroupRepositoryChanged;
    }
}