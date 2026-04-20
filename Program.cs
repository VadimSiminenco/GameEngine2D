using System.Text;
using GameEngine2D.Core.Engine;
using GameEngine2D.Tests;

namespace GameEngine2D
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            bool runTests = Array.Exists(
                args,
                arg => arg.Equals("--tests", StringComparison.OrdinalIgnoreCase));

            if (runTests)
            {
                EngineTests.RunAllTests();
                Console.WriteLine();
            }

            GameEngine engine = new GameEngine();
            engine.Run();
        }
    }
}