using System;
using Executor.Arguments;
using Executor.Arguments.Abstraction;
using Executor.Commands;
using Executor.Commands.OneOperands;
using Executor.Commands.TwoOperands;
using Executor.States;
using Executor.Storages;

namespace Executor.Tests;

public class TwoOperandTest
{

    
    [Test]
    [TestCase((ushort)1500, (ushort)1500, new bool[] {false, false})]
    [TestCase((ushort)0, (ushort)2504, new bool[] {true, false})]
    [TestCase((ushort)33088, (ushort)32768, new bool[] {false, true})]
    public void TestBIT(ushort value1, ushort value2, bool[] ExpectedResult)
    {
        // Arrange

        var memory = new Memory();
        var state = new State
        {
            Registers =
            {
                [1] = value1,
                [2] = value2 
            }
        };

        var arg = new IArgument[]
        {
            new RegisterWordArgument(memory, state, 0, 1),
            new RegisterWordArgument(memory, state, 0, 2)
        };
        var command = new BIT(memory, state);

        // Act

        command.Execute(arg);

        // Assert

        Assert.That(state.Z, Is.EqualTo(ExpectedResult[0]));
        Assert.That(state.N, Is.EqualTo(ExpectedResult[1]));
        Assert.That(state.V, Is.EqualTo(false));
        Assert.That(state.C, Is.EqualTo(false));
    }

    [Test]
    [TestCase((ushort)32255, new bool[] {false, false})]
    [TestCase((ushort)128, new bool[] {true, false})]
    [TestCase((ushort)65535, new bool[] {false, true})]
    public void TestBIT(ushort value1, bool[] ExpectedResult)
    {
        // Arrange

        var memory = new Memory();
        memory.SetWord(1002, value1);
        var state = new State
        {
            Registers =
            {
                [1] = 1002,
            }
        };

        var arg = new IArgument[]
        {
            new RegisterByteArgument(memory, state, 2, 1),
            new RegisterByteArgument(memory, state, 2, 1)
        };
        var command = new BITB(memory, state);

        // Act

        command.Execute(arg);

        // Assert

        Assert.That(state.Z, Is.EqualTo(ExpectedResult[0]));
        Assert.That(state.N, Is.EqualTo(ExpectedResult[1]));
        Assert.That(state.V, Is.EqualTo(false));
        Assert.That(state.C, Is.EqualTo(false));
    }
}
