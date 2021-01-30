using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Germadent.UI.Helpers
{
    /// <summary>
    /// Для того чтобы связывать имена с колонками грида
    /// </summary>
    public static class DataGridColumnNameHelper
    {
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name", typeof(string), typeof(DataGridColumnNameHelper),
                new UIPropertyMetadata(""));

        public static string GetName(DependencyObject obj)
        {
            return (string)obj.GetValue(NameProperty);
        }

        public static void SetName(DependencyObject obj, string value)
        {
            obj.SetValue(NameProperty, value);
        }

        public static string GetColumnName(this DataGridColumn column)
        {
            return GetName(column);
        }
    }
}
