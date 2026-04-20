namespace GameEngine2D.Biomes
{
    public interface IBiomeFactory
    {
        IBackground CreateBackground();
        IPlatformStyle CreatePlatformStyle();
        IDecorationStyle CreateDecorationStyle();
    }
}