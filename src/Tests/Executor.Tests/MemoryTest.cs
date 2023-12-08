using Executor.Memories;

namespace Executor.Tests;

public class MemoryTest
{
    [Test]
    public void TestSetGetByte()
    {
        // Arrange

        var memory = new Memory();

        // Act

        memory.SetByte(7, 5);
        memory.SetByte(6, 3);

        // Assert

        Assert.That(memory.GetByte(7), Is.EqualTo(5));
        Assert.That(memory.GetByte(6), Is.EqualTo(3));
    }

    [Test]
    public void TestSetGetWord()
    {
        // Arrange

        var memory = new Memory();

        // Act

        memory.SetWord(4, 258);
        memory.SetWord(8, 8);

        // Assert

        Assert.That(memory.GetWord(4), Is.EqualTo(258));
        Assert.That(memory.GetWord(8), Is.EqualTo(8));
    }
}