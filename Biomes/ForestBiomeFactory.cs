namespace GameEngine2D.Biomes
{
    public class ForestBiomeFactory : IBiomeFactory
    {
        public IBackground CreateBackground()
        {
            return new ForestBackground();
        }

        public IPlatformStyle CreatePlatformStyle()
        {
            return new ForestPlatformStyle();
        }

        public IDecorationStyle CreateDecorationStyle()
        {
            return new ForestDecorationStyle();
        }
    }
}