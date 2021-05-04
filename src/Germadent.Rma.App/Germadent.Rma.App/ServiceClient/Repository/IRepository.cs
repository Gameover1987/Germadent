using System;
using System.Windows;
using Germadent.UI.Infrastructure;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();
    }

    public interface IRepository<T> : IRepository
    {
        /// <summary>
        /// Коллекция всех элементов
        /// </summary>
        T[] Items { get; }

        /// <summary>
        /// Событие наструпающее при изменении коллекции
        /// </summary>
        event EventHandler<RepositoryChangedEventArgs<T>> Changed;
    }

    public abstract class Repository<T> : IRepository<T>
    {
        private T[] _items;

        public T[] Items => _items;

        public void Initialize()
        {
            _items = GetItems();
        }

        protected abstract T[] GetItems();

        public event EventHandler<RepositoryChangedEventArgs<T>> Changed;

        protected void OnRepositoryChanged(object sender, RepositoryChangedEventArgs<T> e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Changed?.Invoke(sender, e);
            });
        }
    }
}
