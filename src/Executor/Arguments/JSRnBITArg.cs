namespace Executor;

class JSRnBITArg: IArgument {

    private ushort _Mode;
    private ushort _Register1;
    private ushort _Register2;
    private State _state;
    private Memory _memory;

    public ushort GetValue() {
      return 0;
    }

    public void SetValue(ushort word) {
      return;
    }

    public JSRnBITArg(ushort Register1, ushort Mode, ushort Register2, State state, Memory memory) {
      this._memory = memory;
      this._state = state;
      this._Register1 = Register1;
      this._Register2 = Register2;
      this._Mode = Mode;
    }
}