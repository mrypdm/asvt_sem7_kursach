using System.Linq;
using GUI.Converters;

namespace GUI.Tests;

public class StringDoubleConverterTests
{
    [Test]
    [TestCaseSource(nameof(StringToDoubleTestSource))]
    public void StringToDoubleTest(string str, double expected)
    {
        // Arrange

        var converter = new StringDoubleConverter();

        // Act & Assert

        Assert.That(converter.ConvertBack(str, null, null, null), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(DoubleToStringTestSource))]
    public void DoubleToStringTest(double num, string expected)
    {
        // Arrange

        var converter = new StringDoubleConverter();

        // Act & Assert

        Assert.That(converter.Convert(num, null, null, null), Is.EqualTo(expected));
    }

    private static readonly object[] StringToDoubleTestSource =
    {
        new object[] { "1", 1.0 },
        new object[] { "-1", -1.0 },
        new object[] { "1.5", 1.5 },
        new object[] { "2.33", 2.33 },
        new object[] { "0", 0.0 },
        new object[] { "123.123", 123.123 },
        new object[] { "-123.123", -123.123 }
    };

    private static readonly object[] DoubleToStringTestSource =
        StringToDoubleTestSource
            .Select(obj => new[] { ((object[])obj)[1], ((object[])obj)[0] })
            .ToArray();
}