using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Extensions;
using Executor.States;
using Executor.Storages;

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
            Convert.ToUInt16("000006", 8), // 2 6
            Convert.ToUInt16("016702", 8), // 4 MOV 6(PC), R2
            Convert.ToUInt16("000004", 8), // 6 4
            Convert.ToUInt16("060102", 8), // 8 ADD R1, R2
            Convert.ToUInt16("000055", 8), //10 55
            Convert.ToUInt16("000054", 8) //12 54
        };

        var memory = new Memory();
        var state = new State();
        var opcodeIdentifier = new CommandParser(memory, state);

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

    [Test]
    public void TestMovIncSub()
    {
        // Arrange

        ushort[] rawMem =
        {
            Convert.ToUInt16("016701", 8), // 0  MOV 10(PC), R1
            Convert.ToUInt16("000010", 8), // 2  6
            Convert.ToUInt16("016702", 8), // 4  MOV 6(PC), R2
            Convert.ToUInt16("000006", 8), // 6  4
            Convert.ToUInt16("005202", 8), // 8  INC R2
            Convert.ToUInt16("160102", 8), // 10 Sub R1, R2
            Convert.ToUInt16("000055", 8), // 12 55
            Convert.ToUInt16("000054", 8), // 14 54
        };

        var memory = new Memory();
        var state = new State();
        var opcodeIdentifier = new CommandParser(memory, state);

        for (var i = 0; i < rawMem.Length; i++)
        {
            memory.SetWord((ushort)(2 * i), rawMem[i]);
        }

        // Act

        while (state.Registers[7] != 12)
        {
            TestContext.WriteLine($"PC {state.Registers[7]} | R1 {state.Registers[1]} | R2 {state.Registers[2]}");
            var word = memory.GetWord(state.Registers[7]);
            state.Registers[7] += 2;
            var command = opcodeIdentifier.GetCommand(word);
            command.Execute(command.GetArguments(word));
        }

        // Assert

        Assert.That(state.Registers[2], Is.EqualTo(0));
    }

    [Test]
    public void MarkTest()
    {
        // Arrange

        var memory = new Memory();
        var state = new State
        {
            Registers =
            {
                [5] = 1200,
                [6] = 1024,
                [7] = 1500 // after JSR
            }
        };

        memory.PushToStack(state, 1200);
        memory.PushToStack(state, 5);
        memory.PushToStack(state, 3);
        memory.PushToStack(state, 1203);
        memory.PushToStack(state, Convert.ToUInt16("006403", 8)); // MARK 3
        state.Registers[5] = 1502;

        // Act

        var markArgs = new IArgument[] { new MarkArgument(3) };
        var mark = new MARK(memory, state);
        mark.Execute(markArgs);

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(state.Registers[5], Is.EqualTo(1200));
            Assert.That(state.Registers[6], Is.EqualTo(1024));
            Assert.That(state.Registers[7], Is.EqualTo(1502));
        });
    }
}