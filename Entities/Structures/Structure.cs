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

        public override string ToString()
        {
            return $"{Name} [Base={BasePart}, Middle={MiddlePart}, Top={TopPart}, Pos=({X},{Y})]";
        }
    }
}