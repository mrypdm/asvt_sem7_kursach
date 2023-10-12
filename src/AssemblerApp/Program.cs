using AssemblerLib;

class Program
{
    static void Main(string[] args)
    {
        string main_asm_file = "main.asm";
        string[] linked_asm_files = new string[] { "macro.asm" };

        Assembler asm = new(ref main_asm_file, ref linked_asm_files);
    }
}
