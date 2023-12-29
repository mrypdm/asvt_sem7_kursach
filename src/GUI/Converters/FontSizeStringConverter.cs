using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace GUI.Converters;

public class FontSizeStringConverter : IValueConverter
{
    /// <summary>
    /// Converts font size to string
    /// </summary>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double number and > 0.0)
        {
            return number.ToString(CultureInfo.InvariantCulture);
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }

    /// <summary>
    /// Converts string to font size
    /// </summary>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string doubleString
            && double.TryParse(doubleString, NumberStyles.Any, CultureInfo.InvariantCulture, out var number)
            && number > 0)
        {
            return number;
        }

        return new BindingNotification(new InvalidCastException(), BindingErrorType.Error);
    }
}