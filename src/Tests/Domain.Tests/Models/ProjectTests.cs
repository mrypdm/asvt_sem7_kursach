using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Domain.Extensions;
using Domain.Models;
using Shared.Helpers;

namespace Domain.Tests.Models;

public class ProjectTests
{
    [Test]
    public async Task ToJsonTest()
    {
        // Arrange

        var project = InitProject();

        // Act

        await project.ToJsonAsync();

        // Arrange

        var lines = await File.ReadAllLinesAsync(project.ProjectFile);

        Assert.That(lines, Has.Exactly(2).Matches<string>(s => s.Contains("Projects/main.asm")));
    }

    [Test]
    public async Task FromJsonTest()
    {
        // Arrange

        var project = InitProject();
        await project.ToJsonAsync();

        // Act

        var dto = await JsonHelper.DeserializeFileAsync<ProjectDto>(project.ProjectFile);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(dto.Files, Has.Exactly(1).Matches<string>(s => s == "Projects/main.asm"));
            Assert.That(dto.Executable, Is.EqualTo("Projects/main.asm"));
        });
    }

    private Project InitProject() => new()
    {
        ProjectFile = $"{Constants.CurrentDir}{nameof(ToJsonTest)}.pdp11proj",
        Files = new List<string> { $"{Constants.ProjectDir}main.asm" },
        Executable = $"{Constants.ProjectDir}main.asm"
    };
}