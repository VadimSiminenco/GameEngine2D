namespace GameEngine2D.Biomes
{
    public class CastleBiomeFactory : IBiomeFactory
    {
        public IBackground CreateBackground()
        {
            return new CastleBackground();
        }

        public IPlatformStyle CreatePlatformStyle()
        {
            return new CastlePlatformStyle();
        }

        public IDecorationStyle CreateDecorationStyle()
        {
            return new CastleDecorationStyle();
        }
    }
}