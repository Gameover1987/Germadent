using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Corcon.ViewModel
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OnPropertyChanged(Expression<Func<object>> expression)
        {
            var handler = PropertyChanged;
            if (handler == null)
                return;

            if (expression.NodeType != ExpressionType.Lambda)
            {
                throw new ArgumentException("Value must be a lamda expression", "expression");
            }

            var body = expression.Body as MemberExpression;

            if (body == null)
            {
                throw new ArgumentException("‘x’ should be a member expression");
            }

            var propertyName = body.Member.Name;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetProperty<T>(ref T field, T value, [CallerMemberName] string caller = "")
        {
            if (!Equals(field, value))
            {
                field = value;
                OnPropertyChanged(caller);
                return true;
            }

            return false;
        }
    }
}
