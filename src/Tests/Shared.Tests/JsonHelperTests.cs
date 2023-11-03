using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Shared.Helpers;

namespace Shared.Tests;

public class JsonHelperTests
{
    [Test]
    [TestCaseSource(nameof(ValidateJsonAsyncSource))]
    public async Task ValidateJsonAsync<TType>(string json, bool isValid, TType type)
    {
        var res = await JsonHelper.ValidateJsonAsync<TType>(json);
        Assert.That(res, isValid ? Is.Null : Is.Not.Null);
    }

    [Test]
    [TestCaseSource(nameof(DeserializeProjectFileAsyncTestSource))]
    public async Task DeserializeProjectFileAsyncTest(string path, ProjectModel expected)
    {
        var project = await JsonHelper.DeserializeFileAsync<ProjectModel>(path);

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
    [TestCaseSource(nameof(SerializeToFileAsyncTestSource))]
    public async Task SerializeToFileAsyncTest(object obj, string pathToExpected)
    {
        // Arrange
        const string testFile = $"./Jsons/{nameof(SerializeToFileAsyncTest)}.json";

        // Act
        await JsonHelper.SerializeToFileAsync(obj, testFile);

        // Assert
        var actual = await File.ReadAllTextAsync(testFile);
        actual = new string(actual.Where(c => !char.IsWhiteSpace(c)).ToArray());
        
        var expected = await File.ReadAllTextAsync(pathToExpected);
        expected = new string(expected.Where(c => !char.IsWhiteSpace(c)).ToArray());

        Assert.That(actual, Is.EqualTo(expected));
    }

    private static readonly object[] ValidateJsonAsyncSource =
    {
        new object[]
        {
            "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddressString\":\"0o1000\",\"ProgramAddressString\":\"0o1000\"}",
            true,
            new ProjectModel()
        },
        new object[]
        {
            "{\n  \"Files\": [\n    \"main.asm\"\n  ],\n  \"Executable\": \"main.asm\",\n  \"Devices\": [],\n  \"StackAddressString\": \"0o1000\",\n  \"ProgramAddressString\": \"0o1000\"\n}",
            true,
            new ProjectModel()
        },
        new object[]
        {
            "{\"Files\":[],\"Executable\": \"\",\"Devices\":[],\"StackAddressString\":\"0o1000\",\"ProgramAddressString\":\"0o1000\"}",
            true,
            new ProjectModel()
        },
        new object[]
        {
            "{\"Files\":[\"main.asm\"],\"Executable\": \"main.asm\",\"Devices\":[],\"StackAddressString\":\"0o1000\",\"ProgramAddressString\":\"0o1000\"",
            false,
            new ProjectModel()
        },
        new object[]
        {
            "{\"Files\":[]\"Executable\": \"\",\"Devices\":[],\"StackAddressString\":\"0o1000\",\"ProgramAddressString\":\"0o1000\"}",
            false,
            new ProjectModel()
        },
    };

    private static readonly object[] DeserializeProjectFileAsyncTestSource =
    {
        new object[]
        {
            "./Jsons/1.json",
            new ProjectModel
            {
                Files = new List<string> { "main.asm" },
                Executable = "main.asm",
                StackAddressString = "0o1000",
                ProgramAddressString = "0o1000"
            }
        },
        new object[]
        {
            "./Jsons/2.json",
            new ProjectModel
            {
                Files = new List<string>(),
                Executable = string.Empty,
                StackAddressString = "0o1000",
                ProgramAddressString = "0o1000"
            }
        },
        new object[]
        {
            "./Jsons/3.json",
            new ProjectModel
            {
                Files = new List<string>(),
                Executable = string.Empty,
                StackAddressString = "0b1010",
                ProgramAddressString = "0xFF"
            }
        },
        new object[]
        {
            "./Jsons/4.json",
            new ProjectModel
            {
                Files = new List<string>(),
                Executable = string.Empty,
                Devices = new List<string> { "C:\\a.dll" },
                StackAddressString = "0b1010",
                ProgramAddressString = "0xFF"
            }
        },
    };

    private static readonly object[] SerializeToFileAsyncTestSource =
        DeserializeProjectFileAsyncTestSource
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