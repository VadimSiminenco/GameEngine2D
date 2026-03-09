using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Core.World
{
    public class Level
    {
        public string Name { get; set; } = "Unknown Level";
        public string Background { get; set; } = "Default Background";
        public string PlatformStyle { get; set; } = "Default Platform";
        public string DecorationStyle { get; set; } = "Default Decoration";

        public int Width { get; set; } = 40;
        public int Height { get; set; } = 12;

        public List<Character> Characters { get; } = new();
        public List<Structure> Structures { get; } = new();

        public void PrintInfo()
        {
            Console.WriteLine();
            Console.WriteLine("=== Level Info ===");
            Console.WriteLine($"Name: {Name}");
            Console.WriteLine($"Background: {Background}");
            Console.WriteLine($"Platform Style: {PlatformStyle}");
            Console.WriteLine($"Decoration Style: {DecorationStyle}");
            Console.WriteLine($"Size: {Width} x {Height}");

            Console.WriteLine();
            Console.WriteLine("Characters:");
            foreach (var character in Characters)
            {
                Console.WriteLine($"- {character.GetInfo()}");
            }

            Console.WriteLine();
            Console.WriteLine("Structures:");
            foreach (var structure in Structures)
            {
                Console.WriteLine($"- {structure}");
            }
        }
    }
}