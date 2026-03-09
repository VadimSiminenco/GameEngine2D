namespace GameEngine2D.Construction
{
    public class StructureDirector
    {
        public void Construct(IStructureBuilder builder)
        {
            builder.Reset();
            builder.BuildBase();
            builder.BuildMiddle();
            builder.BuildTop();
        }
    }
}