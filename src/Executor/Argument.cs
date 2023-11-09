namespace Executor {

  public interface IArgument {
    ushort GetValue();
    void SetValue(ushort word);
  }
  public class TwoOperandsArg: IArgument {
    private ushort Mode;
    private ushort Register;
    private State state;
    private Memory memory;
    public TwoOperandsArg(ushort Mode, ushort Register, State state, Memory memory) {
      this.memory = memory;
      this.state = state;
      this.Register = Register;
      this.Mode = Mode;
    }

    public ushort GetValue() {
      return 0;
    }
    public void SetValue(ushort word) {
      return;
    }
  }
  public class OneOperandArg: IArgument {

    private ushort Mode;
    private ushort Register;
    private State state;
    private Memory memory;
    public ushort GetValue() {
      return 0;
    }
    public void SetValue(ushort word) {
      return;
    }
    public OneOperandArg(ushort Mode, ushort Register, State state, Memory memory) {
      this.memory = memory;
      this.state = state;
      this.Register = Register;
      this.Mode = Mode;
    }
  }

  class SOBArg: IArgument {

    private ushort Offset;
    private ushort Register;
    private State state;
    private Memory memory;
    public ushort GetValue() {
      return 0;
    }
    public void SetValue(ushort word) {
      return;
    }
    public SOBArg(ushort Register, ushort Offset, State state, Memory memory) {
      this.memory = memory;
      this.state = state;
      this.Register = Register;
      this.Offset = Offset;
    }
  }

  class JSRnBITArg: IArgument {

    private ushort Mode;
    private ushort Register1;
    private ushort Register2;
    private State state;
    private Memory memory;

    public ushort GetValue() {
      return 0;
    }

    public void SetValue(ushort word) {
      return;
    }

    public JSRnBITArg(ushort Register1, ushort Mode, ushort Register2, State state, Memory memory) {
      this.memory = memory;
      this.state = state;
      this.Register1 = Register1;
      this.Register2 = Register2;
      this.Mode = Mode;
    }
  }

  class RTSArg: IArgument {

    private ushort Register;
    private State state;
    private Memory memory;

    public ushort GetValue() {
      return 0;
    }

    public void SetValue(ushort word) {
      return;
    }

    public RTSArg(ushort Register, State state, Memory memory) {
      this.memory = memory;
      this.state = state;
      this.Register = Register;
    }
  }
}