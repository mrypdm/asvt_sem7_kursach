using System;
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
        var opcodeIdentifier = new OpcodeIdentifier(state, memory);

        for (var i = 0; i < rawMem.Length; i++)
        {
            memory.SetWord((ushort)(2 * i), rawMem[i]);
        }

        // Act
        state.Z = true;
        state.V = true;

        for (var i = 0;i < 10; i+=2) 
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

        var opcodeIdentifier = new OpcodeIdentifier(state, memory);
        var command = opcodeIdentifier.GetCommand(word);
        command.Execute(command.GetArguments(word));

        Assert.That(state.Registers[7], Is.EqualTo(72 - 2 - 20));
    }
}