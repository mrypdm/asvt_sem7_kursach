using System;
using Shared.Converters;

namespace Shared.Tests.Converters;

/// <summary>
/// Tests for <see cref="NumberStringConverter"/>
/// </summary>
public class NumberStringConverterTests
{
    [Test]
    [TestCaseSource(nameof(ConvertTestSource))]
    public void ConvertTest(string number, int expected)
    {
        var converter = new NumberStringConverter();
        Assert.That(converter.Convert(number), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(ConvertInvalidTestSource))]
    public void ConvertInvalidTest(string number)
    {
        var converter = new NumberStringConverter();
        Assert.Catch<FormatException>(() => converter.Convert(number));
    }

    private static readonly object[] ConvertTestSource =
    {
        new object[] { "12", 12 },
        new object[] { "0d12", 12 },
        new object[] { "0b1100", 12 },
        new object[] { "1100", 1100 },
        new object[] { "0x1f", 31 },
        new object[] { "0o10", 8 }
    };

    private static readonly object[] ConvertInvalidTestSource =
    {
        new[] { "1f" },
        new[] { "x1f" },
        new[] { "1,5" },
        new[] { "b1010" }
    };
}