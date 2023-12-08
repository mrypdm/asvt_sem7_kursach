using Executor.Memories;
using Executor.States;

namespace Executor.Arguments;

public class JSRnBITArg : BaseArgument // Надо разделить
{
    private ushort _mode;
    private ushort _register1;
    private ushort _register2;

    public override object GetValue()
    {
        return 0;
    }

    public override void SetValue(object obj)
    {
    }

    public JSRnBITArg(IMemory memory, IState state, ushort register1, ushort mode, ushort register2) : base(memory, state)
    {
        _register1 = register1;
        _register2 = register2;
        _mode = mode;
    }
}