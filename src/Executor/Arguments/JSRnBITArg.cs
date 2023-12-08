using Executor.States;
using Executor.Storages;

namespace Executor.Arguments;

public class JSRnBITArg : BaseArgument // Need to split
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

    public JSRnBITArg(IStorage storage, IState state, ushort register1, ushort mode, ushort register2) : base(storage, state)
    {
        _register1 = register1;
        _register2 = register2;
        _mode = mode;
    }
}