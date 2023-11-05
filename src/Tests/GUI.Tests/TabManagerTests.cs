using System;
using GUI.Exceptions;
using GUI.Managers;
using GUI.Models;
using GUI.ViewModels;

namespace GUI.Tests;

public class TabManagerTests
{
    [Test]
    public void CreateTabTest()
    {
        // Arrange

        var file = CreateFile();
        var manager = new TabManager();

        // Act

        var tab = manager.CreateTab(file, null, null);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(tab.File, Is.EqualTo(file));
            Assert.That(manager.Tabs, Does.Contain(tab));
            Assert.That(manager.Tabs, Has.Count.EqualTo(1));
            Assert.That(manager.Tab, Is.Null);
        });
    }

    [Test]
    public void DeleteTabTest()
    {
        // Arrange

        var manager = new TabManager();
        var tab = manager.CreateTab(CreateFile(), null, null);

        // Act

        manager.DeleteTab(tab);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(manager.Tabs, Does.Not.Contains(tab));
            Assert.That(manager.Tabs, Has.Count.EqualTo(0));
            Assert.That(manager.Tab, Is.Null);
        });
    }

    [Test]
    public void SelectTabTest()
    {
        // Arrange

        var manager = new TabManager();
        var tab = manager.CreateTab(CreateFile(), null, null);

        // Act

        manager.SelectTab(tab);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(manager.Tab, Is.EqualTo(tab));
            Assert.That(tab.TabBackground, Is.EqualTo(FileTabViewModel.SelectedBackground));
        });
    }

    [Test]
    public void SelectAnotherTab()
    {
        // Arrange

        var hasCalled = false;

        var manager = new TabManager();
        var tab1 = manager.CreateTab(CreateFile(), null, null);
        var tab2 = manager.CreateTab(CreateFile(), null, null);

        manager.SelectTab(tab1);

        // Act

        manager.SelectTab(tab2);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(manager.Tab, Is.EqualTo(tab2));
            Assert.That(tab1.TabBackground, Is.EqualTo(FileTabViewModel.DefaultBackground));
            Assert.That(tab2.TabBackground, Is.EqualTo(FileTabViewModel.SelectedBackground));
        });
    }

    [Test]
    public void SelectFirstWhenDelete()
    {
        // Arrange

        var manager = new TabManager();
        var tab1 = manager.CreateTab(CreateFile(), null, null);
        var tab2 = manager.CreateTab(CreateFile(), null, null);
        var tab3 = manager.CreateTab(CreateFile(), null, null);

        manager.SelectTab(tab2);

        // Act

        manager.DeleteTab(tab2);

        // Assert

        Assert.That(manager.Tab, Is.EqualTo(tab1));
    }

    [Test]
    public void AddDuplicateThrow()
    {
        // Arrange

        var file = CreateFile();
        var manager = new TabManager();
        manager.CreateTab(file, null, null);

        // Act & Assert

        Assert.Throws<TabExistsException>(() =>
            manager.CreateTab(file, null, null));
    }

    private static FileModel CreateFile() => new()
    {
        FilePath = $"C:\\{Guid.NewGuid()}",
        Text = "",
        IsNeedSave = false
    };
}