using NUnit.Framework;
using Executor;
namespace Executor.Tests;
using System;


public class CommandExecutionTest
{

    [Test]
    public void TestMovAdd()
    {
        /*  
            0o016701 - 0  MOV 55, R1
            0o000010 - 2 
            0o016702 - 4  MOV 54, R2
            0o000006 - 6
            0o060102 - 10 ADD R1, R2
            0o000055 - 12
            0o000054 - 14
        */
        ushort[] RawMem =
        {
           7617,
           8,
           7618,
           6,
           24642,
           45,
           44
        };
        Memory memory = new Memory();
        State state = new State();
        OpcodeIndentifyer opid = new OpcodeIndentifyer(state, memory);
        ushort addr = 0;
        foreach (ushort RawWord in RawMem) {
            memory.SetWord(addr, RawWord);
            addr += 2;
        }
        ushort Word;
        ICommand command;
        while (state.R[7] != 10)
        {
            Console.WriteLine($"PC {state.R[7]}.");
            Console.WriteLine($"R1 {state.R[1]}.");
            Console.WriteLine($"R2 {state.R[2]}.");
            Word = memory.GetWord(state.R[7]);
             command = opid.GetCommand(Word);
             command.Execute(command.GetArguments(Word));
             state.R[7] += 2;
        }
        Assert.AreEqual(89, state.R[2], "Wrong Summ");
    }

}