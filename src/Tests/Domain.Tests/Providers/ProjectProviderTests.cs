using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;

namespace Domain.Tests.Providers;

public class ProjectProviderTests
{
    [Test]
    [TestCaseSource(nameof(OpenProjectTestSource))]
    public async Task OpenProjectTest(string path, Project expected)
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
    public async Task TryOpenProjectTest(string path, Project expected)
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
            "./Projects/1.json", new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
                StackAddress = 512,
                ProgramAddress = 512,
            }
        },
        new object[]
        {
            "./Projects/2.json", new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm", $"{Constants.ProjectDir}another.asm" },
                StackAddress = 1000,
                ProgramAddress = 512,
            }
        },
        new object[]
        {
            "./Projects/3.json", new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
                Devices = new List<string>
                {
                    $"{Constants.CurrentDir}device.dll"
                },
                StackAddress = 255,
                ProgramAddress = 10
            }
        }
    };
}