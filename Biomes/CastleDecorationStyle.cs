namespace GameEngine2D.Biomes
{
    public class CastleDecorationStyle : IDecorationStyle
    {
        public string Name => "Torches and Columns";

        public IReadOnlyList<BiomeDecoration> Decorations => new List<BiomeDecoration>
        {
            new BiomeDecoration("castle_column", 100, 350, 45, 190),
            new BiomeDecoration("castle_column", 330, 335, 45, 205),
            new BiomeDecoration("castle_column", 780, 345, 45, 195),
            new BiomeDecoration("castle_column", 1080, 330, 45, 210),

            new BiomeDecoration("castle_torch", 210, 455, 30, 75),
            new BiomeDecoration("castle_torch", 610, 455, 30, 75),
            new BiomeDecoration("castle_torch", 970, 455, 30, 75)
        };
    }
}