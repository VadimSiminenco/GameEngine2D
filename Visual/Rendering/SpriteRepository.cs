using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public class SpriteRepository
    {
        private readonly Dictionary<string, Texture2D> textures = new();

        public void Load(GraphicsDevice graphicsDevice)
        {
            LoadTexture(graphicsDevice, "player_idle_sprite", "Content/Sprites/Player/player_idle.png");
            LoadTexture(graphicsDevice, "player_walk_1_sprite", "Content/Sprites/Player/player_walk_1.png");
            LoadTexture(graphicsDevice, "player_walk_2_sprite", "Content/Sprites/Player/player_walk_2.png");
            LoadTexture(graphicsDevice, "player_jump_sprite", "Content/Sprites/Player/player_jump.png");
            LoadTexture(graphicsDevice, "player_attack_sprite", "Content/Sprites/Player/player_attack.png");

            LoadTexture(graphicsDevice, "player_sprite", "Content/Sprites/Player/player_idle.png");

            LoadTexture(graphicsDevice, "npc_sprite", "Content/Sprites/npc.png");
            LoadTexture(graphicsDevice, "enemy_sprite", "Content/Sprites/enemy.png");
            LoadTexture(graphicsDevice, "boss_sprite", "Content/Sprites/boss.png");
            LoadTexture(graphicsDevice, "ground_tile", "Content/Sprites/ground_tile.png");
        }

        public bool TryGetTexture(string spriteKey, out Texture2D texture)
        {
            return textures.TryGetValue(spriteKey, out texture!);
        }

        private void LoadTexture(GraphicsDevice graphicsDevice, string spriteKey, string relativePath)
        {
            string fullPath = Path.Combine(AppContext.BaseDirectory, relativePath);

            if (!File.Exists(fullPath))
            {
                return;
            }

            using FileStream stream = File.OpenRead(fullPath);
            textures[spriteKey] = Texture2D.FromStream(graphicsDevice, stream);
        }
    }
}