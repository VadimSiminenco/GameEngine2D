using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public class MonoGameSpriteRenderer
    {
        private readonly SpriteRepository sprites;

        public MonoGameSpriteRenderer(SpriteRepository sprites)
        {
            this.sprites = sprites;
        }

        public void RenderSprite(
            SpriteBatch spriteBatch,
            Texture2D fallbackPixel,
            string spriteKey,
            int x,
            int y,
            int width,
            int height,
            bool flipHorizontally = false)
        {
            Rectangle destination = new Rectangle(x, y, width, height);

            if (sprites.TryGetTexture(spriteKey, out Texture2D texture))
            {
                SpriteEffects effects = flipHorizontally
                    ? SpriteEffects.FlipHorizontally
                    : SpriteEffects.None;

                spriteBatch.Draw(
                    texture,
                    destination,
                    null,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    effects,
                    0f);

                return;
            }

            Color fallbackColor = GetFallbackColor(spriteKey);

            spriteBatch.Draw(
                fallbackPixel,
                destination,
                fallbackColor);
        }

        private Color GetFallbackColor(string spriteKey)
        {
            return spriteKey switch
            {
                "player_sprite" => Color.Blue,
                "player_idle_sprite" => Color.Blue,
                "player_walk_1_sprite" => Color.Blue,
                "player_walk_2_sprite" => Color.Blue,
                "player_jump_sprite" => Color.Blue,
                "player_attack_sprite" => Color.Blue,

                "npc_sprite" => Color.Yellow,
                "enemy_sprite" => Color.Red,
                "house_sprite" => Color.Brown,
                "bridge_sprite" => Color.Sienna,
                "trap_sprite" => Color.LightGray,
                "boss_sprite" => Color.DarkRed,
                _ => Color.White
            };
        }
    }
}