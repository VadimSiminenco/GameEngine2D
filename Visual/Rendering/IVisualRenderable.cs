using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public interface IVisualRenderable
    {
        void Draw(SpriteBatch spriteBatch, Texture2D pixel, int tileSize);
    }
}