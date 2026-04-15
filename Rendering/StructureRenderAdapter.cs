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
            adaptee.RenderSprite(GetSpriteName(), structure.X, structure.Y);
        }

        private string GetSpriteName()
        {
            return structure.Name switch
            {
                "House" => "house_sprite",
                "Bridge" => "bridge_sprite",
                "Trap" => "trap_sprite",
                _ => "unknown_structure_sprite"
            };
        }
    }
}