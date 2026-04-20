using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Rendering
{
    public class CharacterRenderAdapter : IGameDrawable
    {
        private readonly Character character;
        private readonly OldSpriteRenderer adaptee;

        public CharacterRenderAdapter(Character character, OldSpriteRenderer adaptee)
        {
            this.character = character;
            this.adaptee = adaptee;
        }

        public void Draw()
        {
            adaptee.RenderSprite(character.SpriteKey, character.X, character.Y);
        }
    }
}