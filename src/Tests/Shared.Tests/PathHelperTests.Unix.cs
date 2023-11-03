using System;
using Shared.Helpers;

namespace Shared.Tests;

/// <summary>
/// Tests for <see cref="PathHelper"/> on <see cref="Platforms.Unix"/> platform
/// </summary>
[Platform(Platforms.Unix)]
public class PathHelperUnixTests
{
    [Test]
    [TestCaseSource(nameof(CombineTestUnixSource))]
    public void CombineTest(string path1, string path2, string expected)
    {
        Assert.That(PathHelper.Combine(path1, path2), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetDirectoryNameTestUnixSource))]
    public void GetDirectoryNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetDirectoryName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetFileNameTestUnixSource))]
    public void GetFileNameTest(string path, string expected)
    {
        Assert.That(PathHelper.GetFileName(path), Is.EqualTo(expected));
    }

    [Test]
    [TestCaseSource(nameof(GetRelativePathTestUnixSource))]
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

    private static readonly object[] CombineTestUnixSource =
    {
        new[] { "/a", "b/c.txt", "/a/b/c.txt" },
        new[] { "/a/..", "b/c.txt", "/b/c.txt" },
        new[] { "/a/.", "b/c.txt", "/a/b/c.txt" },
        new[] { "/a/./../a", "./b/c.txt", "/a/b/c.txt" },
        new[] { "/a", "/b", "/b" }
    };

    private static readonly object[] GetDirectoryNameTestUnixSource =
    {
        new[] { "/a/b/c.txt", "/a/b" },
        new[] { "/a/../b/c.txt", "/b" },
        new[] { "/a/./b/c.txt", "/a/b" },
        new[] { "/a/./../a/./b/c.txt", "/a/b" },
        new[] { "/", null }
    };

    private static readonly object[] GetFileNameTestUnixSource =
    {
        new[] { "/a/b/c.txt", "c.txt" },
        new[] { "/a/../b/c.txt", "c.txt" },
        new[] { "/a/./b/c.txt", "c.txt" },
        new[] { "/a/./../a/./b/c.txt", "c.txt" },
        new[] { "/", "" },
        new[] { "c.txt", "c.txt" },
        new[] { "./c.txt", "c.txt" },
        new[] { "../c.txt", "c.txt" },
        new string[] { null, null }
    };

    private static readonly object[] GetRelativePathTestUnixSource =
    {
        new[] { "/a", "/c.txt", "../c.txt" },
        new[] { "/a/..", "/c.txt", "c.txt" },
        new[] { "/a/.", "/c.txt", "../c.txt" }
    };

    private static readonly object[] GetRelativePathInvalidThrowsTestSource =
    {
        new string[] { null, null },
        new[] { "  ", "" }
    };

    private static readonly object[] GetPathTypeTestSource =
    {
        new object[] { null, PathHelper.PathType.UnExisting },
        new object[] { "  ", PathHelper.PathType.UnExisting },
        new object[] { "./Jsons/1.json", PathHelper.PathType.File },
        new object[] { "./Jsons", PathHelper.PathType.Directory }
    };
}