﻿using System.Windows;
using System.Windows.Data;

namespace Germadent.UI.Controls
{
    public class BindingEvaluator : FrameworkElement
    {

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(string), typeof(BindingEvaluator), new FrameworkPropertyMetadata(string.Empty));

        private Binding _valueBinding;

        public BindingEvaluator(Binding binding)
        {
            ValueBinding = binding;
        }

        public string Value
        {
            get { return (string)GetValue(ValueProperty); }

            set { SetValue(ValueProperty, value); }
        }

        public Binding ValueBinding
        {
            get { return _valueBinding; }
            set { _valueBinding = value; }
        }

        public string Evaluate(object dataItem)
        {
            DataContext = dataItem;
            SetBinding(ValueProperty, ValueBinding);
            return Value;
        }
    }
}