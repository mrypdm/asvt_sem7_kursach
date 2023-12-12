using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Commands.OneOperands;
using Executor.Commands.TwoOperands;
using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class BranchingTest
{
    [Test]
    public void TestBranch()
    {
        // Arrange

        ushort[] rawMem =
        {
            Convert.ToUInt16("102402", 8), // 0 BVS 2
            Convert.ToUInt16("102002", 8), // 2 BVC 2
            Convert.ToUInt16("001002", 8), // 4 BNE 2
            Convert.ToUInt16("001402", 8), // 6 BEQ 2
            Convert.ToUInt16("000402", 8), // 8 BR 2
        };

        var memory = new Memory();
        var state = new State();
        var opcodeIdentifier = new CommandParser(memory, state);

        for (var i = 0; i < rawMem.Length; i++)
        {
            memory.SetWord((ushort)(2 * i), rawMem[i]);
        }

        // Act
        state.Z = true;
        state.V = true;

        for (var i = 0; i < 10; i += 2)
        {
            TestContext.WriteLine($"PC {state.Registers[7]}");
            var word = memory.GetWord((ushort)i);
            var command = opcodeIdentifier.GetCommand(word);
            command.Execute(command.GetArguments(word));
        }


        Assert.That(state.Registers[7], Is.EqualTo(12));

        state.Z = false;
        state.V = false;


        for (var i = 0; i < 10; i += 2)
        {
            TestContext.WriteLine($"PC {state.Registers[7]}");
            var word = memory.GetWord((ushort)i);
            var command = opcodeIdentifier.GetCommand(word);
            command.Execute(command.GetArguments(word));
        }

        Assert.That(state.Registers[7], Is.EqualTo(24));
    }

    [Test]
    public void TestBrNegative()
    {
        var word = Convert.ToUInt16("000765", 8); //1_11110101
        var memory = new Memory();
        var state = new State();
        state.Registers[7] = 72; // next to instruction

        var opcodeIdentifier = new CommandParser(memory, state);
        var command = opcodeIdentifier.GetCommand(word);
        command.Execute(command.GetArguments(word));

        Assert.That(state.Registers[7], Is.EqualTo(72 - 2 - 20));
    }

    [Test]
    [TestCase((ushort)2, (ushort)4)]
    [TestCase((ushort)1, (ushort)16)]
    public void TestSob(ushort initialRegisterValue, ushort expectedProgramCounterValue)
    {
        // Arrange

        var memory = new Memory();
        var state = new State
        {
            Registers =
            {
                [0] = initialRegisterValue,
                [7] = 16 // next to instruction
            }
        };

        var arg = new SobArgument(memory, state, 0, 6);
        var command = new SOB(memory, state);

        // Act

        command.Execute(new IArgument[] { arg });

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(state.Registers[0], Is.EqualTo(initialRegisterValue - 1));
            Assert.That(state.Registers[7], Is.EqualTo(expectedProgramCounterValue));
        });
    }

    [Test]
    public void TestJsr_ShouldSwapStackTopValueAndProgramCounter()
    {
        // Arrange

        var memory = new Memory();
        memory.SetWord(500, 1500);
        var state = new State
        {
            Registers =
            {
                [6] = 500,
                [7] = 1002 // next to jsr
            }
        };

        var arg0 = new RegisterWordArgument(memory, state, 0, 7);
        var arg1 = new RegisterWordArgument(memory, state, 3, 6);
        var command = new JSR(memory, state);

        // Act

        command.Execute(new IArgument[] { arg0, arg1 });

        // Assert

        Assert.Multiple(() =>
        {
            Assert.That(state.Registers[6], Is.EqualTo(500));
            Assert.That(state.Registers[7], Is.EqualTo(1500));
            Assert.That(memory.GetWord(500), Is.EqualTo(1002));
        });
    }

    [Test]
    [TestCase((ushort)3, (ushort)1500)]
    [TestCase((ushort)6, (ushort)2504)]
    public void TestJmp(ushort mode, ushort expectedAddress)
    {
        // Arrange

        var memory = new Memory();
        memory.SetWord(1002, 1500);

        var state = new State
        {
            Registers =
            {
                [7] = 1002 // next to jmp
            }
        };

        var arg = new RegisterWordArgument(memory, state, mode, 7);
        var command = new JMP(memory, state);

        // Act

        command.Execute(new IArgument[] { arg });

        // Assert

        Assert.That(state.Registers[7], Is.EqualTo(expectedAddress));
    }
}