using System;

namespace Germadent.Rma.App.ServiceClient.Repository
{
    public interface IRepository
    {
        /// <summary>
        /// Инициализация
        /// </summary>
        void Initialize();

        /// <summary>
        /// Событие наструпающее при изменении коллекции
        /// </summary>
        event EventHandler<EventArgs> Changed;
    }

    public interface IRepository<T> : IRepository
    {
        /// <summary>
        /// Коллекция всех элементов
        /// </summary>
        T[] Items { get; }
    }

    public abstract class Repository<T> : IRepository<T>
    {
        private T[] _items;

        public T[] Items => _items;

        public void Initialize()
        {
            _items = GetItems();
        }

        protected void ReLoad()
        {
            _items = GetItems();
            Changed?.Invoke(this, EventArgs.Empty);
        }

        protected abstract T[] GetItems();

        public event EventHandler<EventArgs> Changed;
    }
}
