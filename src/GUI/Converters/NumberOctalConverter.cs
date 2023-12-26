using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace GUI.Converters;

public class NumberOctalConverter : IValueConverter
{
    /// <summary>
    /// Converts double to string
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ushort word)
        {
            return System.Convert.ToString(word, 8).PadLeft(6, '0');
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    /// <summary>
    /// Converts string to double
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string octalString)
        {
            return System.Convert.ToUInt16(octalString, 8);
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}