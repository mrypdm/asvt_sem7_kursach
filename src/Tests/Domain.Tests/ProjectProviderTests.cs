using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;

namespace Domain.Tests;

public class ProjectProviderTests
{
    [Test]
    [TestCaseSource(nameof(OpenProjectTestSource))]
    public async Task OpenProjectTest(string path, ProjectModel expected)
    {
        // Arrange

        var provider = new ProjectProvider();

        // Act

        var project = await provider.OpenProjectAsync(path);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(project.Executable, Is.EqualTo(expected.Executable));
            Assert.That(project.Files, Is.EqualTo(expected.Files));
            Assert.That(project.Devices, Is.EqualTo(expected.Devices));
            Assert.That(project.ProgramAddress, Is.EqualTo(expected.ProgramAddress));
            Assert.That(project.StackAddress, Is.EqualTo(expected.StackAddress));
        });
    }

    [Test]
    [TestCaseSource(nameof(OpenProjectTestSource))]
    public async Task TryOpenProjectTest(string path, ProjectModel expected)
    {
        // Arrange

        var provider = new ProjectProvider();

        // Act

        var (isSuccess, project) = await provider.TryOpenProjectAsync(path);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.True);
            Assert.That(project.Executable, Is.EqualTo(expected.Executable));
            Assert.That(project.Files, Is.EqualTo(expected.Files));
            Assert.That(project.Devices, Is.EqualTo(expected.Devices));
            Assert.That(project.ProgramAddress, Is.EqualTo(expected.ProgramAddress));
            Assert.That(project.StackAddress, Is.EqualTo(expected.StackAddress));
        });
    }

    [Test]
    public void OpenInvalidProjectTest()
    {
        // Arrange

        var provider = new ProjectProvider();

        // Act & Assert

        Assert.CatchAsync<JsonException>(async () => await provider.OpenProjectAsync("./Projects/invalid.json"));
    }

    [Test]
    public async Task TryOpenInvalidProjectTest()
    {
        // Arrange

        var provider = new ProjectProvider();

        // Act

        var (isSuccess, project) = await provider.TryOpenProjectAsync("./Projects/invalid.json");

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(isSuccess, Is.False);
            Assert.That(project, Is.Null);
        });
    }

    private static readonly object[] OpenProjectTestSource =
    {
        new object[]
        {
            "./Projects/1.json", new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                StackAddressString = "0o1000",
                ProgramAddressString = "0o1000",
            }
        },
        new object[]
        {
            "./Projects/2.json", new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm", "another.asm" },
                StackAddressString = "1000",
                ProgramAddressString = "512",
            }
        },
        new object[]
        {
            "./Projects/3.json", new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                Devices = new List<string> { "C:\\device.dll" },
                StackAddressString = "0xff",
                ProgramAddressString = "0b1010",
            }
        }
    };
}