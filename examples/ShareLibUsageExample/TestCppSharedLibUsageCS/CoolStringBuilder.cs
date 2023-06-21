namespace TestCppSharedLibUsageCS;

public class CoolStringBuilder : IDisposable
{
    private IntPtr _ptr;

    private IntPtr SafePtr
    {
        get
        {
            if (_ptr != IntPtr.Zero)
            {
                return _ptr;
            }

            throw new NullReferenceException(nameof(_ptr));
        }
    }

    public CoolStringBuilder()
    {
        _ptr = CoolStringBuilderExtern.Create();
    }

    public CoolStringBuilder Append(string value)
    {
        CoolStringBuilderExtern.AppendSafe(SafePtr, value);
        return this;
    }

    public CoolStringBuilder AppendLine(string value)
    {
        CoolStringBuilderExtern.AppendLineSafe(SafePtr, value);
        return this;
    }

    public string? GetString()
    {
        return CoolStringBuilderExtern.GetStringSafe(_ptr);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_ptr != IntPtr.Zero)
        {
            CoolStringBuilderExtern.DisposeSafe(_ptr);
            _ptr = IntPtr.Zero;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    ~CoolStringBuilder()
    {
        Dispose(false);
    }
}