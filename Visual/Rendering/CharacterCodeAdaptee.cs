using System;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public class CharacterCodeAdaptee
    {
        public void SpecificRequestForPlayer(
            SpriteBatch spriteBatch,
            Texture2D pixel,
            Player player,
            int x,
            int y,
            int tileSize,
            string playerState,
            float animationTime,
            bool isTakingHit,
            WeaponEffectType weaponEffect)
        {
            bool facingLeft = player.FacingDirection < 0;

            float idleBreath = MathF.Sin(animationTime * 2.5f) * 1.5f;
            float walkBounce = MathF.Sin(animationTime * 10f) * 4f;
            float legSwing = MathF.Sin(animationTime * 10f) * 5f;
            float armSwing = MathF.Sin(animationTime * 10f) * 3f;

            float hitShake = isTakingHit ? MathF.Sin(animationTime * 35f) * 3f : 0f;
            int hitPushX = 0;
            int hitPushY = 0;

            if (isTakingHit)
            {
                hitPushX = facingLeft ? 6 : -6;
                hitPushY = -2;
            }

            int verticalOffset = 0;

            if (playerState == "Idle")
            {
                verticalOffset = (int)idleBreath;
            }
            else if (playerState == "Moving Left" || playerState == "Moving Right")
            {
                verticalOffset = (int)walkBounce;
            }
            else if (playerState == "Jumping")
            {
                verticalOffset = -10;
            }
            else if (playerState == "Attacking")
            {
                verticalOffset = -2;
            }

            int drawX = x - tileSize + (int)hitShake + hitPushX;
            int drawY = y - tileSize * 2 + verticalOffset + hitPushY;

            Color skin = isTakingHit ? new Color(255, 205, 205) : new Color(246, 213, 182);
            Color hair = new Color(118, 72, 34);

            Color tunicLight = isTakingHit ? new Color(255, 215, 215) : new Color(168, 78, 54);
            Color tunicDark = isTakingHit ? new Color(210, 120, 130) : new Color(118, 52, 40);

            Color shoulderLight = isTakingHit ? new Color(240, 200, 210) : new Color(145, 145, 155);
            Color shoulderDark = isTakingHit ? new Color(180, 105, 115) : new Color(95, 95, 108);

            Color glove = new Color(118, 74, 34);
            Color belt = new Color(98, 62, 30);
            Color buckle = new Color(220, 172, 68);

            Color pants = isTakingHit ? new Color(130, 110, 120) : new Color(72, 82, 104);
            Color boot = new Color(132, 78, 38);

            Color outline = new Color(45, 35, 30);

            Color blade = weaponEffect switch
            {
                WeaponEffectType.Fire => new Color(255, 140, 40),
                WeaponEffectType.Ice => new Color(120, 220, 255),
                _ => new Color(220, 222, 228)
            };

            Color bladeGlow = weaponEffect switch
            {
                WeaponEffectType.Fire => new Color(255, 80, 25, 120),
                WeaponEffectType.Ice => new Color(120, 230, 255, 120),
                _ => new Color(0, 0, 0, 0)
            };

            int leftLegOffset = 0;
            int rightLegOffset = 0;
            int leftArmOffset = 0;
            int rightArmOffset = 0;

            if (playerState == "Moving Left" || playerState == "Moving Right")
            {
                leftLegOffset = (int)legSwing;
                rightLegOffset = (int)(-legSwing);
                leftArmOffset = (int)(-armSwing);
                rightArmOffset = (int)armSwing;
            }

            if (playerState == "Jumping")
            {
                leftLegOffset = -4;
                rightLegOffset = -4;
                leftArmOffset = -2;
                rightArmOffset = -2;
            }

            if (playerState == "Attacking")
            {
                if (facingLeft)
                {
                    leftArmOffset = -4;
                    rightArmOffset = 2;
                }
                else
                {
                    leftArmOffset = 2;
                    rightArmOffset = -4;
                }
            }

            // Плащ
            if (facingLeft)
            {
                DrawRect(spriteBatch, pixel, drawX + 12, drawY + 42, 16, 40, new Color(90, 38, 32));
                DrawRect(spriteBatch, pixel, drawX + 6, drawY + 50, 18, 34, new Color(150, 52, 42));
            }
            else
            {
                DrawRect(spriteBatch, pixel, drawX + 68, drawY + 42, 16, 40, new Color(90, 38, 32));
                DrawRect(spriteBatch, pixel, drawX + 72, drawY + 50, 18, 34, new Color(150, 52, 42));
            }

            // Голова без шлема
            DrawRect(spriteBatch, pixel, drawX + 34, drawY + 14, 28, 22, skin);
            DrawRect(spriteBatch, pixel, drawX + 31, drawY + 8, 34, 10, hair);
            DrawRect(spriteBatch, pixel, drawX + 30, drawY + 16, 4, 8, hair);
            DrawRect(spriteBatch, pixel, drawX + 62, drawY + 16, 4, 8, hair);

            DrawRect(spriteBatch, pixel, drawX + 40, drawY + 22, 3, 4, outline);
            DrawRect(spriteBatch, pixel, drawX + 53, drawY + 22, 3, 4, outline);

            // Шея
            DrawRect(spriteBatch, pixel, drawX + 43, drawY + 34, 10, 6, skin);

            // Плечи
            DrawRect(spriteBatch, pixel, drawX + 26, drawY + 40, 14, 10, shoulderDark);
            DrawRect(spriteBatch, pixel, drawX + 56, drawY + 40, 14, 10, shoulderDark);

            DrawRect(spriteBatch, pixel, drawX + 28, drawY + 42, 10, 6, shoulderLight);
            DrawRect(spriteBatch, pixel, drawX + 58, drawY + 42, 10, 6, shoulderLight);

            // Тело
            DrawRect(spriteBatch, pixel, drawX + 34, drawY + 40, 28, 26, tunicDark);
            DrawRect(spriteBatch, pixel, drawX + 36, drawY + 42, 24, 12, tunicLight);
            DrawRect(spriteBatch, pixel, drawX + 36, drawY + 54, 24, 10, tunicDark);

            // Пояс
            DrawRect(spriteBatch, pixel, drawX + 34, drawY + 62, 28, 5, belt);
            DrawRect(spriteBatch, pixel, drawX + 45, drawY + 60, 8, 8, buckle);

            // Левая рука
            DrawRect(spriteBatch, pixel, drawX + 20, drawY + 46 + leftArmOffset / 2, 10, 18, skin);
            DrawRect(spriteBatch, pixel, drawX + 18, drawY + 62 + leftArmOffset / 2, 10, 10, glove);

            // Правая рука
            DrawRect(spriteBatch, pixel, drawX + 66, drawY + 46 + rightArmOffset / 2, 10, 18, skin);
            DrawRect(spriteBatch, pixel, drawX + 68, drawY + 62 + rightArmOffset / 2, 10, 10, glove);

            // Ноги
            DrawRect(spriteBatch, pixel, drawX + 36, drawY + 68 + leftLegOffset / 2, 10, 22, pants);
            DrawRect(spriteBatch, pixel, drawX + 50, drawY + 68 + rightLegOffset / 2, 10, 22, pants);

            // Ботинки
            DrawRect(spriteBatch, pixel, drawX + 34, drawY + 88 + leftLegOffset / 2, 14, 10, boot);
            DrawRect(spriteBatch, pixel, drawX + 48, drawY + 88 + rightLegOffset / 2, 14, 10, boot);

            DrawPlayerWeapon(
                spriteBatch,
                pixel,
                drawX,
                drawY,
                facingLeft,
                playerState,
                blade,
                buckle,
                weaponEffect,
                bladeGlow);
        }

        public void SpecificRequestForNpc(
            SpriteBatch spriteBatch,
            Texture2D pixel,
            Npc npc,
            int x,
            int y,
            int tileSize,
            float animationTime)
        {
            float idleBreath = MathF.Sin(animationTime * 2.0f) * 1.5f;

            int drawX = x - tileSize;
            int drawY = y - tileSize * 2 + (int)idleBreath;

            Color hat = new Color(85, 120, 55);
            Color robeLight = new Color(198, 168, 95);
            Color robeDark = new Color(145, 112, 55);
            Color skin = new Color(244, 214, 180);
            Color hair = new Color(110, 70, 40);
            Color boot = new Color(120, 75, 35);
            Color staff = new Color(120, 85, 50);
            Color leaf = new Color(90, 150, 80);
            Color outline = new Color(45, 35, 30);

            DrawRect(spriteBatch, pixel, drawX + 10, drawY + 42, 6, 40, staff);
            DrawRect(spriteBatch, pixel, drawX + 5, drawY + 32, 16, 12, leaf);

            DrawRect(spriteBatch, pixel, drawX + 32, drawY + 16, 32, 22, skin);
            DrawRect(spriteBatch, pixel, drawX + 28, drawY + 10, 40, 10, hair);
            DrawRect(spriteBatch, pixel, drawX + 26, drawY + 6, 44, 8, hat);

            DrawRect(spriteBatch, pixel, drawX + 40, drawY + 24, 4, 5, outline);
            DrawRect(spriteBatch, pixel, drawX + 52, drawY + 24, 4, 5, outline);

            DrawRect(spriteBatch, pixel, drawX + 28, drawY + 40, 40, 10, hat);

            DrawRect(spriteBatch, pixel, drawX + 24, drawY + 50, 48, 32, robeLight);
            DrawRect(spriteBatch, pixel, drawX + 20, drawY + 56, 12, 24, robeDark);
            DrawRect(spriteBatch, pixel, drawX + 64, drawY + 56, 12, 24, robeDark);

            DrawRect(spriteBatch, pixel, drawX + 28, drawY + 80, 16, 18, boot);
            DrawRect(spriteBatch, pixel, drawX + 52, drawY + 80, 16, 18, boot);
        }

        public void SpecificRequestForEnemy(
            SpriteBatch spriteBatch,
            Texture2D pixel,
            Enemy enemy,
            int x,
            int y,
            int tileSize,
            float animationTime)
        {
            float shake = MathF.Sin(animationTime * 5.0f) * 2f;

            int drawX = x - tileSize + (int)shake;
            int drawY = y - tileSize * 2;

            Color skinLight = new Color(98, 155, 74);
            Color skinDark = new Color(60, 108, 50);
            Color armor = new Color(102, 92, 78);
            Color armorDark = new Color(76, 66, 54);
            Color eye = new Color(222, 72, 40);
            Color weaponWood = new Color(120, 82, 42);
            Color metal = new Color(155, 155, 160);
            Color outline = new Color(40, 30, 25);

            DrawRect(spriteBatch, pixel, drawX + 8, drawY + 54, 18, 18, new Color(120, 120, 126));
            DrawRect(spriteBatch, pixel, drawX + 12, drawY + 58, 10, 10, armorDark);

            DrawRect(spriteBatch, pixel, drawX + 30, drawY + 18, 36, 22, skinLight);
            DrawRect(spriteBatch, pixel, drawX + 26, drawY + 12, 12, 12, skinDark);
            DrawRect(spriteBatch, pixel, drawX + 58, drawY + 12, 12, 12, skinDark);

            DrawRect(spriteBatch, pixel, drawX + 38, drawY + 25, 4, 5, eye);
            DrawRect(spriteBatch, pixel, drawX + 54, drawY + 25, 4, 5, eye);
            DrawRect(spriteBatch, pixel, drawX + 44, drawY + 34, 8, 3, outline);

            DrawRect(spriteBatch, pixel, drawX + 24, drawY + 44, 48, 30, armor);
            DrawRect(spriteBatch, pixel, drawX + 18, drawY + 50, 12, 22, armorDark);
            DrawRect(spriteBatch, pixel, drawX + 66, drawY + 50, 12, 22, armorDark);

            DrawRect(spriteBatch, pixel, drawX + 28, drawY + 74, 14, 18, skinDark);
            DrawRect(spriteBatch, pixel, drawX + 54, drawY + 74, 14, 18, skinDark);

            DrawRect(spriteBatch, pixel, drawX + 24, drawY + 90, 20, 10, new Color(92, 58, 28));
            DrawRect(spriteBatch, pixel, drawX + 50, drawY + 90, 20, 10, new Color(92, 58, 28));

            DrawRect(spriteBatch, pixel, drawX + 72, drawY + 56, 16, 6, weaponWood);
            DrawRect(spriteBatch, pixel, drawX + 86, drawY + 44, 8, 26, metal);
            DrawRect(spriteBatch, pixel, drawX + 84, drawY + 38, 12, 10, metal);
        }

        private void DrawPlayerWeapon(
    SpriteBatch spriteBatch,
    Texture2D pixel,
    int drawX,
    int drawY,
    bool facingLeft,
    string playerState,
    Color blade,
    Color gold,
    WeaponEffectType weaponEffect,
    Color bladeGlow)
        {
            int handY = drawY + 58;

            Color handle = new Color(90, 60, 30);
            Color darkHandle = new Color(60, 40, 25);

            if (facingLeft)
            {
                if (playerState == "Attacking")
                {
                    // Левая атака: меч вынесен влево от персонажа
                    if (weaponEffect != WeaponEffectType.None)
                    {
                        DrawRect(spriteBatch, pixel, drawX - 48, handY - 6, 48, 22, bladeGlow);
                    }

                    // Рука с мечом
                    DrawRect(spriteBatch, pixel, drawX + 14, handY + 2, 12, 7, new Color(118, 74, 34));

                    // Рукоять
                    DrawRect(spriteBatch, pixel, drawX + 2, handY + 4, 14, 5, handle);
                    DrawRect(spriteBatch, pixel, drawX - 3, handY + 1, 5, 12, darkHandle);

                    // Гарда, не щит
                    DrawRect(spriteBatch, pixel, drawX - 8, handY + 2, 5, 10, gold);

                    // Лезвие, длинное и хорошо видимое
                    DrawRect(spriteBatch, pixel, drawX - 48, handY + 3, 40, 7, blade);
                    DrawRect(spriteBatch, pixel, drawX - 53, handY + 5, 5, 3, blade);
                }
                else
                {
                    // Меч в покое слева
                    if (weaponEffect != WeaponEffectType.None)
                    {
                        DrawRect(spriteBatch, pixel, drawX + 0, handY - 12, 20, 36, bladeGlow);
                    }

                    DrawRect(spriteBatch, pixel, drawX + 14, handY + 5, 10, 6, new Color(118, 74, 34));

                    DrawRect(spriteBatch, pixel, drawX + 8, handY + 2, 5, 18, handle);
                    DrawRect(spriteBatch, pixel, drawX + 3, handY + 5, 10, 4, gold);

                    DrawRect(spriteBatch, pixel, drawX + 2, handY - 18, 7, 24, blade);
                    DrawRect(spriteBatch, pixel, drawX + 3, handY - 22, 5, 4, blade);
                }
            }
            else
            {
                if (playerState == "Attacking")
                {
                    // Правая атака: меч вынесен вправо от персонажа
                    if (weaponEffect != WeaponEffectType.None)
                    {
                        DrawRect(spriteBatch, pixel, drawX + 96, handY - 6, 48, 22, bladeGlow);
                    }

                    // Рука с мечом
                    DrawRect(spriteBatch, pixel, drawX + 70, handY + 2, 12, 7, new Color(118, 74, 34));

                    // Рукоять
                    DrawRect(spriteBatch, pixel, drawX + 80, handY + 4, 14, 5, handle);
                    DrawRect(spriteBatch, pixel, drawX + 94, handY + 1, 5, 12, darkHandle);

                    // Гарда, не щит
                    DrawRect(spriteBatch, pixel, drawX + 99, handY + 2, 5, 10, gold);

                    // Лезвие теперь такое же длинное, как слева
                    DrawRect(spriteBatch, pixel, drawX + 104, handY + 3, 40, 7, blade);
                    DrawRect(spriteBatch, pixel, drawX + 144, handY + 5, 5, 3, blade);
                }
                else
                {
                    // Меч в покое справа
                    if (weaponEffect != WeaponEffectType.None)
                    {
                        DrawRect(spriteBatch, pixel, drawX + 76, handY - 12, 20, 36, bladeGlow);
                    }

                    DrawRect(spriteBatch, pixel, drawX + 68, handY + 5, 10, 6, new Color(118, 74, 34));

                    DrawRect(spriteBatch, pixel, drawX + 86, handY + 2, 5, 18, handle);
                    DrawRect(spriteBatch, pixel, drawX + 82, handY + 5, 10, 4, gold);

                    DrawRect(spriteBatch, pixel, drawX + 91, handY - 18, 7, 24, blade);
                    DrawRect(spriteBatch, pixel, drawX + 92, handY - 22, 5, 4, blade);
                }
            }
        }

        private void DrawRect(
            SpriteBatch spriteBatch,
            Texture2D pixel,
            int x,
            int y,
            int width,
            int height,
            Color color)
        {
            spriteBatch.Draw(pixel, new Rectangle(x, y, width, height), color);
        }
    }
}