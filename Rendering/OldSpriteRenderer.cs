using System;

namespace GameEngine2D.Rendering
{
    public class OldSpriteRenderer
    {
        public void RenderSprite(string spriteName, int x, int y)
        {
            Console.WriteLine($"[OldSpriteRenderer] Render '{spriteName}' at ({x}, {y})");
        }
    }
}