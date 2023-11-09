using NUnit.Framework;
using Executor;
namespace Executor.Tests;


public class MemoryTest
{

    [Test]
    public void TestSetGetByte()
    {   
        Memory memory = new Memory();
        memory.SetByte(7, 5);
        memory.SetByte(6, 3);
        Assert.AreEqual(memory.GetByte(7), 5, "Wrong byte at addr 7");
        Assert.AreEqual(memory.GetByte(6), 3, "Wrong byte at addr 6");
    }

    [Test]
    public void TestSetGetWord()
    {   
        Memory memory = new Memory();
        memory.SetWord(4, 258);
        memory.SetWord(8, 8);
        Assert.AreEqual(memory.GetWord(4), 258, "Wrong word at addr 4");
        Assert.AreEqual(memory.GetWord(8), 8, "Wrong word at addr 8");
    }
}