using NUnit.Framework;
using Executor;
namespace Executor.Tests;


public class OpcodeIndentifyerTest
{

    [Test]
    public void TestGetCommand()
    {   
        State state = new State();
        Memory memory = new Memory();
        OpcodeIndentifyer OpId = new OpcodeIndentifyer(state, memory);
        ushort Word1 = 0b1010111000000; // MOV
        ICommand command = OpId.GetCommand(Word1);
        Assert.AreEqual(command.Opcode, 0b1_0000_0000_0000, "Wrong opcode for MOV");
    }
}