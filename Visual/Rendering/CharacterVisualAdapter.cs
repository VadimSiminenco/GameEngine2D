using System.Linq;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Weapons;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public class CharacterVisualAdapter : IVisualRenderable
    {
        private readonly Character character;
        private readonly CharacterCodeAdaptee adaptee;
        private readonly float animationTime;
        private readonly string playerVisualState;
        private readonly bool isPlayerHit;

        public CharacterVisualAdapter(
            Character character,
            CharacterCodeAdaptee adaptee,
            float animationTime,
            string playerVisualState = "Idle",
            bool isPlayerHit = false)
        {
            this.character = character;
            this.adaptee = adaptee;
            this.animationTime = animationTime;
            this.playerVisualState = playerVisualState;
            this.isPlayerHit = isPlayerHit;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, int tileSize)
        {
            int x = character.X * tileSize;
            int y = character.Y * tileSize;

            if (character is Player player)
            {
                WeaponEffectType weaponEffect = GetPlayerWeaponEffect(player);

                adaptee.SpecificRequestForPlayer(
                    spriteBatch,
                    pixel,
                    player,
                    x,
                    y,
                    tileSize,
                    playerVisualState,
                    animationTime,
                    isPlayerHit,
                    weaponEffect);

                return;
            }

            if (character is Npc npc)
            {
                adaptee.SpecificRequestForNpc(
                    spriteBatch,
                    pixel,
                    npc,
                    x,
                    y,
                    tileSize,
                    animationTime);

                return;
            }

            if (character is Enemy enemy)
            {
                adaptee.SpecificRequestForEnemy(
                    spriteBatch,
                    pixel,
                    enemy,
                    x,
                    y,
                    tileSize,
                    animationTime);
            }
        }

        private WeaponEffectType GetPlayerWeaponEffect(Player player)
        {
            if (player.EquippedWeapon == null)
            {
                return WeaponEffectType.None;
            }

            IReadOnlyList<WeaponEffectType> effects = player.EquippedWeapon.GetEffects();

            if (effects.Contains(WeaponEffectType.Fire))
            {
                return WeaponEffectType.Fire;
            }

            if (effects.Contains(WeaponEffectType.Ice))
            {
                return WeaponEffectType.Ice;
            }

            return WeaponEffectType.None;
        }
    }
}