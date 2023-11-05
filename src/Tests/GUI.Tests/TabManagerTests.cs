using System;
using System.Threading.Tasks;
using GUI.Exceptions;
using GUI.Managers;
using GUI.Models;

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

        var tab = manager.CreateTab(file, _ => Task.CompletedTask, _ => Task.CompletedTask);

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
        var tab = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);

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
        var tab = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);

        // Act

        manager.SelectTab(tab);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(manager.Tab, Is.EqualTo(tab));
            Assert.That(tab.IsSelected, Is.True);
        });
    }

    [Test]
    public void SelectAnotherTab()
    {
        // Arrange

        var manager = new TabManager();
        var tab1 = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);
        var tab2 = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);

        manager.SelectTab(tab1);

        // Act

        manager.SelectTab(tab2);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(manager.Tab, Is.EqualTo(tab2));
            Assert.That(tab1.IsSelected, Is.False);
            Assert.That(tab2.IsSelected, Is.True);
        });
    }

    [Test]
    public void SelectFirstWhenDelete()
    {
        // Arrange

        var manager = new TabManager();
        manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);
        var tab2 = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);
        var tab3 = manager.CreateTab(CreateFile(), _ => Task.CompletedTask, _ => Task.CompletedTask);

        manager.SelectTab(tab3);

        // Act

        manager.DeleteTab(tab3);

        // Assert

        Assert.That(manager.Tab, Is.EqualTo(tab2));
    }

    [Test]
    public void AddDuplicateThrow()
    {
        // Arrange

        var file = CreateFile();
        var manager = new TabManager();
        manager.CreateTab(file, _ => Task.CompletedTask, _ => Task.CompletedTask);

        // Act & Assert

        Assert.Throws<TabExistsException>(() =>
            manager.CreateTab(file, _ => Task.CompletedTask, _ => Task.CompletedTask));
    }

    private static FileModel CreateFile() => new()
    {
        FilePath = $"C:\\{Guid.NewGuid()}",
        Text = "",
        IsNeedSave = false
    };
}