using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace GUI.Converters;

public class StringDoubleConverter : IValueConverter
{
    /// <summary>
    /// Converts double to string
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double number)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    /// <summary>
    /// Converts string to double
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string doubleString &&
            double.TryParse(doubleString, NumberStyles.Any, CultureInfo.InvariantCulture, out var number))
        {
            return number;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}