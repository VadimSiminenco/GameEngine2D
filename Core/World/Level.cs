using System.Linq;
using GameEngine2D.Biomes;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Core.World
{
    public class Level
    {
        public string Name { get; set; } = "Unknown Level";

        public IBackground? Background { get; set; }
        public IPlatformStyle? PlatformStyle { get; set; }
        public IDecorationStyle? DecorationStyle { get; set; }

        public int Width { get; set; } = 40;
        public int Height { get; set; } = 12;

        public List<Character> Characters { get; } = new();
        public List<Structure> Structures { get; } = new();

        public Player? Player => Characters.OfType<Player>().FirstOrDefault();

        public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine("=== Level Info ===");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Background: {Background?.Name ?? "None"}");
            Console.WriteLine($"Platform Style: {PlatformStyle?.Name ?? "None"}");
            Console.WriteLine($"Decoration Style: {DecorationStyle?.Name ?? "None"}");
            Console.WriteLine($"Size: {Width} x {Height}");

            Console.WriteLine();
            Console.WriteLine("Characters:");
            foreach (Character character in Characters)
            {
                Console.WriteLine($"- {character.GetInfo()}");
            }

            Console.WriteLine();
            Console.WriteLine("Structures:");
            foreach (Structure structure in Structures)
            {
                Console.WriteLine($"- {structure}");
            }
        }
    }
}