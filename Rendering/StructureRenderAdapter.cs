using GameEngine2D.Entities.Structures;

namespace GameEngine2D.Rendering
{
    public class StructureRenderAdapter : IGameDrawable
    {
        private readonly Structure structure;
        private readonly OldSpriteRenderer adaptee;

        public StructureRenderAdapter(Structure structure, OldSpriteRenderer adaptee)
        {
            this.structure = structure;
            this.adaptee = adaptee;
        }

        public void Draw()
        {
            adaptee.RenderSprite(structure.SpriteKey, structure.X, structure.Y);
        }
    }
}