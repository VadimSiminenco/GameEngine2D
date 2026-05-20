namespace GameEngine2D.Biomes
{
    public class DesertDecorationStyle : IDecorationStyle
    {
        public string Name => "Cacti and Stones";

        public IReadOnlyList<BiomeDecoration> Decorations => new List<BiomeDecoration>
        {
            new BiomeDecoration("desert_cactus", 120, 435, 35, 90),
            new BiomeDecoration("desert_cactus", 390, 455, 30, 75),
            new BiomeDecoration("desert_cactus", 870, 430, 38, 95),
            new BiomeDecoration("desert_cactus", 1120, 460, 28, 70),

            new BiomeDecoration("desert_rock", 230, 535, 70, 24),
            new BiomeDecoration("desert_rock", 690, 525, 85, 28),
            new BiomeDecoration("desert_rock", 1010, 540, 75, 24)
        };
    }
}