using System;
using Executor.Memories;
using Executor.States;

namespace Executor.Tests;

public class CommandExecutionTest
{
    [Test]
    public void TestMovAdd()
    {
        // Arrange

        ushort[] rawMem =
        {
            Convert.ToUInt16("016701", 8), // 0 MOV 10(PC), R1
            Convert.ToUInt16("000006", 8), // 2 10
            Convert.ToUInt16("016702", 8), // 4 MOV 6(PC), R2
            Convert.ToUInt16("000004", 8), // 6 6
            Convert.ToUInt16("060102", 8), // 8 ADD R1, R2
            Convert.ToUInt16("000055", 8), //10 55
            Convert.ToUInt16("000054", 8) //12 54
        };

        var memory = new Memory();
        var state = new State();
        var opcodeIdentifier = new OpcodeIdentifier(state, memory);

        for (var i = 0; i < rawMem.Length; i++)
        {
            memory.SetWord((ushort)(2 * i), rawMem[i]);
        }

        // Act

        while (state.Registers[7] != 10)
        {
            TestContext.WriteLine($"PC {state.Registers[7]} | R1 {state.Registers[1]} | R2 {state.Registers[2]}");
            var word = memory.GetWord(state.Registers[7]);
            state.Registers[7] += 2;
            var command = opcodeIdentifier.GetCommand(word);
            command.Execute(command.GetArguments(word));
        }

        // Assert

        Assert.That(state.Registers[2], Is.EqualTo(89));
    }
}