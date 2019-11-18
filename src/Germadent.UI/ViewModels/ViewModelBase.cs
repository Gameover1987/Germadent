using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using Expression = System.Linq.Expressions.Expression;

namespace Germadent.UI.ViewModels
{
    /// <summary>
    ///     Базовый класс для всех ViewModel'ей.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        private static readonly int _mainThreadId;

        static ViewModelBase()
        {
            _mainThreadId = Thread.CurrentThread.ManagedThreadId;
        }

        /// <summary>
        ///     Сообщает об изменении значения свойства.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;        

        /// <summary>
        ///     Обновляет свойство, вызывая <see cref="OnPropertyChanged()"/>.
        /// </summary>
        /// <typeparam name="TValue">Тип значения свойства.</typeparam>
        /// <param name="propertyExpression">Лямбда для получения имени свойства</param>
        protected void RefreshProperty<TValue>(Expression<Func<TValue>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);
            var owner = GetContainer(propertyExpression);
            var ownerType = owner.GetType();

            var value = default (TValue);

            var fieldInfo = GetFieldRecursively(ownerType, propertyName);
            if (fieldInfo != null)
                value = (TValue) fieldInfo.GetValue(owner);

            var propertyInfo = ownerType.GetProperty(propertyName);
            if (propertyInfo != null)
                value = (TValue) propertyInfo.GetValue(owner);

            SetProperty(propertyExpression, default(TValue));
            SetProperty(propertyExpression, value);
        }

        /// <summary>
        /// Задает значение свойству и сообщает о его изменении только если новое значение отличается.
        /// </summary>
        /// <typeparam name="TValue">Тип устанавливаемого значения</typeparam>
        /// <param name="propertyExpression">Лямбда для получения имени свойства</param>
        /// <param name="value">Устанавливаемое значение</param>
        /// <param name="callerMemberName">Имя свойства</param>
        /// <returns>Возвращает true если значение свойства было изменено</returns>
        protected bool SetProperty<TValue>(Expression<Func<TValue>> propertyExpression, TValue value, bool throwNullException = true, [CallerMemberName] string callerMemberName = null)
        {
            var func = propertyExpression.Compile();
            var funcValue = func();
            if (Equals(funcValue, value))
                return false;

            var propertyName = GetPropertyName(propertyExpression);
            var owner = GetContainer(propertyExpression);
            var ownerType = owner.GetType();
            var fieldInfo = GetFieldRecursively(ownerType, propertyName);
            if (fieldInfo != null)
            {
                fieldInfo.SetValue(owner, value);
                OnPropertyChanged(callerMemberName);
                return true;
            }

            var propertyInfo = ownerType.GetProperty(propertyName);
            if (propertyInfo != null)
            {
                propertyInfo.SetValue(owner, value);
                OnPropertyChanged(callerMemberName);
                return true;
            }

            throw new NotSupportedException(propertyExpression.ToString());
        }

        private FieldInfo GetFieldRecursively(Type type, string fieldName)
        {
            if (type == typeof(object))
                return null;

            var fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            if (fieldInfo == null)
                return GetFieldRecursively(type.BaseType, fieldName);

            return fieldInfo;
        }

        private object GetContainer<TValue>(Expression<Func<TValue>> propertyLambdaExpression)
        {
            return Evaluate((propertyLambdaExpression.Body as MemberExpression).Expression);
        }

        private object Evaluate(Expression e)
        {
            switch (e.NodeType)
            {
                case ExpressionType.Constant:
                    return (e as ConstantExpression).Value;
                case ExpressionType.MemberAccess:
                    {
                        var propertyExpression = e as MemberExpression;
                        var field = propertyExpression.Member as FieldInfo;
                        var property = propertyExpression.Member as PropertyInfo;
                        var container = propertyExpression.Expression == null ? null : Evaluate(propertyExpression.Expression);
                        if (field != null)
                            return field.GetValue(container);
                        else if (property != null)
                            return property.GetValue(container, null);
                        else
                            return null;
                    }
                default:
                    return null;
            }
        }

        /// <summary>
        ///     Сообщает об изменении значений всех свойств.
        /// </summary>
        protected void OnPropertyChanged()
        {
            RaisePropertyChangedEvent(string.Empty);
        }

        /// <summary>
        ///     Сообщает об изменении значения свойства.
        /// </summary>
        /// <param name="propertyName">Имя свойства.</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged == null)
                return;

            OnPropertyChangedImpl(propertyName);
        }

        /// <summary>
        ///     Сообщает об изменении значения свойства.
        /// </summary>
        /// <param name="propertyExpression">Выражения для получения имени свойства.</param>
        protected void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var propertyName = GetPropertyName(propertyExpression);

            OnPropertyChanged(propertyName);
        }

        /// <summary>
        ///     Возвращает название свойства по выражению.
        /// </summary>
        /// <param name="propertyExpression">Выражение для получения имени свойства.</param>
        /// <returns></returns>
        protected string GetPropertyName(LambdaExpression propertyExpression)
        {
            if (propertyExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentNullException(nameof(propertyExpression));

            return memberExpression.Member.Name;
        }

        protected void ExecuteInUIThread(Action action)
        {
            Application.Current?.Dispatcher.Invoke(action);
        }

        protected T ExecuteInUIThread<T>(Func<T> func)
        {
            var result = default(T);

            Application.Current?.Dispatcher.Invoke(() => result = func());

            return result;
        }

        private void OnPropertyChangedImpl(string propertyName)
        {
            if (Thread.CurrentThread.ManagedThreadId == _mainThreadId)
                RaisePropertyChangedEvent(propertyName);
            else
                ExecuteInUIThread(() => RaisePropertyChangedEvent(propertyName));
        }

        private void RaisePropertyChangedEvent(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler == null)
            {
                return;
            }

            var e = new PropertyChangedEventArgs(propertyName);
            handler(this, e);
        }

        public void RefreshProperty(string propertyName)
        {
            OnPropertyChanged(propertyName);
        }

        public void Refresh()
        {
            OnPropertyChanged();
        }
    }
}