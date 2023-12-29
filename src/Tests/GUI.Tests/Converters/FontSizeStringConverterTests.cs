using System.Linq;
using Avalonia.Data;
using GUI.Converters;

namespace GUI.Tests.Converters;

public class FontSizeStringConverterTests
{
    [Test]
    [TestCaseSource(nameof(StringToDoubleTestSource))]
    public void StringToDoubleTest(string str, double expected)
    {
        // Arrange

        var converter = new FontSizeStringConverter();

        // Act & Assert

        Assert.That(converter.ConvertBack(str, null, null, null), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(DoubleToStringTestSource))]
    public void DoubleToStringTest(double num, string expected)
    {
        // Arrange

        var converter = new FontSizeStringConverter();

        // Act & Assert

        Assert.That(converter.Convert(num, null, null, null), Is.EqualTo(expected));
    }

    [Test]
    public void ConvertNegativeStringError()
    {
        // Arrange

        var converter = new FontSizeStringConverter();

        // Act & Assert

        Assert.That(converter.ConvertBack("-1", null, null, null), Is.TypeOf<BindingNotification>());
    }
    
    [Test]
    public void ConvertNegativeSizeError()
    {
        // Arrange

        var converter = new FontSizeStringConverter();

        // Act & Assert

        Assert.That(converter.Convert(-1.0, null, null, null), Is.TypeOf<BindingNotification>());
    }

    private static readonly object[] StringToDoubleTestSource =
    {
        new object[] { "1", 1.0 },
        new object[] { "1.5", 1.5 },
        new object[] { "2.33", 2.33 },
        new object[] { "123.123", 123.123 },
    };

    private static readonly object[] DoubleToStringTestSource =
        StringToDoubleTestSource
            .Select(obj => new[] { ((object[])obj)[1], ((object[])obj)[0] })
            .ToArray();
}