using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace GUI.Converters;

public class StringOctalConverter : IValueConverter
{
    /// <summary>
    /// Converts octal to string
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int number and >= 0)
        {
            return System.Convert.ToString(number, 8);
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    /// <summary>
    /// Converts string to octal
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string octalString)
        {
            try
            {
                return System.Convert.ToInt32(octalString, 8);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}