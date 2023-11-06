using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Domain.Models;
using Domain.Providers;
using Domain.Validators;

namespace Domain.Tests;

public class ProjectValidatorTests
{
    [Test]
    [TestCaseSource(nameof(ThrowIfFileInvalidTestSource))]
    public async Task ThrowIfFileInvalidTest(string path, bool invalid)
    {
        // Arrange

        var validator = new ProjectValidator(new ProjectProvider());

        // Act & Assert

        if (invalid)
        {
            Assert.CatchAsync<ValidationException>(async () => await validator.ThrowIfFileInvalidAsync(path));
        }
        else
        {
            await validator.ThrowIfFileInvalidAsync(path);
        }
    }

    [Test]
    [TestCaseSource(nameof(ThrowIfModelInvalidTestSource))]
    public void ThrowIfModelInvalidTest(Project project, bool invalid, string errorSubstring)
    {
        // Arrange

        var validator = new ProjectValidator(new ProjectProvider());

        // Act & Assert

        if (invalid)
        {
            var error = Assert.Catch<ValidationException>(() => validator.ThrowIfModelInvalid(project));
            Assert.That(error!.Message, Does.Contain(errorSubstring));
        }
        else
        {
            validator.ThrowIfModelInvalid(project);
        }
    }

    private static readonly object[] ThrowIfFileInvalidTestSource =
    {
        new object[] { "./Projects/1.json", false },
        new object[] { "./Projects/2.json", false },
        new object[] { "./Projects/3.json", false },
        new object[] { "./Projects/invalid.json", true },
    };

    private static readonly object[] ThrowIfModelInvalidTestSource =
    {
        new object[]
        {
            new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
                ProjectFile = $"{Constants.ProjectDir}1.json"
            },
            false,
            null
        },
        new object[]
        {
            new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" }
            },
            true,
            "Project file path is not set"
        },
        new object[]
        {
            new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
                ProjectFile = $"{Constants.ProjectDir}"
            },
            true,
            "Project file does not exist"
        },
        new object[]
        {
            new Project
            {
                Executable = $"{Constants.ProjectDir}another.asm",
                Files = new List<string> { $"{Constants.ProjectDir}another.asm" },
                ProjectFile = $"{Constants.ProjectDir}1.json"
            },
            true,
            "These files do not exist on disk"
        },
        new object[]
        {
            new Project
            {
                Files = new List<string> { $"{Constants.ProjectDir}../Projects" },
                ProjectFile = $"{Constants.ProjectDir}1.json"
            },
            true,
            "These files do not exist on disk"
        },
        new object[]
        {
            new Project
            {
                Executable = "  ",
                Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
                ProjectFile = $"{Constants.ProjectDir}1.json"
            },
            true,
            "Executable file is not set"
        },
        new object[]
        {
            new Project
            {
                Executable = $"{Constants.ProjectDir}main.asm",
                ProjectFile = $"{Constants.ProjectDir}1.json"
            },
            true,
            "Executable file is not represented in project files"
        }
    };
}