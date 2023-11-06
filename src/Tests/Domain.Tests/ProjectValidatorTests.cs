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
    public void ThrowIfModelInvalidTest(ProjectModel projectModel, bool invalid, string errorSubstring)
    {
        // Arrange

        var validator = new ProjectValidator(new ProjectProvider());

        // Act & Assert

        if (invalid)
        {
            var error = Assert.Catch<ValidationException>(() => validator.ThrowIfModelInvalid(projectModel));
            Assert.That(error!.Message, Does.Contain(errorSubstring));
        }
        else
        {
            validator.ThrowIfModelInvalid(projectModel);
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
            new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                ProjectFilePath = "./Projects/1.json"
            },
            false,
            null
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" }
            },
            true,
            "Project file path is not set"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                ProjectFilePath = "./Projects"
            },
            true,
            "Project file does not exist"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "another.asm",
                Files = new List<string> { "another.asm" },
                ProjectFilePath = "./Projects/1.json"
            },
            true,
            "These files do not exist on disk"
        },
        new object[]
        {
            new ProjectModel
            {
                Files = new List<string> { "../Projects" },
                ProjectFilePath = "./Projects/1.json"
            },
            true,
            "These files do not exist on disk"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "  ",
                Files = new List<string> { "main.asm" },
                ProjectFilePath = "./Projects/1.json"
            },
            true,
            "Executable file is not set"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "main.asm",
                ProjectFilePath = "./Projects/1.json"
            },
            true,
            "Executable file is not represented in project files"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                ProjectFilePath = "./Projects/1.json",
                ProgramAddressString = ""
            },
            true,
            "Program start address is not set"
        },
        new object[]
        {
            new ProjectModel
            {
                Executable = "main.asm",
                Files = new List<string> { "main.asm" },
                ProjectFilePath = "./Projects/1.json",
                StackAddressString = ""
            },
            true,
            "Stack start address is not set"
        },
    };
}