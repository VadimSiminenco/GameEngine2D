using GameEngine2D.Core.Engine;
using GameEngine2D.Tests;

namespace GameEngine2D
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            EngineTests.RunAllTests();

            GameEngine engine = new GameEngine();
            engine.RunDemo();

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}