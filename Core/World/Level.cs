using System.Linq;
using System.Text;
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

        public string GetInfo()
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("=== Level Info ===");
            builder.AppendLine($"Name: {Name}");
            builder.AppendLine($"Background: {Background?.Name ?? "None"}");
            builder.AppendLine($"Platform Style: {PlatformStyle?.Name ?? "None"}");
            builder.AppendLine($"Decoration Style: {DecorationStyle?.Name ?? "None"}");
            builder.AppendLine($"Size: {Width} x {Height}");

            builder.AppendLine();
            builder.AppendLine("Characters:");

            foreach (Character character in Characters)
            {
                builder.AppendLine($"- {character.GetInfo()}");
            }

            builder.AppendLine();
            builder.AppendLine("Structures:");

            foreach (Structure structure in Structures)
            {
                builder.AppendLine($"- {structure}");
            }

            return builder.ToString();
        }
    }
}