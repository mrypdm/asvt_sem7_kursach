using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Shared.Helpers;

namespace Shared.Tests.Helpers;

public class JsonHelperTests
{
    [Test]
    [TestCaseSource(nameof(ValidateJsonSource))]
    public async Task ValidateJson<TType>(string json, bool isValid, TType type)
    {
        var res = await JsonHelper.ValidateJsonAsync<TType>(json);
        Assert.That(res, isValid ? Is.Null : Is.Not.Null);
    }

    [Test]
    [TestCaseSource(nameof(DeserializeProjectFileTestSource))]
    public async Task DeserializeProjectFileTest(string path, ProjectDto expected)
    {
        var project = await JsonHelper.DeserializeFileAsync<ProjectDto>(path);

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
    [TestCaseSource(nameof(SerializeToFileTestSource))]
    public async Task SerializeToFileTest(object obj, string pathToExpected)
    {
        // Arrange

        const string testFile = $"./Jsons/{nameof(SerializeToFileTest)}.json";

        // Act

        await JsonHelper.SerializeToFileAsync(obj, testFile);

        // Assert

        var actual = await File.ReadAllTextAsync(testFile);
        actual = new string(actual.Where(c => !char.IsWhiteSpace(c)).ToArray());
        
        var expected = await File.ReadAllTextAsync(pathToExpected);
        expected = new string(expected.Where(c => !char.IsWhiteSpace(c)).ToArray());

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static readonly object[] ValidateJsonSource =
    {
        new object[]
        {
            "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"}",
            true,
            new ProjectDto()
        },
        new object[]
        {
            "{\n  \"Files\": [\n    \"main.asm\"\n  ],\n  \"Executable\": \"main.asm\",\n  \"Devices\": [],\n  \"StackAddress\": \"0o1000\",\n  \"ProgramAddress\": \"0o1000\"\n}",
            true,
            new ProjectDto()
        },
        new object[]
        {
            "{\"Files\":[],\"Executable\": \"\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"}",
            true,
            new ProjectDto()
        },
        new object[]
        {
            "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"",
            false,
            new ProjectDto()
        },
        new object[]
        {
            "{\"Files\":[]\"Executable\": \"\",\"Devices\":[],\"StackAddress\":\"0o1000\",\"ProgramAddress\":\"0o1000\"}",
            false,
            new ProjectDto()
        }
    };

    private static readonly object[] DeserializeProjectFileTestSource =
    {
        new object[]
        {
            "./Jsons/1.json",
            new ProjectDto
            {
                Files = new List<string> { "main.asm" },
                Executable = "main.asm",
                StackAddress = "0o1000",
                ProgramAddress = "0o1000"
            }
        },
        new object[]
        {
            "./Jsons/2.json",
            new ProjectDto
            {
                Files = new List<string>(),
                Executable = string.Empty,
                StackAddress = "0o1000",
                ProgramAddress = "0o1000"
            }
        },
        new object[]
        {
            "./Jsons/3.json",
            new ProjectDto
            {
                Files = new List<string>(),
                Executable = string.Empty,
                StackAddress = "0b1010",
                ProgramAddress = "0xFF"
            }
        },
        new object[]
        {
            "./Jsons/4.json",
            new ProjectDto
            {
                Files = new List<string>(),
                Executable = string.Empty,
                Devices = new List<string> { "C:\\a.dll" },
                StackAddress = "0b1010",
                ProgramAddress = "0xFF"
            }
        }
    };

    private static readonly object[] SerializeToFileTestSource =
        DeserializeProjectFileTestSource
            .Select(obj => new[] { ((object[])obj)[1], ((object[])obj)[0] })
            .Union(
                new object[]
                {
                    new object[]
                    {
                        new Dictionary<string, object> { { "Options", new { FontFamily = "Font", FontSize = 24 } } },
                        "./Jsons/5.json"
                    }
                })
            .ToArray();
}