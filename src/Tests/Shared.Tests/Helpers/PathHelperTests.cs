using System;
using System.IO;
using Shared.Helpers;

namespace Shared.Tests.Helpers;

/// <summary>
/// Tests for <see cref="PathHelper"/>
/// </summary>
public class PathHelperTests
{
    private static string Root => OperatingSystem.IsWindows() ? "C:" : "";

    private static char Separator => Path.DirectorySeparatorChar;

    [Test]
    [TestCaseSource(nameof(CombineTestSource))]
    public void CombineTest(string path1, string path2, string expected)
    {
        Assert.That(PathHelper.Combine(path1, path2), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetDirectoryNameTestSource))]
    public void GetDirectoryNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetDirectoryName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetFileNameTestSource))]
    public void GetFileNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetFileName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetRelativePathTestSource))]
    public void GetRelativePathTest(string relativeTo, string path, string expected)
    {
        Assert.That(PathHelper.GetRelativePath(relativeTo, path), Is.EqualTo(expected));
    }

    [Test]
    [Platform(Platforms.Win)]
    public void GetRelativeMultiDiskPathTest()
    {
        Assert.That(PathHelper.GetRelativePath("C:\\", "D:\\folder"), Is.EqualTo("D:\\folder"));
    }

    [Test]
    [TestCaseSource(nameof(GetPathTypeTestSource))]
    public void GetPathTypeTest(string path, PathHelper.PathType expected)
    {
        Assert.That(PathHelper.GetPathType(path), Is.EqualTo(expected));
    }

    private static readonly object[] CombineTestSource =
    {
        new[] { $"{Root}{Separator}a", $"b{Separator}c.txt", $"{Root}{Separator}a{Separator}b{Separator}c.txt" },
        new[] { $"{Root}{Separator}a{Separator}..", $"b{Separator}c.txt", $"{Root}{Separator}b{Separator}c.txt" },
        new[]
        {
            $"{Root}{Separator}a{Separator}.", $"b{Separator}c.txt", $"{Root}{Separator}a{Separator}b{Separator}c.txt"
        },
        new[]
        {
            $"{Root}{Separator}a{Separator}.{Separator}..{Separator}a", $".{Separator}b{Separator}c.txt",
            $"{Root}{Separator}a{Separator}b{Separator}c.txt"
        },
        new[] { $"{Root}{Separator}a", $"{Root}{Separator}b", $"{Root}{Separator}b" }
    };

    private static readonly object[] GetDirectoryNameTestSource =
    {
        new[] { $"{Root}{Separator}a{Separator}b{Separator}c.txt", $"{Root}{Separator}a{Separator}b" },
        new[] { $"{Root}{Separator}a{Separator}..{Separator}b{Separator}c.txt", $"{Root}{Separator}b" },
        new[] { $"{Root}{Separator}a{Separator}.{Separator}b{Separator}c.txt", $"{Root}{Separator}a{Separator}b" },
        new[]
        {
            $"{Root}{Separator}a{Separator}.{Separator}..{Separator}a{Separator}.{Separator}b{Separator}c.txt",
            $"{Root}{Separator}a{Separator}b"
        },
        new[] { $"{Root}{Separator}", null }
    };

    private static readonly object[] GetFileNameTestSource =
    {
        new[] { $"{Root}{Separator}a{Separator}b{Separator}c.txt", "c.txt" },
        new[] { $"{Root}{Separator}a{Separator}..{Separator}b{Separator}c.txt", "c.txt" },
        new[] { $"{Root}{Separator}a{Separator}.{Separator}b{Separator}c.txt", "c.txt" },
        new[]
        {
            $"{Root}{Separator}a{Separator}.{Separator}..{Separator}a{Separator}.{Separator}b{Separator}c.txt", "c.txt"
        },
        new[] { $"{Root}{Separator}", "" },
        new[] { "c.txt", "c.txt" },
        new[] { $".{Separator}c.txt", "c.txt" },
        new[] { $"..{Separator}c.txt", "c.txt" },
        new string[] { null, null }
    };

    private static readonly object[] GetRelativePathTestSource =
    {
        new[] { $"{Root}{Separator}a", $"{Root}{Separator}c.txt", $"..{Separator}c.txt" },
        new[] { $"{Root}{Separator}a{Separator}..", $"{Root}{Separator}c.txt", "c.txt" },
        new[] { $"{Root}{Separator}a{Separator}.", $"{Root}{Separator}c.txt", $"..{Separator}c.txt" },
        new[] { null, null, string.Empty },
        new[] { " ", "", string.Empty },
        new[] { $"{Root}{Separator}", " ", string.Empty },
        new[] { " ", $"{Root}{Separator}", $"{Root}{Separator}" }
    };

    private static readonly object[] GetPathTypeTestSource =
    {
        new object[] { null, PathHelper.PathType.UnExisting },
        new object[] { "  ", PathHelper.PathType.UnExisting },
        new object[] { $".{Separator}Jsons{Separator}1.json", PathHelper.PathType.File },
        new object[] { $".{Separator}Jsons", PathHelper.PathType.Directory }
    };
}