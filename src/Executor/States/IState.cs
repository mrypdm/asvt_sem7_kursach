namespace Executor.States;

public interface IState
{
    void SetFlag(Flag flag, bool val);

    bool GetFlag(Flag flag);

    ushort[] Registers { get; set; }
}