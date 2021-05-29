using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Germadent.UI.Converters
{
    /// <summary>
    /// Конвертация int в Color или Brush и обратно
    /// </summary>
    public class ColorConverter : IValueConverter
    {
        /// <summary>
        /// Color To int
        /// </summary>
        public static int ToInt(Color color)
        {
            var iCol = (color.A << 24) | (color.R << 16) | (color.G << 8) | color.B;
            return iCol;
        }

        /// <summary>
        /// integer from Color
        /// </summary>
        /// <param name="iCol"></param>
        /// <returns></returns>
        public static Color ToColor(int iCol)
        {
            var color = Color.FromArgb((byte)(iCol >> 24),
                (byte)(iCol >> 16),
                (byte)(iCol >> 8),
                (byte)iCol);
            //ставим полную непрозрачность
            //color.A = 255;
            return color;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int)
            {
                if (targetType == typeof(Color))
                    return ToColor((int)value);
                if (targetType == typeof(Brush))
                    return new SolidColorBrush(ToColor((int)value));
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Color && targetType == typeof(int))
                return ToInt((Color)value);
            if (value is SolidColorBrush && targetType == typeof(int))
                return ToInt(((SolidColorBrush)value).Color);
            return null;
        }
    }

	/// <summary>
	/// Получить цвет, который будет видно на фоне переданного
	/// Поддерживается цвет int, Color, SolidColorBrush
	/// </summary>
	public class ReadableColorConverter : IValueConverter
    {
        /// <summary>
        /// Получить цвет, который будет видно на фоне backColor
        /// </summary>
        public static Color GetReadableForeColor(Color backColor)
        {
            return (backColor.R + backColor.B + backColor.G) / 3 > 128 ?
                Colors.Black : Colors.White;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Color color;
            if (value is int)
                color = ColorConverter.ToColor((int)value);
            else if (value is Color)
                color = (Color)value;
            else if (value is SolidColorBrush)
                color = ((SolidColorBrush)value).Color;
            else
                return value;

            if (color == Colors.Transparent)
                return Binding.DoNothing;

            color = GetReadableForeColor(color);

            if (targetType == typeof(Color))
                return color;
            if (targetType == typeof(Brush))
                return new SolidColorBrush(color);

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
