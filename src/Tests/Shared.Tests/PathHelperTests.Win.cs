using System;
using Shared.Helpers;

namespace Shared.Tests;

/// <summary>
/// Tests for <see cref="PathHelper"/> on <see cref="Platforms.Win"/> platform
/// </summary>
[Platform(Platforms.Win)]
public class PathHelperWinTests
{
    [Test]
    [TestCaseSource(nameof(CombineTestWinSource))]
    public void CombineTest(string path1, string path2, string expected)
    {
        Assert.That(PathHelper.Combine(path1, path2), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetDirectoryNameTestWinSource))]
    public void GetDirectoryNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetDirectoryName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetFileNameTestWinSource))]
    public void GetFileNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetFileName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetRelativePathTestWinSource))]
    public void GetRelativePathTest(string relativeTo, string path, string expected)
    {
        Assert.That(PathHelper.GetRelativePath(relativeTo, path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetRelativePathInvalidThrowsTestSource))]
    public void GetRelativePathInvalidThrowsTest(string relativeTo, string path)
    {
        Assert.Catch<ArgumentException>(() => PathHelper.GetRelativePath(relativeTo, path));
    }

    [Test]
    [TestCaseSource(nameof(GetPathTypeTestSource))]
    public void GetPathTypeTest(string path, PathHelper.PathType expected)
    {
        Assert.That(PathHelper.GetPathType(path), Is.EqualTo(expected));
    }

    private static readonly object[] CombineTestWinSource =
    {
        new[] { "C:\\a", "b\\c.txt", "C:\\a\\b\\c.txt" },
        new[] { "C:\\a\\..", "b\\c.txt", "C:\\b\\c.txt" },
        new[] { "C:\\a\\.", "b\\c.txt", "C:\\a\\b\\c.txt" },
        new[] { "C:\\a\\.\\..\\a", ".\\b\\c.txt", "C:\\a\\b\\c.txt" },
        new[] { "C:\\a", "C:\\b", "C:\\b" }
    };

    private static readonly object[] GetDirectoryNameTestWinSource =
    {
        new[] { "C:\\a\\b\\c.txt", "C:\\a\\b" },
        new[] { "C:\\a\\..\\b\\c.txt", "C:\\b" },
        new[] { "C:\\a\\.\\b\\c.txt", "C:\\a\\b" },
        new[] { "C:\\a\\.\\..\\a\\.\\b\\c.txt", "C:\\a\\b" },
        new[] { "C:\\", null }
    };

    private static readonly object[] GetFileNameTestWinSource =
    {
        new[] { "C:\\a\\b\\c.txt", "c.txt" },
        new[] { "C:\\a\\..\\b\\c.txt", "c.txt" },
        new[] { "C:\\a\\.\\b\\c.txt", "c.txt" },
        new[] { "C:\\a\\.\\..\\a\\.\\b\\c.txt", "c.txt" },
        new[] { "C:\\", "" },
        new[] { "c.txt", "c.txt" },
        new[] { "./c.txt", "c.txt" },
        new[] { "../c.txt", "c.txt" },
        new string[] { null, null }
    };

    private static readonly object[] GetRelativePathTestWinSource =
    {
        new[] { "C:\\a", "C:\\c.txt", "..\\c.txt" },
        new[] { "C:\\a\\..", "C:\\c.txt", "c.txt" },
        new[] { "C:\\a\\.", "C:\\c.txt", "..\\c.txt" },
        new[] { "C:\\", "D:\\folder", "D:\\folder" }
    };

    private static readonly object[] GetRelativePathInvalidThrowsTestSource =
    {
        new string[] { null, null },
        new[] { "  ", "" },
        new[] { "C:\\", " " },
        new[] { " ", "C:\\c.txt" }
    };

    private static readonly object[] GetPathTypeTestSource =
    {
        new object[] { null, PathHelper.PathType.UnExisting },
        new object[] { "  ", PathHelper.PathType.UnExisting },
        new object[] { "./Jsons/1.json", PathHelper.PathType.File },
        new object[] { "./Jsons", PathHelper.PathType.Directory }
    };
}