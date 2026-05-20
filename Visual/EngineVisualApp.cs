using GameEngine2D.Biomes;
using GameEngine2D.Core.Scene;
using GameEngine2D.Visual.Animation;
using GameEngine2D.Visual.Rendering;
using GameEngine2D.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Visual
{
    public class EngineVisualApp : Game
    {
        private readonly GraphicsDeviceManager graphics;

        private SpriteBatch? spriteBatch;
        private Texture2D? pixel;
        private EngineScene? scene;
        private MonoGameSpriteRenderer? spriteRenderer;
        private KeyboardState previousKeyboardState;
        private VisualPlayerHudObserver? hudObserver;
        private Player? observedPlayer;
        private SpriteRepository? spriteRepository;
        private CharacterAnimationController? characterAnimationController;
        private CharacterCodeAdaptee? characterAdaptee;

        private MouseState previousMouseState;
        private MenuTool draggedTool = MenuTool.None;
        private bool isEditorVisible = false;

        private const int TileSize = 32;

        private const int MenuX = 1000;
        private const int MenuY = 20;
        private const int MenuWidth = 260;
        private const int MenuButtonHeight = 34;
        private const int WorldRightLimit = MenuX - 10;

        private enum MenuTool
        {
            None,
            ForestBiome,
            DesertBiome,
            CastleBiome,
            Enemy,
            Npc,
            House,
            Bridge,
            Trap
        }

        public EngineVisualApp()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            pixel = new Texture2D(GraphicsDevice, 1, 1);
            pixel.SetData(new[] { Color.White });

            spriteRepository = new SpriteRepository();
            spriteRepository.Load(GraphicsDevice);

            spriteRenderer = new MonoGameSpriteRenderer(spriteRepository);

            characterAnimationController = new CharacterAnimationController();
            characterAdaptee = new CharacterCodeAdaptee();

            scene = new EngineScene();

            hudObserver = new VisualPlayerHudObserver(scene.Player);
            scene.AttachPlayerObserver(hudObserver);
            observedPlayer = scene.Player;
        }

        protected override void Update(GameTime gameTime)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (IsKeyPressed(keyboard, Keys.Tab))
            {
                isEditorVisible = !isEditorVisible;
            }

            if (scene != null)
            {
                float elapsedSeconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (!scene.IsLevelRestarting)
                {
                    if (IsKeyPressed(keyboard, Keys.A) || IsKeyPressed(keyboard, Keys.Left))
                    {
                        scene.MovePlayerLeft();
                    }

                    if (IsKeyPressed(keyboard, Keys.D) || IsKeyPressed(keyboard, Keys.Right))
                    {
                        scene.MovePlayerRight();
                    }

                    if (IsKeyPressed(keyboard, Keys.W) || IsKeyPressed(keyboard, Keys.Up))
                    {
                        scene.MakePlayerJump();
                    }

                    if (IsKeyPressed(keyboard, Keys.Space))
                    {
                        scene.MakePlayerAttack();
                    }

                    if (IsKeyPressed(keyboard, Keys.B))
                    {
                        scene.SpawnBoss();
                    }

                    if (IsKeyPressed(keyboard, Keys.D1))
                    {
                        scene.SetForestBiome();
                    }

                    if (IsKeyPressed(keyboard, Keys.D2))
                    {
                        scene.SetDesertBiome();
                    }

                    if (IsKeyPressed(keyboard, Keys.D3))
                    {
                        scene.SetCastleBiome();
                    }

                    if (IsKeyPressed(keyboard, Keys.N))
                    {
                        scene.SpawnEnemy();
                    }

                    if (IsKeyPressed(keyboard, Keys.H))
                    {
                        scene.BuildHouse();
                    }

                    if (IsKeyPressed(keyboard, Keys.G))
                    {
                        scene.BuildBridge();
                    }

                    if (IsKeyPressed(keyboard, Keys.T))
                    {
                        scene.BuildTrap();
                    }
                    HandleMouseMenu();
                }

                characterAnimationController?.Update(elapsedSeconds);
                scene.Update(elapsedSeconds);

                EnsureHudObserverAttachedToCurrentPlayer();
            }

            previousKeyboardState = keyboard;

            base.Update(gameTime);
        }
        private void EnsureHudObserverAttachedToCurrentPlayer()
        {
            if (scene == null)
            {
                return;
            }

            if (observedPlayer == scene.Player)
            {
                return;
            }

            hudObserver = new VisualPlayerHudObserver(scene.Player);
            scene.AttachPlayerObserver(hudObserver);
            observedPlayer = scene.Player;
        }
        private bool IsKeyPressed(KeyboardState currentKeyboardState, Keys key)
        {
            return currentKeyboardState.IsKeyDown(key)
                && previousKeyboardState.IsKeyUp(key);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (spriteBatch == null || pixel == null || scene == null)
            {
                return;
            }

            spriteBatch.Begin();

            DrawWorldBackground();
            DrawBiomeDecorations();
            DrawGrid();
            DrawStructures();
            DrawWeaponEffect();
            DrawPlayerAttackHitbox();
            DrawCharacters();
            DrawBossPreview();
            DrawHud();

            if (isEditorVisible)
            {
                DrawEditorMenu();
                DrawDraggedToolPreview();
            }
            else
            {
                DrawEditorToggleHint();
            }

            DrawRestartOverlay();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawWorldBackground()
        {
            if (scene == null)
            {
                DrawRectangle(0, 0, 1280, 720, new Color(120, 180, 120));
                return;
            }

            switch (scene.CurrentBackgroundVisualKey)
            {
                case "forest":
                    DrawForestBiomeBackground();
                    break;

                case "desert":
                    DrawDesertBiomeBackground();
                    break;

                case "castle":
                    DrawCastleBiomeBackground();
                    break;

                default:
                    DrawForestBiomeBackground();
                    break;
            }
        }

        private void DrawBiomeDecorations()
        {
            if (scene == null)
            {
                return;
            }

            foreach (BiomeDecoration decoration in scene.CurrentBiomeDecorations)
            {
                DrawBiomeDecoration(decoration);
            }
        }

        private void DrawBiomeDecoration(BiomeDecoration decoration)
        {
            switch (decoration.VisualKey)
            {
                case "forest_tree":
                    DrawBiomeTree(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;

                case "forest_bush":
                    DrawBiomeBush(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;

                case "desert_cactus":
                    DrawBiomeCactus(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;

                case "desert_rock":
                    DrawBiomeRock(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;

                case "castle_column":
                    DrawBiomeColumn(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;

                case "castle_torch":
                    DrawBiomeTorch(decoration.X, decoration.Y, decoration.Width, decoration.Height);
                    break;
            }
        }

        private void DrawBiomeTree(int x, int y, int width, int height)
        {
            int trunkWidth = width / 4;
            int trunkX = x + width / 2 - trunkWidth / 2;
            int trunkY = y + height / 3;

            DrawRectangle(trunkX, trunkY, trunkWidth, height * 2 / 3, new Color(95, 60, 35));

            DrawRectangle(x + width / 5, y + height / 4, width * 3 / 5, height / 3, new Color(55, 130, 60));
            DrawRectangle(x, y + height / 3, width, height / 3, new Color(45, 115, 50));
            DrawRectangle(x + width / 6, y, width * 2 / 3, height / 3, new Color(70, 150, 70));
        }

        private void DrawBiomeBush(int x, int y, int width, int height)
        {
            DrawRectangle(x, y + height / 3, width, height * 2 / 3, new Color(45, 115, 50));
            DrawRectangle(x + width / 8, y, width * 3 / 4, height / 2, new Color(65, 145, 65));
        }

        private void DrawBiomeCactus(int x, int y, int width, int height)
        {
            DrawRectangle(x + width / 3, y, width / 3, height, new Color(55, 130, 80));
            DrawRectangle(x, y + height / 3, width / 3, height / 5, new Color(55, 130, 80));
            DrawRectangle(x + width * 2 / 3, y + height / 2, width / 3, height / 5, new Color(55, 130, 80));
        }

        private void DrawBiomeRock(int x, int y, int width, int height)
        {
            DrawRectangle(x, y + height / 3, width, height * 2 / 3, new Color(120, 105, 85));
            DrawRectangle(x + width / 5, y, width * 3 / 5, height / 2, new Color(150, 135, 110));
        }

        private void DrawBiomeColumn(int x, int y, int width, int height)
        {
            DrawRectangle(x, y, width, height, new Color(90, 90, 100));
            DrawRectangle(x - 6, y, width + 12, 12, new Color(120, 120, 135));
            DrawRectangle(x - 6, y + height - 12, width + 12, 12, new Color(120, 120, 135));

            for (int lineY = y + 25; lineY < y + height - 20; lineY += 30)
            {
                DrawRectangle(x + 5, lineY, width - 10, 3, new Color(70, 70, 80));
            }
        }

        private void DrawBiomeTorch(int x, int y, int width, int height)
        {
            DrawRectangle(x + width / 2 - 3, y + height / 3, 6, height * 2 / 3, new Color(80, 50, 30));
            DrawRectangle(x + width / 2 - 10, y + height / 5, 20, 12, new Color(120, 80, 40));
            DrawRectangle(x + width / 2 - 8, y, 16, height / 3, Color.Orange);
            DrawRectangle(x + width / 2 - 4, y + 5, 8, height / 4, Color.Yellow);
        }

        private void DrawForestBiomeBackground()
        {
            DrawRectangle(0, 0, 1280, 220, new Color(150, 210, 170));
            DrawRectangle(0, 220, 1280, 220, new Color(130, 195, 145));
            DrawRectangle(0, 440, 1280, 280, new Color(110, 180, 125));

            DrawRectangle(0, 310, 1280, 80, new Color(70, 120, 70));
            DrawRectangle(0, 370, 1280, 90, new Color(55, 100, 55));

            int groundY = 18 * TileSize;
            DrawForestGround(groundY);
        }

        private void DrawDesertBiomeBackground()
        {
            DrawRectangle(0, 0, 1280, 250, new Color(235, 205, 140));
            DrawRectangle(0, 250, 1280, 250, new Color(220, 185, 110));
            DrawRectangle(0, 500, 1280, 220, new Color(205, 165, 90));

            DrawRectangle(0, 360, 300, 70, new Color(210, 175, 100));
            DrawRectangle(220, 330, 360, 90, new Color(200, 165, 90));
            DrawRectangle(520, 350, 330, 80, new Color(215, 180, 105));
            DrawRectangle(840, 325, 420, 95, new Color(205, 170, 95));

            int groundY = 18 * TileSize;

            for (int y = groundY; y < 720; y += TileSize)
            {
                for (int x = 0; x < 1280; x += TileSize)
                {
                    Color tileColor = ((x / TileSize) + (y / TileSize)) % 2 == 0
                        ? new Color(185, 145, 75)
                        : new Color(165, 125, 60);

                    DrawRectangle(x, y, TileSize - 1, TileSize - 1, tileColor);
                }
            }
        }

        private void DrawCastleBiomeBackground()
        {
            DrawRectangle(0, 0, 1280, 220, new Color(125, 130, 155));
            DrawRectangle(0, 220, 1280, 220, new Color(105, 110, 135));
            DrawRectangle(0, 440, 1280, 280, new Color(90, 95, 120));

            DrawRectangle(0, 290, 180, 170, new Color(80, 80, 95));
            DrawRectangle(200, 250, 220, 210, new Color(75, 75, 90));
            DrawRectangle(450, 275, 190, 185, new Color(85, 85, 100));
            DrawRectangle(670, 235, 260, 225, new Color(75, 75, 90));
            DrawRectangle(960, 270, 220, 190, new Color(85, 85, 100));

            int groundY = 18 * TileSize;

            for (int y = groundY; y < 720; y += TileSize)
            {
                for (int x = 0; x < 1280; x += TileSize)
                {
                    Color tileColor = ((x / TileSize) + (y / TileSize)) % 2 == 0
                        ? new Color(85, 95, 90)
                        : new Color(70, 80, 75);

                    DrawRectangle(x, y, TileSize - 1, TileSize - 1, tileColor);
                }
            }
        }

        private void DrawForestGround(int groundY)
        {
            for (int y = groundY; y < 720; y += TileSize)
            {
                for (int x = 0; x < 1280; x += TileSize)
                {
                    Color tileColor = ((x / TileSize) + (y / TileSize)) % 2 == 0
                        ? new Color(70, 105, 65)
                        : new Color(55, 85, 50);

                    DrawRectangle(x, y, TileSize - 1, TileSize - 1, tileColor);
                }
            }

            for (int x = 0; x < 1280; x += TileSize)
            {
                DrawRectangle(x, groundY - 5, TileSize - 1, 5, new Color(95, 150, 80));
            }
        }

        private void DrawGrid()
        {
            Color gridColor = new Color(40, 40, 40, 15);

            for (int x = 0; x <= 1280; x += TileSize)
            {
                DrawRectangle(x, 0, 1, 720, gridColor);
            }

            for (int y = 0; y <= 720; y += TileSize)
            {
                DrawRectangle(0, y, 1280, 1, gridColor);
            }
        }

        private void DrawStructures()
        {
            if (scene == null || spriteBatch == null || pixel == null || spriteRenderer == null)
            {
                return;
            }

            foreach (var structure in scene.Structures)
            {
                IVisualRenderable adapter = new StructureVisualAdapter(structure, spriteRenderer);
                adapter.Draw(spriteBatch, pixel, TileSize);
            }
        }

        private void DrawWeaponEffect()
        {
            if (scene == null || hudObserver == null)
            {
                return;
            }

            bool playerIsAttacking = hudObserver.CurrentPlayerState == "Attacking";

            if (!playerIsAttacking)
            {
                return;
            }

            if (scene.HasFireWeaponEffect())
            {
                DrawFireAttackEffect();
                return;
            }

            if (scene.HasIceWeaponEffect())
            {
                DrawIceAttackEffect();
            }
        }

        private void DrawFireAttackEffect()
        {
            if (scene == null)
            {
                return;
            }

            int direction = scene.Player.FacingDirection;
            int baseX = scene.Player.X * TileSize + direction * TileSize;
            int baseY = scene.Player.Y * TileSize - TileSize;

            Color flameDark = new Color(210, 45, 20, 170);
            Color flameMid = new Color(255, 105, 30, 190);
            Color flameLight = new Color(255, 220, 80, 210);

            DrawDirectionalRectangle(baseX, baseY + 28, direction, TileSize + 18, 18, flameDark);
            DrawDirectionalRectangle(baseX + direction * 8, baseY + 18, direction, TileSize + 8, 20, flameMid);
            DrawDirectionalRectangle(baseX + direction * 16, baseY + 24, direction, TileSize / 2, 12, flameLight);

            DrawFlameParticle(baseX + direction * 4, baseY + 8, 8, flameMid);
            DrawFlameParticle(baseX + direction * 22, baseY + 4, 6, flameLight);
            DrawFlameParticle(baseX + direction * 38, baseY + 14, 7, flameDark);
        }

        private void DrawIceAttackEffect()
        {
            if (scene == null)
            {
                return;
            }

            int direction = scene.Player.FacingDirection;
            int baseX = scene.Player.X * TileSize + direction * TileSize;
            int baseY = scene.Player.Y * TileSize - TileSize;

            Color iceDark = new Color(60, 150, 210, 160);
            Color iceMid = new Color(120, 220, 255, 190);
            Color iceLight = new Color(220, 250, 255, 220);

            DrawDirectionalRectangle(baseX, baseY + 28, direction, TileSize + 18, 12, iceDark);
            DrawDirectionalRectangle(baseX + direction * 8, baseY + 18, direction, TileSize + 8, 10, iceMid);
            DrawDirectionalRectangle(baseX + direction * 18, baseY + 10, direction, TileSize / 2, 8, iceLight);

            DrawIceShard(baseX + direction * 4, baseY + 8, 8, 18, iceMid);
            DrawIceShard(baseX + direction * 24, baseY + 2, 7, 22, iceLight);
            DrawIceShard(baseX + direction * 44, baseY + 12, 6, 16, iceDark);
        }

        private void DrawDirectionalRectangle(int x, int y, int direction, int width, int height, Color color)
        {
            if (direction >= 0)
            {
                DrawRectangle(x, y, width, height, color);
            }
            else
            {
                DrawRectangle(x - width, y, width, height, color);
            }
        }

        private void DrawFlameParticle(int x, int y, int size, Color color)
        {
            DrawRectangle(x, y + size / 2, size, size, color);
            DrawRectangle(x + size / 4, y, size / 2, size, color);
        }

        private void DrawIceShard(int x, int y, int width, int height, Color color)
        {
            for (int i = 0; i < height; i++)
            {
                int currentWidth = Math.Max(1, width - i / 3);
                int currentX = x + (width - currentWidth) / 2;
                DrawRectangle(currentX, y + i, currentWidth, 1, color);
            }
        }

        private void DrawPlayerAttackHitbox()
        {
            if (scene == null)
            {
                return;
            }

            if (!scene.IsPlayerAttackVisible)
            {
                return;
            }

            int attackX = (scene.Player.X + scene.Player.FacingDirection) * TileSize;
            int attackY = scene.Player.Y * TileSize;

            DrawRectangle(
                attackX,
                attackY + 6,
                TileSize,
                TileSize - 12,
                new Color(255, 180, 40, 120));
        }

        private void DrawCharacters()
        {
            if (scene == null || spriteBatch == null || pixel == null || characterAdaptee == null)
            {
                return;
            }

            string playerVisualState = hudObserver?.CurrentPlayerState ?? "Idle";
            float animationTime = characterAnimationController?.AnimationTime ?? 0f;

            IVisualRenderable playerAdapter = new CharacterVisualAdapter(
                scene.Player,
                characterAdaptee,
                animationTime,
                playerVisualState,
                scene.IsPlayerHitVisible);

            playerAdapter.Draw(spriteBatch, pixel, TileSize);
            DrawPlayerHitEffect();

            foreach (var npc in scene.Npcs)
            {
                IVisualRenderable npcAdapter = new CharacterVisualAdapter(
                    npc,
                    characterAdaptee,
                    animationTime);

                npcAdapter.Draw(spriteBatch, pixel, TileSize);
            }

            foreach (var enemy in scene.Enemies)
            {
                IVisualRenderable enemyAdapter = new CharacterVisualAdapter(
                    enemy,
                    characterAdaptee,
                    animationTime);

                enemyAdapter.Draw(spriteBatch, pixel, TileSize);

                DrawEnemyHealthBar(enemy);
            }
        }

        private void DrawEnemyHealthBar(GameEngine2D.Entities.Characters.Enemy enemy)
        {
            int barWidth = TileSize * 3;
            int barHeight = 5;

            int x = enemy.X * TileSize - TileSize;
            int y = enemy.Y * TileSize - TileSize * 2 - 10;

            DrawRectangle(x, y, barWidth, barHeight, Color.DarkRed);

            int healthWidth = Math.Max(0, enemy.Health) * barWidth / 60;

            DrawRectangle(x, y, healthWidth, barHeight, Color.LimeGreen);
        }

        private void DrawPlayerHitEffect()
        {
            if (scene == null || !scene.IsPlayerHitVisible)
            {
                return;
            }

            int centerX = scene.Player.X * TileSize + TileSize / 2;
            int centerY = scene.Player.Y * TileSize + TileSize / 2;

            float t = characterAnimationController?.AnimationTime ?? 0f;

            int pulse = (int)(MathF.Sin(t * 28f) * 3f);
            int ringSize = TileSize + 12 + pulse;

            DrawRectangle(centerX - 12, centerY - 18, 24, 24, new Color(255, 240, 180, 170));
            DrawRectangle(centerX - 8, centerY - 14, 16, 16, new Color(255, 120, 70, 180));

            DrawRectangle(centerX - ringSize / 2, centerY - ringSize / 2, ringSize, 4, new Color(255, 80, 60, 180));
            DrawRectangle(centerX - ringSize / 2, centerY + ringSize / 2 - 4, ringSize, 4, new Color(255, 120, 70, 160));
            DrawRectangle(centerX - ringSize / 2, centerY - ringSize / 2, 4, ringSize, new Color(255, 90, 60, 160));
            DrawRectangle(centerX + ringSize / 2 - 4, centerY - ringSize / 2, 4, ringSize, new Color(255, 140, 80, 140));

            DrawSlashEffect(centerX - 26, centerY - 16, 18, 4, new Color(255, 210, 120, 200));
            DrawSlashEffect(centerX + 8, centerY - 6, 16, 4, new Color(255, 160, 70, 200));
            DrawSlashEffect(centerX - 12, centerY + 12, 14, 3, new Color(255, 230, 150, 170));

            DrawParticle(centerX - 20, centerY - 20, 5, new Color(255, 220, 120, 220));
            DrawParticle(centerX + 22, centerY - 12, 4, new Color(255, 150, 70, 220));
            DrawParticle(centerX - 6, centerY + 22, 5, new Color(255, 240, 170, 220));
            DrawParticle(centerX + 14, centerY + 18, 4, new Color(255, 100, 60, 220));
        }

        private void DrawSlashEffect(int x, int y, int width, int height, Color color)
        {
            for (int i = 0; i < width; i++)
            {
                DrawRectangle(x + i, y + i / 2, 1, height, color);
            }
        }

        private void DrawParticle(int x, int y, int size, Color color)
        {
            DrawRectangle(x, y, size, size, color);
            DrawRectangle(x + 1, y - 1, Math.Max(1, size - 2), 1, color);
            DrawRectangle(x + 1, y + size, Math.Max(1, size - 2), 1, color);
        }

        private void DrawBossPreview()
        {
            if (scene == null)
            {
                return;
            }

            int bossX = scene.BossX * TileSize - 60;
            int bossY = scene.BossY * TileSize - 140;

            if (!scene.IsBossSpawned)
            {
                DrawBossShadow(bossX, bossY);
                DrawBossSilhouette(bossX, bossY);
                return;
            }

            DrawBossShadow(bossX, bossY);
            DrawBossBody(bossX, bossY, scene.IsBossAttacking);
            DrawBossHealthBar(bossX, bossY);

            if (scene.IsBossAttacking)
            {
                DrawBossAttackEffect(bossX, bossY);
            }
        }

        private void DrawBossShadow(int x, int y)
        {
            DrawRectangle(x + 10, y + 128, 140, 14, new Color(0, 0, 0, 90));
        }

        private void DrawBossSilhouette(int x, int y)
        {
            Color darkOuter = new Color(60, 25, 30);
            Color darkInner = new Color(95, 45, 50);
            Color glow = new Color(180, 60, 45);

            DrawRectangle(x + 30, y + 20, 95, 110, darkOuter);
            DrawRectangle(x + 45, y + 35, 65, 75, darkInner);

            DrawRectangle(x + 18, y + 45, 18, 55, darkOuter);
            DrawRectangle(x + 120, y + 45, 18, 55, darkOuter);

            DrawRectangle(x + 48, y + 55, 12, 6, glow);
            DrawRectangle(x + 92, y + 55, 12, 6, glow);
        }

        private void DrawBossBody(int x, int y, bool attacking)
        {
            Color stoneDark = attacking
                ? new Color(125, 35, 35)
                : new Color(75, 35, 45);

            Color stoneMid = attacking
                ? new Color(165, 55, 45)
                : new Color(105, 55, 65);

            Color stoneLight = attacking
                ? new Color(215, 95, 55)
                : new Color(135, 80, 85);

            Color gold = new Color(210, 145, 55);
            Color lava = attacking
                ? new Color(255, 120, 30)
                : new Color(220, 65, 35);

            DrawBossLegs(x, y, stoneDark, stoneMid);
            DrawBossTorso(x, y, stoneDark, stoneMid, stoneLight, gold, lava);
            DrawBossHead(x, y, stoneDark, stoneMid, gold, lava);
            DrawBossArms(x, y, stoneDark, stoneMid, gold, lava);
        }
        private void DrawBossHealthBar(int bossX, int bossY)
        {
            if (scene == null || !scene.IsBossSpawned)
            {
                return;
            }

            int barWidth = 150;
            int barHeight = 8;

            int x = bossX + 5;
            int y = bossY - 16;

            DrawRectangle(x, y, barWidth, barHeight, Color.DarkRed);

            int healthWidth = Math.Max(0, scene.Boss.Health) * barWidth / scene.Boss.MaxHealth;

            DrawRectangle(x, y, healthWidth, barHeight, Color.LimeGreen);
        }
        private void DrawBossLegs(int x, int y, Color stoneDark, Color stoneMid)
        {
            DrawRectangle(x + 42, y + 92, 28, 42, stoneDark);
            DrawRectangle(x + 88, y + 92, 28, 42, stoneDark);

            DrawRectangle(x + 34, y + 128, 42, 14, stoneMid);
            DrawRectangle(x + 82, y + 128, 42, 14, stoneMid);

            DrawRectangle(x + 48, y + 106, 16, 5, new Color(45, 25, 30));
            DrawRectangle(x + 94, y + 106, 16, 5, new Color(45, 25, 30));
        }

        private void DrawBossTorso(
            int x,
            int y,
            Color stoneDark,
            Color stoneMid,
            Color stoneLight,
            Color gold,
            Color lava)
        {
            DrawRectangle(x + 35, y + 42, 88, 60, stoneDark);
            DrawRectangle(x + 45, y + 50, 68, 44, stoneMid);

            DrawRectangle(x + 62, y + 60, 34, 28, stoneLight);
            DrawRectangle(x + 70, y + 66, 18, 16, lava);

            DrawRectangle(x + 54, y + 38, 16, 12, gold);
            DrawRectangle(x + 88, y + 38, 16, 12, gold);

            DrawRectangle(x + 58, y + 94, 42, 8, gold);
            DrawRectangle(x + 74, y + 96, 10, 10, new Color(120, 65, 35));
        }

        private void DrawBossHead(int x, int y, Color stoneDark, Color stoneMid, Color gold, Color lava)
        {
            DrawRectangle(x + 50, y + 8, 58, 42, stoneDark);
            DrawRectangle(x + 58, y + 16, 42, 28, stoneMid);

            DrawRectangle(x + 43, y + 0, 18, 16, stoneDark);
            DrawRectangle(x + 97, y + 0, 18, 16, stoneDark);

            DrawRectangle(x + 56, y + 5, 46, 7, gold);

            DrawRectangle(x + 62, y + 28, 12, 6, lava);
            DrawRectangle(x + 86, y + 28, 12, 6, lava);

            DrawRectangle(x + 74, y + 42, 12, 4, new Color(35, 20, 25));
        }

        private void DrawBossArms(int x, int y, Color stoneDark, Color stoneMid, Color gold, Color lava)
        {
            DrawRectangle(x + 10, y + 48, 30, 50, stoneDark);
            DrawRectangle(x + 118, y + 48, 30, 50, stoneDark);

            DrawRectangle(x + 0, y + 86, 42, 36, stoneMid);
            DrawRectangle(x + 116, y + 86, 42, 36, stoneMid);

            DrawRectangle(x + 8, y + 94, 18, 8, lava);
            DrawRectangle(x + 132, y + 94, 18, 8, lava);

            DrawRectangle(x + 18, y + 52, 20, 7, gold);
            DrawRectangle(x + 120, y + 52, 20, 7, gold);
        }

        private void DrawBossAttackEffect(int x, int y)
        {
            DrawRectangle(x - 115, y + 58, 115, 28, new Color(255, 120, 20, 130));
            DrawRectangle(x - 95, y + 48, 80, 48, new Color(255, 170, 40, 90));
            DrawRectangle(x - 65, y + 64, 50, 16, new Color(255, 230, 80, 160));

            DrawRectangle(x - 128, y + 50, 18, 10, new Color(255, 190, 40, 160));
            DrawRectangle(x - 145, y + 72, 24, 8, new Color(255, 90, 20, 130));
        }
        private void HandleMouseMenu()
        {
            if (scene == null || !isEditorVisible)
            {
                return;
            }

            MouseState mouse = Mouse.GetState();

            bool leftPressed = mouse.LeftButton == ButtonState.Pressed
                && previousMouseState.LeftButton == ButtonState.Released;

            bool leftReleased = mouse.LeftButton == ButtonState.Released
                && previousMouseState.LeftButton == ButtonState.Pressed;

            if (leftPressed)
            {
                draggedTool = GetToolFromMenu(mouse.X, mouse.Y);

                if (IsPointInside(mouse.X, mouse.Y, MenuX + 16, 514, 104, 34))
                {
                    scene.EquipBasicSword();
                    draggedTool = MenuTool.None;
                }
                else if (IsPointInside(mouse.X, mouse.Y, MenuX + 136, 514, 104, 34))
                {
                    scene.ApplyFireEffectToSword();
                    draggedTool = MenuTool.None;
                }
                else if (IsPointInside(mouse.X, mouse.Y, MenuX + 16, 554, 104, 34))
                {
                    scene.ApplyIceEffectToSword();
                    draggedTool = MenuTool.None;
                }
                else if (IsPointInside(mouse.X, mouse.Y, MenuX + 16, 606, 224, 42))
                {
                    scene.SpawnBoss();
                    draggedTool = MenuTool.None;
                }
            }

            if (leftReleased)
            {
                if (draggedTool != MenuTool.None && mouse.X < WorldRightLimit)
                {
                    ApplyDraggedToolToWorld(draggedTool, mouse.X, mouse.Y);
                }

                draggedTool = MenuTool.None;
            }

            previousMouseState = mouse;
        }

        private MenuTool GetToolFromMenu(int mouseX, int mouseY)
        {
            if (IsPointInside(mouseX, mouseY, MenuX + 16, 96, 224, MenuButtonHeight))
            {
                return MenuTool.ForestBiome;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 16, 136, 224, MenuButtonHeight))
            {
                return MenuTool.DesertBiome;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 16, 176, 224, MenuButtonHeight))
            {
                return MenuTool.CastleBiome;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 16, 260, 104, MenuButtonHeight))
            {
                return MenuTool.Enemy;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 136, 260, 104, MenuButtonHeight))
            {
                return MenuTool.Npc;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 16, 358, 104, MenuButtonHeight))
            {
                return MenuTool.House;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 136, 358, 104, MenuButtonHeight))
            {
                return MenuTool.Bridge;
            }

            if (IsPointInside(mouseX, mouseY, MenuX + 16, 398, 104, MenuButtonHeight))
            {
                return MenuTool.Trap;
            }

            return MenuTool.None;
        }

        private void ApplyDraggedToolToWorld(MenuTool tool, int mouseX, int mouseY)
        {
            if (scene == null)
            {
                return;
            }

            int tileX = Math.Clamp(mouseX / TileSize, 0, 30);
            int tileY = Math.Clamp(mouseY / TileSize, 0, 18);

            switch (tool)
            {
                case MenuTool.ForestBiome:
                    scene.SetForestBiome();
                    break;

                case MenuTool.DesertBiome:
                    scene.SetDesertBiome();
                    break;

                case MenuTool.CastleBiome:
                    scene.SetCastleBiome();
                    break;

                case MenuTool.Enemy:
                    scene.SpawnEnemyAt(tileX, tileY);
                    break;

                case MenuTool.Npc:
                    scene.SpawnNpcAt(tileX, tileY);
                    break;

                case MenuTool.House:
                    scene.BuildHouseAt(tileX);
                    break;

                case MenuTool.Bridge:
                    scene.BuildBridgeAt(tileX);
                    break;

                case MenuTool.Trap:
                    scene.BuildTrapAt(tileX);
                    break;
            }
        }

        private bool IsPointInside(int pointX, int pointY, int x, int y, int width, int height)
        {
            return pointX >= x
                && pointX <= x + width
                && pointY >= y
                && pointY <= y + height;
        }

        private void DrawEditorMenu()
        {
            if (scene == null)
            {
                return;
            }

            DrawRectangle(MenuX, MenuY, MenuWidth, 680, new Color(24, 22, 28, 235));
            DrawRectangle(MenuX + 4, MenuY + 4, MenuWidth - 8, 672, new Color(42, 36, 44, 245));
            DrawRectangle(MenuX + 12, MenuY + 12, MenuWidth - 24, 4, new Color(210, 130, 55));

            DrawPixelText("WORLD EDITOR", MenuX + 24, MenuY + 28, 2, new Color(240, 210, 150));

            DrawPixelText("BIOMES", MenuX + 20, 78, 1, new Color(210, 190, 150));
            DrawMenuButton(MenuX + 16, 96, 224, MenuButtonHeight, "FOREST", new Color(65, 135, 80));
            DrawMenuButton(MenuX + 16, 136, 224, MenuButtonHeight, "DESERT", new Color(180, 135, 70));
            DrawMenuButton(MenuX + 16, 176, 224, MenuButtonHeight, "CASTLE", new Color(100, 100, 130));

            DrawPixelText("CHARACTERS", MenuX + 20, 242, 1, new Color(210, 190, 150));
            DrawMenuButton(MenuX + 16, 260, 104, MenuButtonHeight, "ENEMY", new Color(145, 65, 65));
            DrawMenuButton(MenuX + 136, 260, 104, MenuButtonHeight, "NPC", new Color(70, 120, 180));

            DrawPixelText("OBJECTS", MenuX + 20, 340, 1, new Color(210, 190, 150));
            DrawMenuButton(MenuX + 16, 358, 104, MenuButtonHeight, "HOUSE", new Color(130, 90, 60));
            DrawMenuButton(MenuX + 136, 358, 104, MenuButtonHeight, "BRIDGE", new Color(120, 100, 75));
            DrawMenuButton(MenuX + 16, 398, 104, MenuButtonHeight, "TRAP", new Color(130, 55, 55));

            DrawPixelText("WEAPON", MenuX + 20, 496, 1, new Color(210, 190, 150));
            DrawMenuButton(MenuX + 16, 514, 104, MenuButtonHeight, "SWORD", new Color(125, 90, 55));
            DrawMenuButton(MenuX + 136, 514, 104, MenuButtonHeight, "FIRE", new Color(205, 85, 45));
            DrawMenuButton(MenuX + 16, 554, 104, MenuButtonHeight, "ICE", new Color(75, 160, 210));

            DrawMenuButton(MenuX + 16, 606, 224, 42, "AWAKEN BOSS", new Color(155, 50, 45));

            DrawRectangle(MenuX + 16, 660, 224, 34, new Color(28, 25, 30));
            DrawPixelText(CleanPixelText(scene.LastEditorMessage), MenuX + 24, 671, 1, new Color(235, 200, 120));
        }

        private void DrawMenuButton(int x, int y, int width, int height, string text, Color color)
        {
            DrawRectangle(x, y, width, height, new Color(18, 16, 20));
            DrawRectangle(x + 2, y + 2, width - 4, height - 4, color);
            DrawRectangle(x + 2, y + 2, width - 4, 4, new Color(255, 255, 255, 60));

            DrawPixelText(text, x + 10, y + 11, 1, Color.White);
        }

        private void DrawDraggedToolPreview()
        {
            if (draggedTool == MenuTool.None)
            {
                return;
            }

            MouseState mouse = Mouse.GetState();

            Color color = draggedTool switch
            {
                MenuTool.ForestBiome => new Color(65, 135, 80, 180),
                MenuTool.DesertBiome => new Color(180, 135, 70, 180),
                MenuTool.CastleBiome => new Color(100, 100, 130, 180),
                MenuTool.Enemy => new Color(145, 65, 65, 180),
                MenuTool.Npc => new Color(70, 120, 180, 180),
                MenuTool.House => new Color(130, 90, 60, 180),
                MenuTool.Bridge => new Color(120, 100, 75, 180),
                MenuTool.Trap => new Color(130, 55, 55, 180),
                _ => Color.White
            };

            DrawRectangle(mouse.X - 16, mouse.Y - 16, 32, 32, color);
            DrawRectangle(mouse.X - 20, mouse.Y - 20, 40, 3, Color.White);
            DrawRectangle(mouse.X - 20, mouse.Y + 17, 40, 3, Color.White);
            DrawRectangle(mouse.X - 20, mouse.Y - 20, 3, 40, Color.White);
            DrawRectangle(mouse.X + 17, mouse.Y - 20, 3, 40, Color.White);
        }

        private string CleanPixelText(string text)
        {
            return text
                .Replace("+", " ")
                .Replace(".", " ")
                .Replace(",", " ")
                .Replace(":", " ");
        }
        private void DrawEditorToggleHint()
        {
            DrawRectangle(1030, 20, 220, 34, new Color(24, 22, 28, 210));
            DrawRectangle(1034, 24, 212, 26, new Color(42, 36, 44, 230));

            DrawPixelText("TAB EDITOR", 1050, 33, 1, new Color(235, 200, 120));
        }
        private void DrawHud()
        {
            if (scene == null)
            {
                return;
            }

            int hudX = 20;
            int hudY = 20;
            int hudWidth = 300;
            int hudHeight = 168;

            Color panelDark = new Color(28, 24, 28, 220);
            Color panelBorder = new Color(105, 80, 55);
            Color panelHighlight = new Color(170, 120, 60);

            DrawRectangle(hudX, hudY, hudWidth, hudHeight, panelBorder);
            DrawRectangle(hudX + 4, hudY + 4, hudWidth - 8, hudHeight - 8, panelDark);
            DrawRectangle(hudX + 8, hudY + 8, hudWidth - 16, 3, panelHighlight);

            DrawPixelText("PLAYER", hudX + 18, hudY + 20, 2, new Color(235, 205, 140));

            DrawPixelText("HEALTH", hudX + 18, hudY + 46, 1, new Color(210, 190, 150));

            int healthBarX = hudX + 90;
            int healthBarY = hudY + 43;
            int healthBarWidth = 180;
            int healthBarHeight = 14;

            DrawRectangle(
                healthBarX - 2,
                healthBarY - 2,
                healthBarWidth + 4,
                healthBarHeight + 4,
                new Color(70, 45, 45));

            DrawRectangle(
                healthBarX,
                healthBarY,
                healthBarWidth,
                healthBarHeight,
                new Color(55, 25, 30));

            int healthWidth = Math.Max(0, scene.Player.Health) * healthBarWidth / 100;

            Color healthColor = new Color(80, 210, 95);

            if (scene.IsPlayerHitVisible)
            {
                healthColor = new Color(255, 70, 55);
            }
            else if (scene.IsPlayerOnTrap)
            {
                healthColor = new Color(235, 80, 55);
            }

            DrawRectangle(
                healthBarX,
                healthBarY,
                healthWidth,
                healthBarHeight,
                healthColor);

            DrawRectangle(
                healthBarX,
                healthBarY,
                healthWidth,
                3,
                new Color(255, 255, 255, 70));

            string state = hudObserver?.CurrentPlayerState ?? "Idle";
            string stateText = state.ToUpper();

            DrawPixelText("STATE", hudX + 18, hudY + 78, 1, new Color(210, 190, 150));

            Color stateColor = scene.IsPlayerHitVisible
                ? new Color(255, 80, 65)
                : state switch
                {
                    "Moving Left" => new Color(120, 190, 255),
                    "Moving Right" => new Color(120, 190, 255),
                    "Jumping" => new Color(190, 130, 255),
                    "Attacking" => new Color(255, 150, 55),
                    _ => new Color(180, 180, 180)
                };

            DrawRectangle(
                hudX + 90,
                hudY + 73,
                180,
                24,
                new Color(45, 40, 45));

            DrawRectangle(
                hudX + 92,
                hudY + 75,
                176,
                20,
                scene.IsPlayerHitVisible
                    ? new Color(90, 30, 35)
                    : new Color(35, 35, 40));

            if (scene.IsPlayerHitVisible)
            {
                DrawRectangle(
                    hudX + 86,
                    hudY + 69,
                    188,
                    32,
                    new Color(255, 40, 30, 60));
            }

            DrawPixelText(stateText, hudX + 100, hudY + 80, 1, stateColor);

            if (scene.IsTrapActivated)
            {
                DrawPixelText("TRAP HIT", hudX + 18, hudY + 112, 1, new Color(255, 95, 70));
            }
            else if (scene.IsPlayerHitVisible)
            {
                DrawPixelText("DAMAGE", hudX + 18, hudY + 112, 1, new Color(255, 95, 70));
            }

            DrawPixelText("EQUIPPED", hudX + 18, hudY + 136, 1, new Color(210, 190, 150));

            string equippedText = GetEquippedWeaponText();

            Color equippedColor = equippedText switch
            {
                "FIRE SWORD" => new Color(255, 130, 55),
                "ICE SWORD" => new Color(120, 220, 255),
                _ => new Color(220, 220, 220)
            };

            DrawPixelText(equippedText, hudX + 110, hudY + 136, 1, equippedColor);
        }

        private string GetEquippedWeaponText()
        {
            if (scene == null || scene.Player.EquippedWeapon == null)
            {
                return "NONE";
            }

            IReadOnlyList<WeaponEffectType> effects = scene.Player.EquippedWeapon.GetEffects();

            if (effects.Contains(WeaponEffectType.Fire))
            {
                return "FIRE SWORD";
            }

            if (effects.Contains(WeaponEffectType.Ice))
            {
                return "ICE SWORD";
            }
            return "SWORD";
        }
        private void DrawRectangle(int x, int y, int width, int height, Color color)
        {
            if (spriteBatch == null || pixel == null)
            {
                return;
            }

            spriteBatch.Draw(
                pixel,
                new Rectangle(x, y, width, height),
                color);
        }
        private void DrawRestartOverlay()
        {
            if (scene == null || !scene.IsLevelRestarting)
            {
                return;
            }

            DrawRectangle(0, 0, 1280, 720, new Color(0, 0, 0, 150));

            int centerX = 1280 / 2;
            int centerY = 720 / 2;

            DrawRectangle(centerX - 230, centerY - 90, 460, 180, new Color(95, 55, 45));
            DrawRectangle(centerX - 222, centerY - 82, 444, 164, new Color(35, 25, 28, 235));
            DrawRectangle(centerX - 210, centerY - 70, 420, 5, new Color(210, 90, 55));

            DrawPixelText("YOU DIED", centerX - 145, centerY - 42, 4, new Color(255, 90, 70));
            DrawPixelText("RESTARTING LEVEL", centerX - 140, centerY + 26, 2, new Color(235, 200, 135));

            DrawRectangle(centerX - 120, centerY + 62, 240, 10, new Color(90, 45, 45));
            DrawRectangle(centerX - 120, centerY + 62, 180, 10, new Color(255, 120, 70));
        }
        private void DrawPixelText(string text, int x, int y, int scale, Color color)
        {
            text = text.ToUpper();

            int cursorX = x;

            foreach (char character in text)
            {
                if (character == ' ')
                {
                    cursorX += 4 * scale;
                    continue;
                }

                DrawPixelCharacter(character, cursorX, y, scale, color);
                cursorX += 6 * scale;
            }
        }

        private void DrawPixelCharacter(char character, int x, int y, int scale, Color color)
        {
            string[] rows = GetPixelCharacterRows(character);

            for (int row = 0; row < rows.Length; row++)
            {
                for (int column = 0; column < rows[row].Length; column++)
                {
                    if (rows[row][column] == '1')
                    {
                        DrawRectangle(
                            x + column * scale,
                            y + row * scale,
                            scale,
                            scale,
                            color);
                    }
                }
            }
        }

        private string[] GetPixelCharacterRows(char character)
        {
            return character switch
            {
                'A' => new[] { "01110", "10001", "10001", "11111", "10001", "10001", "10001" },
                'B' => new[] { "11110", "10001", "10001", "11110", "10001", "10001", "11110" },
                'C' => new[] { "01111", "10000", "10000", "10000", "10000", "10000", "01111" },
                'D' => new[] { "11110", "10001", "10001", "10001", "10001", "10001", "11110" },
                'E' => new[] { "11111", "10000", "10000", "11110", "10000", "10000", "11111" },
                'F' => new[] { "11111", "10000", "10000", "11110", "10000", "10000", "10000" },
                'G' => new[] { "01111", "10000", "10000", "10011", "10001", "10001", "01111" },
                'H' => new[] { "10001", "10001", "10001", "11111", "10001", "10001", "10001" },
                'I' => new[] { "11111", "00100", "00100", "00100", "00100", "00100", "11111" },
                'J' => new[] { "00111", "00010", "00010", "00010", "10010", "10010", "01100" },
                'K' => new[] { "10001", "10010", "10100", "11000", "10100", "10010", "10001" },
                'L' => new[] { "10000", "10000", "10000", "10000", "10000", "10000", "11111" },
                'M' => new[] { "10001", "11011", "10101", "10101", "10001", "10001", "10001" },
                'N' => new[] { "10001", "11001", "10101", "10011", "10001", "10001", "10001" },
                'O' => new[] { "01110", "10001", "10001", "10001", "10001", "10001", "01110" },
                'P' => new[] { "11110", "10001", "10001", "11110", "10000", "10000", "10000" },
                'Q' => new[] { "01110", "10001", "10001", "10001", "10101", "10010", "01101" },
                'R' => new[] { "11110", "10001", "10001", "11110", "10100", "10010", "10001" },
                'S' => new[] { "01111", "10000", "10000", "01110", "00001", "00001", "11110" },
                'T' => new[] { "11111", "00100", "00100", "00100", "00100", "00100", "00100" },
                'U' => new[] { "10001", "10001", "10001", "10001", "10001", "10001", "01110" },
                'V' => new[] { "10001", "10001", "10001", "10001", "10001", "01010", "00100" },
                'W' => new[] { "10001", "10001", "10001", "10101", "10101", "10101", "01010" },
                'X' => new[] { "10001", "10001", "01010", "00100", "01010", "10001", "10001" },
                'Y' => new[] { "10001", "10001", "01010", "00100", "00100", "00100", "00100" },
                'Z' => new[] { "11111", "00001", "00010", "00100", "01000", "10000", "11111" },

                '0' => new[] { "01110", "10001", "10011", "10101", "11001", "10001", "01110" },
                '1' => new[] { "00100", "01100", "00100", "00100", "00100", "00100", "01110" },
                '2' => new[] { "01110", "10001", "00001", "00010", "00100", "01000", "11111" },
                '3' => new[] { "11110", "00001", "00001", "01110", "00001", "00001", "11110" },
                '4' => new[] { "00010", "00110", "01010", "10010", "11111", "00010", "00010" },
                '5' => new[] { "11111", "10000", "10000", "11110", "00001", "00001", "11110" },
                '6' => new[] { "01110", "10000", "10000", "11110", "10001", "10001", "01110" },
                '7' => new[] { "11111", "00001", "00010", "00100", "01000", "01000", "01000" },
                '8' => new[] { "01110", "10001", "10001", "01110", "10001", "10001", "01110" },
                '9' => new[] { "01110", "10001", "10001", "01111", "00001", "00001", "01110" },

                '-' => new[] { "00000", "00000", "00000", "11111", "00000", "00000", "00000" },
                ':' => new[] { "00000", "00100", "00100", "00000", "00100", "00100", "00000" },
                '/' => new[] { "00001", "00010", "00010", "00100", "01000", "01000", "10000" },

                _ => new[] { "00000", "00000", "00000", "00000", "00000", "00000", "00000" }
            };
        }
    }
}