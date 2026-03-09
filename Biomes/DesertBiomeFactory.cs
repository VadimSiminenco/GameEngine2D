namespace GameEngine2D.Biomes
{
    public class DesertBiomeFactory : IBiomeFactory
    {
        public IBackground CreateBackground()
        {
            return new DesertBackground();
        }

        public IPlatformStyle CreatePlatformStyle()
        {
            return new DesertPlatformStyle();
        }

        public IDecorationStyle CreateDecorationStyle()
        {
            return new DesertDecorationStyle();
        }
    }
}