using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Terminal;

public static class Program
{
    private const string PipeName = "MainToChildPipe";

    private static bool _termination;
    private static string _otherSideName;
    private static PipeStream _pipe;

    public static void Main(string[] args)
    {
        if (args[0] == "child")
        {
            InitChild();
        }
        else
        {
            InitMain();
        }

        var readerTask = Task.Run(ReaderTask);
        var pipeChecker = PipeChecker();

        WriterTask();

        _pipe.Close();
        readerTask.Wait();
        pipeChecker.Wait();
    }

    /// <summary>
    /// If running as Main Terminal
    /// </summary>
    private static void InitMain()
    {
        Console.Title = "Main Terminal";
        _otherSideName = "child";

        var pipe = new NamedPipeServerStream(PipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte,
            PipeOptions.Asynchronous);

        var process = new Process();

        if (OperatingSystem.IsLinux())
        {
            process.StartInfo.FileName = "gnome-terminal";
            process.StartInfo.Arguments = "-- ./Terminal child";
        }
        else if (OperatingSystem.IsWindows())
        {
            process.StartInfo.FileName = "Terminal";
            process.StartInfo.Arguments = "child";
            process.StartInfo.UseShellExecute = true;
        }
        else
        {
            Console.WriteLine($"Unsupported OS: {Environment.OSVersion.Platform}");
        }

        process.Start();

        Console.Write("Waiting for connection... ");
        pipe.WaitForConnection();
        Console.WriteLine("Connected");

        _pipe = pipe;
    }

    /// <summary>
    /// If running as Child Terminal
    /// </summary>
    private static void InitChild()
    {
        Console.Title = "Child Terminal";
        _otherSideName = "parent";

        var pipe = new NamedPipeClientStream(".", PipeName, PipeDirection.InOut, PipeOptions.Asynchronous);
        pipe.Connect();
        _pipe = pipe;
    }

    /// <summary>
    /// Task for reading stdin and sending messages to other side
    /// </summary>
    private static void WriterTask()
    {
        try
        {
            using var writer = new StreamWriter(_pipe);
            while (_pipe.IsConnected)
            {
                var line = Console.ReadLine();

                if (line == null)
                {
                    continue;
                }

                if (line == "!shutdown")
                {
                    _termination = true;
                    break;
                }

                writer.WriteLine(line);
                writer.Flush();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Writer] [ERR] [{e.Message}]");
        }
    }

    /// <summary>
    /// Task for reading messages from other side and writing them to stdout
    /// </summary>
    private static void ReaderTask()
    {
        try
        {
            using var reader = new StreamReader(_pipe);
            var buffer = new char[1024];
            while (_pipe.IsConnected && !_termination)
            {
                var count = reader.Read(buffer, 0, buffer.Length);

                if (count == 0)
                {
                    continue;
                }

                Console.Write($"({_otherSideName}): ");
                Console.Write(buffer, 0, count);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"[Reader] [ERR] [{e.Message}]");
        }
    }

    /// <summary>
    /// Task for check pipe status and terminating process if it is disconnected
    /// </summary>
    private static async Task PipeChecker()
    {
        while (_pipe.IsConnected && !_termination)
        {
            await Task.Delay(TimeSpan.FromMilliseconds(100));
        }

        if (!_pipe.IsConnected && !_termination)
        {
            Console.WriteLine("Pipe is disconnected. Shutting down");
            Process.GetCurrentProcess().Kill();
        }
    }
}