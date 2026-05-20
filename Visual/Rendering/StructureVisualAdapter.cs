using GameEngine2D.Entities.Structures;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine2D.Visual.Rendering
{
    public class StructureVisualAdapter : IVisualRenderable
    {
        private readonly Structure structure;
        private readonly MonoGameSpriteRenderer adaptee;

        public StructureVisualAdapter(Structure structure, MonoGameSpriteRenderer adaptee)
        {
            this.structure = structure;
            this.adaptee = adaptee;
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D pixel, int tileSize)
        {
            int x = structure.X * tileSize;
            int y = structure.Y * tileSize;

            if (structure.Name == "House")
            {
                DrawHouse(spriteBatch, pixel, x, y, tileSize);
                return;
            }

            if (structure.Name == "Bridge")
            {
                DrawBridge(spriteBatch, pixel, x, y, tileSize);
                return;
            }

            if (structure.Name == "Trap")
            {
                DrawTrap(spriteBatch, pixel, x, y, tileSize);
                return;
            }

            adaptee.RenderSprite(
                spriteBatch,
                pixel,
                structure.SpriteKey,
                x,
                y,
                tileSize * 2,
                tileSize);
        }

        private void DrawHouse(SpriteBatch spriteBatch, Texture2D pixel, int x, int y, int tileSize)
        {
            int width = tileSize * 4;
            int visualY = y - tileSize;

            Color wallDark = new Color(135, 65, 42);
            Color wallLight = new Color(170, 85, 55);
            Color roofDark = new Color(110, 25, 25);
            Color roofLight = new Color(160, 40, 35);
            Color wood = new Color(120, 70, 35);
            Color stone = new Color(95, 95, 90);
            Color window = new Color(255, 235, 150);
            Color shadow = new Color(70, 40, 30);

            Rectangle shadowBase = new Rectangle(x - 4, visualY + tileSize * 3 - 4, width + 8, 8);
            spriteBatch.Draw(pixel, shadowBase, new Color(0, 0, 0, 70));

            Rectangle roofBack = new Rectangle(x - 12, visualY - 16, width + 24, 18);
            Rectangle roofMain = new Rectangle(x - 6, visualY - 4, width + 12, 18);

            spriteBatch.Draw(pixel, roofBack, roofDark);
            spriteBatch.Draw(pixel, roofMain, roofLight);

            Rectangle upperWall = new Rectangle(x, visualY + 10, width, tileSize);
            Rectangle lowerWall = new Rectangle(x, visualY + tileSize + 10, width, tileSize);

            spriteBatch.Draw(pixel, upperWall, wallLight);
            spriteBatch.Draw(pixel, lowerWall, wallDark);

            Rectangle foundation = new Rectangle(x, visualY + tileSize * 2 + 10, width, tileSize / 2);
            spriteBatch.Draw(pixel, foundation, stone);

            for (int i = 0; i < 4; i++)
            {
                Rectangle stoneBlock = new Rectangle(x + i * tileSize, visualY + tileSize * 2 + 10, tileSize - 2, tileSize / 2);
                spriteBatch.Draw(pixel, stoneBlock, i % 2 == 0 ? new Color(115, 115, 110) : new Color(85, 85, 82));
            }

            Rectangle leftBeam = new Rectangle(x + 6, visualY + 8, 6, tileSize * 2 + 14);
            Rectangle rightBeam = new Rectangle(x + width - 12, visualY + 8, 6, tileSize * 2 + 14);
            Rectangle middleBeam = new Rectangle(x + width / 2 - 3, visualY + 8, 6, tileSize * 2 + 14);

            spriteBatch.Draw(pixel, leftBeam, wood);
            spriteBatch.Draw(pixel, rightBeam, wood);
            spriteBatch.Draw(pixel, middleBeam, wood);

            Rectangle door = new Rectangle(x + width / 2 - 12, visualY + tileSize + 22, 24, tileSize);
            Rectangle doorShadow = new Rectangle(x + width / 2 - 10, visualY + tileSize + 26, 20, tileSize - 4);

            spriteBatch.Draw(pixel, door, new Color(90, 50, 25));
            spriteBatch.Draw(pixel, doorShadow, new Color(120, 70, 35));

            Rectangle doorHandle = new Rectangle(x + width / 2 + 6, visualY + tileSize + 38, 4, 4);
            spriteBatch.Draw(pixel, doorHandle, new Color(220, 170, 55));

            Rectangle windowLeftOuter = new Rectangle(x + 18, visualY + tileSize + 2, 20, 20);
            Rectangle windowRightOuter = new Rectangle(x + width - 38, visualY + tileSize + 2, 20, 20);

            spriteBatch.Draw(pixel, windowLeftOuter, shadow);
            spriteBatch.Draw(pixel, windowRightOuter, shadow);

            Rectangle windowLeft = new Rectangle(x + 21, visualY + tileSize + 5, 14, 14);
            Rectangle windowRight = new Rectangle(x + width - 35, visualY + tileSize + 5, 14, 14);

            spriteBatch.Draw(pixel, windowLeft, window);
            spriteBatch.Draw(pixel, windowRight, window);

            spriteBatch.Draw(pixel, new Rectangle(windowLeft.X + 6, windowLeft.Y, 2, 14), wood);
            spriteBatch.Draw(pixel, new Rectangle(windowLeft.X, windowLeft.Y + 6, 14, 2), wood);

            spriteBatch.Draw(pixel, new Rectangle(windowRight.X + 6, windowRight.Y, 2, 14), wood);
            spriteBatch.Draw(pixel, new Rectangle(windowRight.X, windowRight.Y + 6, 14, 2), wood);
        }

        private void DrawBridge(SpriteBatch spriteBatch, Texture2D pixel, int x, int y, int tileSize)
        {
            int visualY = y + tileSize / 2;

            Color plankLight = new Color(170, 100, 45);
            Color plankDark = new Color(115, 65, 30);
            Color support = new Color(90, 50, 25);
            Color metal = new Color(95, 95, 95);
            Color shadow = new Color(0, 0, 0, 70);

            int width = tileSize * 3;
            int plankHeight = 10;

            Rectangle bridgeShadow = new Rectangle(x, visualY + 31, width, 8);
            spriteBatch.Draw(pixel, bridgeShadow, shadow);

            Rectangle mainBeam = new Rectangle(x, visualY + 20, width, 10);
            spriteBatch.Draw(pixel, mainBeam, support);

            for (int i = 0; i < 3; i++)
            {
                Rectangle plank = new Rectangle(x + i * tileSize, visualY + 8, tileSize - 2, plankHeight);
                spriteBatch.Draw(pixel, plank, i % 2 == 0 ? plankLight : new Color(150, 85, 38));

                Rectangle plankTop = new Rectangle(plank.X, plank.Y, plank.Width, 2);
                spriteBatch.Draw(pixel, plankTop, new Color(205, 130, 60));
            }

            Rectangle leftSupport = new Rectangle(x + 4, visualY + 28, 8, tileSize - 4);
            Rectangle rightSupport = new Rectangle(x + width - 12, visualY + 28, 8, tileSize - 4);

            spriteBatch.Draw(pixel, leftSupport, support);
            spriteBatch.Draw(pixel, rightSupport, support);

            Rectangle leftBrace = new Rectangle(x + 12, visualY + 32, tileSize, 6);
            Rectangle rightBrace = new Rectangle(x + width - tileSize - 12, visualY + 32, tileSize, 6);

            spriteBatch.Draw(pixel, leftBrace, plankDark);
            spriteBatch.Draw(pixel, rightBrace, plankDark);

            Rectangle bolt1 = new Rectangle(x + 8, visualY + 12, 5, 5);
            Rectangle bolt2 = new Rectangle(x + width - 13, visualY + 12, 5, 5);

            spriteBatch.Draw(pixel, bolt1, metal);
            spriteBatch.Draw(pixel, bolt2, metal);
        }
        private void DrawTrap(SpriteBatch spriteBatch, Texture2D pixel, int x, int y, int tileSize)
        {
            int visualY = y + 4;

            Color baseDark = new Color(55, 55, 60);
            Color baseLight = new Color(90, 90, 95);
            Color wood = new Color(120, 70, 35);
            Color metalDark = new Color(90, 90, 95);
            Color metalLight = new Color(190, 190, 195);
            Color spikeShadow = new Color(70, 70, 75);
            Color warning = new Color(180, 45, 35);

            Rectangle shadow = new Rectangle(x + 2, visualY + 28, tileSize - 4, 5);
            spriteBatch.Draw(pixel, shadow, new Color(0, 0, 0, 80));

            Rectangle stoneBase = new Rectangle(x + 2, visualY + 22, tileSize - 4, 8);
            Rectangle stoneTop = new Rectangle(x + 4, visualY + 18, tileSize - 8, 6);
            Rectangle woodPlate = new Rectangle(x + 6, visualY + 15, tileSize - 12, 5);

            spriteBatch.Draw(pixel, stoneBase, baseDark);
            spriteBatch.Draw(pixel, stoneTop, baseLight);
            spriteBatch.Draw(pixel, woodPlate, wood);

            DrawTrapSpike(spriteBatch, pixel, x + 7, visualY + 5, 7, 14, spikeShadow, metalLight);
            DrawTrapSpike(spriteBatch, pixel, x + 14, visualY + 1, 8, 18, spikeShadow, metalLight);
            DrawTrapSpike(spriteBatch, pixel, x + 22, visualY + 5, 7, 14, spikeShadow, metalLight);

            Rectangle leftBolt = new Rectangle(x + 5, visualY + 23, 4, 4);
            Rectangle rightBolt = new Rectangle(x + tileSize - 9, visualY + 23, 4, 4);

            spriteBatch.Draw(pixel, leftBolt, metalDark);
            spriteBatch.Draw(pixel, rightBolt, metalDark);

            Rectangle warningLine = new Rectangle(x + 8, visualY + 19, tileSize - 16, 2);
            spriteBatch.Draw(pixel, warningLine, warning);
        }
        private void DrawTrapSpike(
    SpriteBatch spriteBatch,
    Texture2D pixel,
    int x,
    int y,
    int width,
    int height,
    Color shadowColor,
    Color lightColor)
        {
            for (int i = 0; i < height; i++)
            {
                int currentWidth = Math.Max(1, width - i / 2);
                int currentX = x + (width - currentWidth) / 2;
                int currentY = y + height - i;

                Color color = i < height / 2
                    ? shadowColor
                    : lightColor;

                spriteBatch.Draw(
                    pixel,
                    new Rectangle(currentX, currentY, currentWidth, 1),
                    color);
            }
        }
    }
}