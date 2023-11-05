using System.Threading.Tasks;
using GUI.Managers;
using GUI.Models;
using Shared.Helpers;

namespace GUI.Tests;

public class SettingsManagerTests
{
    [Test]
    public void CreationTest()
    {
        // Arrange

        var editorOptions = new EditorOptions();
        var commandLineOptions = new CommandLineOptions();

        // Act

        SettingsManager.Create(editorOptions, commandLineOptions);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(SettingsManager.Instance.FontSize, Is.EqualTo(editorOptions.FontSize));
            Assert.That(SettingsManager.Instance.FontFamily.Name, Is.EqualTo(editorOptions.FontFamily));
            Assert.That(SettingsManager.Instance.CommandLineOptions, Is.EqualTo(commandLineOptions));
        });
    }

    [Test]
    public void SingletonTest()
    {
        // Arrange

        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
        var instance1 = SettingsManager.Instance;

        // Act

        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());
        var instance2 = SettingsManager.Instance;

        // Assert

        Assert.That(instance2, Is.EqualTo(instance1));
    }

    [Test]
    public async Task SaveTest()
    {
        // Arrange

        SettingsManager.Create(new EditorOptions(), new CommandLineOptions());

        // Act

        await SettingsManager.Instance.SaveGlobalSettingsAsync();

        // Assert

        var options = await JsonHelper.DeserializeFileAsync<EditorOptions>("appsettings.json");

        Assert.Multiple(() =>
        {
            Assert.That(options.FontSize, Is.EqualTo(SettingsManager.Instance.FontSize));
            Assert.That(options.FontFamily, Is.EqualTo(SettingsManager.Instance.FontFamily.Name));
        });
    }
}