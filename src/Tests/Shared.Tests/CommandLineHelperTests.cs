using System;
using CommandLine;
using Shared.Helpers;

namespace Shared.Tests;

internal class CommandLineHelperTests
{
    internal class Options
    {
        [Option('f', "family", Required = false, Default = "Arial")]
        public string FontFamily { get; set; }

        [Option('s', "size", Required = false, Default = 24)]
        public int FontSize { get; set; }

        [Option('i', "init", Required = true)] public int InitialNumber { get; set; }

        public Options()
        {
        }

        public Options(string font, int size, int init)
        {
            FontFamily = font;
            FontSize = size;
            InitialNumber = init;
        }
    }

    [Test]
    [TestCaseSource(nameof(ParseCommandLineTestSource))]
    public void ParseCommandLineTest(string[] args, Options expected, string errorTextSubstring)
    {
        var options = CommandLineHelper.ParseCommandLine<Options>(args, out var errorText);

        if (expected == default(Options))
        {
            Assert.Multiple(() =>
            {
                Assert.That(options, Is.EqualTo(default(Options)));
                Assert.That(errorText, Does.Contain(errorTextSubstring));
            });
        }
        else
        {
            Assert.Multiple(() =>
            {
                Assert.That(options.InitialNumber, Is.EqualTo(expected.InitialNumber));
                Assert.That(options.FontFamily, Is.EqualTo(expected.FontFamily));
                Assert.That(options.FontSize, Is.EqualTo(expected.FontSize));
                Assert.That(errorText, Is.Null);
            });
        }
    }

    private static readonly object[] ParseCommandLineTestSource =
    {
        new object[] { Array.Empty<string>(), default(Options), "Required option 'i, init' is missing" },
        new object[] { new[] { "-i", "2" }, new Options("Arial", 24, 2), null },
        new object[] { new[] { "--init", "23", "--family", "Font" }, new Options("Font", 24, 23), null },
        new object[] { new[] { "--init", "23", "-s", "9" }, new Options("Arial", 9, 23), null }
    };
}