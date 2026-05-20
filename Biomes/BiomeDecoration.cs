namespace GameEngine2D.Biomes
{
    public class BiomeDecoration
    {
        public string VisualKey { get; }
        public int X { get; }
        public int Y { get; }
        public int Width { get; }
        public int Height { get; }

        public BiomeDecoration(string visualKey, int x, int y, int width, int height)
        {
            VisualKey = visualKey;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}