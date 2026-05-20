namespace GameEngine2D.Biomes
{
    public class ForestDecorationStyle : IDecorationStyle
    {
        public string Name => "Trees and Bushes";

        public IReadOnlyList<BiomeDecoration> Decorations => new List<BiomeDecoration>
        {
            new BiomeDecoration("forest_tree", 40, 300, 90, 150),
            new BiomeDecoration("forest_tree", 180, 280, 100, 170),
            new BiomeDecoration("forest_tree", 340, 310, 85, 140),
            new BiomeDecoration("forest_tree", 520, 285, 105, 165),
            new BiomeDecoration("forest_tree", 760, 305, 90, 145),
            new BiomeDecoration("forest_tree", 980, 290, 100, 160),
            new BiomeDecoration("forest_tree", 1130, 315, 80, 135),

            new BiomeDecoration("forest_bush", 90, 500, 85, 30),
            new BiomeDecoration("forest_bush", 270, 515, 95, 28),
            new BiomeDecoration("forest_bush", 620, 505, 90, 30),
            new BiomeDecoration("forest_bush", 880, 515, 100, 28),
            new BiomeDecoration("forest_bush", 1080, 500, 90, 30)
        };
    }
}