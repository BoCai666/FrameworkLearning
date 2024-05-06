using System.Reflection.Emit;

namespace ConsoleApp;
partial class Program
{
    static void Main(string[] args)
    {
        HelloFrom("hello world");
    }
    static partial void HelloFrom(string name);
}