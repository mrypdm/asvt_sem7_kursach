using NUnit.Framework;
using Executor;
namespace Executor.Tests;


public class ParsingTests
{

    [Test]
    public void TestTwoOperands()
    {   
        Memory memory = new Memory();
        State state = new State();
        ushort Word1 = 0b1010111000000; // 0o012700 Mov #5, R0
        ushort Word2 = 0b110110010110101; // 0o066265 Add 300(r2), 200 (r5)
        ushort Word3 = 0b1110111000001; // 0o016701 Mov offset, R1
        TwoOperands command = new MOV(state, memory);
        Assert.AreEqual(command.GetRegister1(Word1), 7, "Wrong Register1 in word1");
        Assert.AreEqual(command.GetMode1(Word1), 2, "Wrong Mode1 in word1");
        Assert.AreEqual(command.GetRegister2(Word1), 0, "Wrong Register2 in word1");
        Assert.AreEqual(command.GetMode2(Word1), 0, "Wrong Mode2 in word1");
        Assert.AreEqual(command.GetRegister1(Word2), 2, "Wrong Register1 in word2");
        Assert.AreEqual(command.GetMode1(Word2), 6, "Wrong Mode1 in word2");
        Assert.AreEqual(command.GetRegister2(Word2), 5, "Wrong Register2 in word2");
        Assert.AreEqual(command.GetMode2(Word2), 6, "Wrong Mode2 in word2");
        Assert.AreEqual(command.GetRegister1(Word3), 7, "Wrong Register1 in word3");
        Assert.AreEqual(command.GetMode1(Word3), 6, "Wrong Mode1 in word3");
        Assert.AreEqual(command.GetRegister2(Word3), 1, "Wrong Register2 in word3");
        Assert.AreEqual(command.GetMode2(Word3), 0, "Wrong Mode2 in word3");
    }
}