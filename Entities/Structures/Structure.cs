namespace GameEngine2D.Entities.Structures
{
    public class Structure
    {
        public string Name { get; set; } = string.Empty;
        public string BasePart { get; set; } = string.Empty;
        public string MiddlePart { get; set; } = string.Empty;
        public string TopPart { get; set; } = string.Empty;

        public int X { get; set; }
        public int Y { get; set; }

        public char MapSymbol { get; set; } = 'S';
        public string SpriteKey { get; set; } = "unknown_structure_sprite";

        public override string ToString()
        {
            return $"{Name} [Base={BasePart}, Middle={MiddlePart}, Top={TopPart}, Pos=({X},{Y}), Symbol={MapSymbol}, Sprite={SpriteKey}]";
        }
    }
}