using GameEngine2D.Biomes;
using GameEngine2D.Bosses;
using GameEngine2D.Cloning;
using GameEngine2D.Commands;
using GameEngine2D.Construction;
using GameEngine2D.Core.Configuration;
using GameEngine2D.Core.World;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;
using GameEngine2D.Facades;
using GameEngine2D.Input;
using GameEngine2D.Observers;
using GameEngine2D.Rendering;
using GameEngine2D.Spawning;
using GameEngine2D.Strategies;
using GameEngine2D.Weapons;
using System.Linq;

namespace GameEngine2D.Core.Engine
{
    public class GameEngine
    {
        private readonly GameRenderer _gameRenderer;
        private readonly OldSpriteRenderer _oldSpriteRenderer;
        private readonly BattleFacade _battleFacade;
        private readonly InputHandler _inputHandler;

        private Level? _currentLevel;
        private Player? _player;
        private IBoss? _boss;
        private IWeapon? _playerWeapon;
        private bool _isRunning;

        public GameEngine()
        {
            _gameRenderer = new GameRenderer();
            _oldSpriteRenderer = new OldSpriteRenderer();
            _battleFacade = new BattleFacade();
            _inputHandler = new InputHandler();
        }

        public void Run()
        {
            LoadConfig();
            InitializeWorld();
            InitializeObservers();
            InitializeInput();
            PrintControls();
            RenderFrame();

            _isRunning = true;
            GameLoop();
        }

        private void InitializeWorld()
        {
            _currentLevel = CreateLevel();

            SpawnCharacters(_currentLevel);
            BuildStructures(_currentLevel);
            CloneEnemies(_currentLevel);

            _player = _currentLevel.Player;
            _playerWeapon = CreatePlayerWeapon();

            if (_player != null)
            {
                _player.EquippedWeapon = _playerWeapon;
            }

            PrepareBossProxy();
        }

        private void InitializeInput()
        {
            if (_player == null)
            {
                Console.WriteLine("Player was not found. Input system was not initialized.");
                return;
            }

            _inputHandler.SetCommand(ConsoleKey.A, new MoveLeftCommand(_player));
            _inputHandler.SetCommand(ConsoleKey.D, new MoveRightCommand(_player));
            _inputHandler.SetCommand(ConsoleKey.W, new JumpCommand(_player));
            _inputHandler.SetCommand(ConsoleKey.Spacebar, new AttackCommand(_player));
        }

        private void GameLoop()
        {
            while (_isRunning)
            {
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Escape:
                        _isRunning = false;
                        break;

                    case ConsoleKey.B:
                        StartBossEncounter();
                        break;

                    default:
                        bool handled = _inputHandler.HandleInput(key);

                        if (!handled)
                        {
                            Console.WriteLine($"No command is assigned to key: {key}");
                        }

                        break;
                }

                if (_isRunning)
                {
                    UpdateWorld();
                    RenderFrame();
                }
            }

            Console.WriteLine();
            Console.WriteLine("Engine stopped.");
        }
        private void UpdateWorld()
        {
            if (_currentLevel == null)
            {
                return;
            }

            foreach (Character character in _currentLevel.Characters)
            {
                character.Update();
            }
        }

        private void PrintHeader()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("        2D PLATFORMER GAME ENGINE       ");
            Console.WriteLine("========================================");
            Console.WriteLine();
        }

        private void PrintControls()
        {
            Console.WriteLine();
            Console.WriteLine("Controls:");
            Console.WriteLine("A - move left");
            Console.WriteLine("D - move right");
            Console.WriteLine("W - jump");
            Console.WriteLine("Space - attack");
            Console.WriteLine("B - start boss encounter");
            Console.WriteLine("Esc - exit");
            Console.WriteLine();
        }

        private void LoadConfig()
        {
            GameConfig.Instance.Print();
            Console.WriteLine();
        }

        private Level CreateLevel()
        {
            IBiomeFactory biomeFactory = new ForestBiomeFactory();
            IBackground background = biomeFactory.CreateBackground();
            IPlatformStyle platformStyle = biomeFactory.CreatePlatformStyle();
            IDecorationStyle decorationStyle = biomeFactory.CreateDecorationStyle();

            Level level = new Level
            {
                Name = "Forest Platformer Level",
                Background = background,
                PlatformStyle = platformStyle,
                DecorationStyle = decorationStyle,
                Width = GameConfig.Instance.MapWidth,
                Height = GameConfig.Instance.MapHeight
            };

            Console.WriteLine("[Abstract Factory] Biome created:");
            Console.WriteLine($"Level: {level.Name}");
            Console.WriteLine($"Background: {background.Name}");
            Console.WriteLine($"Platform Style: {platformStyle.Name}");
            Console.WriteLine($"Decoration Style: {decorationStyle.Name}");
            Console.WriteLine();

            return level;
        }

        private void SpawnCharacters(Level level)
        {
            CharacterFactory playerFactory = new PlayerFactory();
            CharacterFactory npcFactory = new NpcFactory();
            CharacterFactory enemyFactory = new EnemyFactory();

            Character player = playerFactory.SpawnCharacter("Hero");
            Character npc = npcFactory.SpawnCharacter("Old Guide");
            Character enemy = enemyFactory.SpawnCharacter("Wild Goblin");

            if (enemy is Enemy wildGoblin)
            {
                wildGoblin.SetBehaviorStrategy(new AggressiveBehaviorStrategy());
            }

            level.Characters.Add(player);
            level.Characters.Add(npc);
            level.Characters.Add(enemy);

            Console.WriteLine("[Factory Method] Characters created:");
            Console.WriteLine($"- {player.GetInfo()}");
            Console.WriteLine($"- {npc.GetInfo()}");
            Console.WriteLine($"- {enemy.GetInfo()}");
            Console.WriteLine();
        }

        private void InitializeObservers()
        {
            if (_player == null)
            {
                return;
            }

            _player.Attach(new PlayerLogObserver(_player));
            _player.Attach(new PlayerHudObserver(_player));
        }
        private void BuildStructures(Level level)
        {
            StructureDirector director = new StructureDirector();

            HouseBuilder houseBuilder = new HouseBuilder();
            director.Construct(houseBuilder);
            level.Structures.Add(houseBuilder.GetResult());

            BridgeBuilder bridgeBuilder = new BridgeBuilder();
            director.Construct(bridgeBuilder);
            level.Structures.Add(bridgeBuilder.GetResult());

            TrapBuilder trapBuilder = new TrapBuilder();
            director.Construct(trapBuilder);
            level.Structures.Add(trapBuilder.GetResult());

            Console.WriteLine("[Builder] Structures created:");
            Console.WriteLine("- House");
            Console.WriteLine("- Bridge");
            Console.WriteLine("- Trap");
            Console.WriteLine();
        }

        private void CloneEnemies(Level level)
        {
            IEnemyPrototype goblinPrototype = new GoblinPrototype("Goblin Prototype", 50, 10, 24, 8);

            Enemy original = goblinPrototype.Clone();
            original.Name = "Goblin Original";
            original.SetBehaviorStrategy(new PatrolBehaviorStrategy(20, 35));

            Enemy clone1 = goblinPrototype.Clone();
            clone1.Name = "Goblin Clone #1";
            clone1.X = 26;
            clone1.SetBehaviorStrategy(new AggressiveBehaviorStrategy());

            Enemy clone2 = goblinPrototype.Clone();
            clone2.Name = "Goblin Clone #2";
            clone2.X = 32;
            clone2.SetBehaviorStrategy(new IdleBehaviorStrategy());

            level.Characters.Add(original);
            level.Characters.Add(clone1);
            level.Characters.Add(clone2);

            Console.WriteLine("[Prototype + Strategy] Enemies cloned with different behaviors:");
            Console.WriteLine($"- {original.GetInfo()}");
            Console.WriteLine($"- {clone1.GetInfo()}");
            Console.WriteLine($"- {clone2.GetInfo()}");
            Console.WriteLine();
        }

        private IWeapon CreatePlayerWeapon()
        {
            Console.WriteLine("[Bridge + Decorator] Preparing player weapon:");

            IWeapon weapon = new Sword(
                GameConfig.Instance.PlayerWeaponDamage,
                new MeleeAttackImplementor());

            Console.WriteLine("- Base weapon: Sword");
            Console.WriteLine("- Attack implementation: MeleeAttackImplementor");

            weapon = new FireWeapon(
                weapon,
                GameConfig.Instance.FireEffectDamage);

            Console.WriteLine("- Added decorator: FireWeapon");

            if (GameConfig.Instance.UseIceEffect)
            {
                weapon = new IceWeapon(weapon);
                Console.WriteLine("- Added decorator: IceWeapon");
            }

            Console.WriteLine();
            return weapon;
        }

        private void PrepareBossProxy()
        {
            _boss = new BossProxy(
                GameConfig.Instance.BossName,
                GameConfig.Instance.BossHealth,
                GameConfig.Instance.BossDamage);

            Console.WriteLine("[Proxy] Boss is prepared through BossProxy.");
            Console.WriteLine("- RealBoss has not been created yet.");
            Console.WriteLine($"- Preview: {_boss.GetInfo()}");
            Console.WriteLine();
        }

        private void StartBossEncounter()
        {
            if (_boss == null || _playerWeapon == null)
            {
                Console.WriteLine("Boss encounter cannot start because boss or player weapon is missing.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("[Facade] Starting boss encounter...");
            _battleFacade.StartBossBattle(_playerWeapon, _boss);
            Console.WriteLine();
        }

        private void RenderFrame()
        {
            if (_currentLevel == null)
            {
                Console.WriteLine("No level loaded.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("=========== CURRENT FRAME ===========");

            _currentLevel.PrintInfo();
            PrintAsciiMap(_currentLevel);
            RenderLevelWithAdapter(_currentLevel);
        }

        private void RenderLevelWithAdapter(Level level)
        {
            Console.WriteLine("[Adapter] Rendering level objects through unified engine interface:");

            foreach (Character character in level.Characters)
            {
                IGameDrawable drawable = new CharacterRenderAdapter(character, _oldSpriteRenderer);
                _gameRenderer.Render(drawable);
            }

            foreach (Structure structure in level.Structures)
            {
                IGameDrawable drawable = new StructureRenderAdapter(structure, _oldSpriteRenderer);
                _gameRenderer.Render(drawable);
            }

            Console.WriteLine();
        }

        private void PrintAsciiMap(Level level)
        {
            char[,] map = CreateEmptyMap(level.Width, level.Height);

            DrawBorders(map, level.Width, level.Height);
            DrawPlatforms(map, level.Width, level.Height);
            DrawStructures(map, level.Structures, level.Width, level.Height);
            DrawCharacters(map, level.Characters, level.Width, level.Height);

            Console.WriteLine();
            Console.WriteLine("=== ASCII MAP ===");

            for (int y = 0; y < level.Height; y++)
            {
                for (int x = 0; x < level.Width; x++)
                {
                    Console.Write(map[y, x]);
                }

                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("P = Player, N = NPC, E = Enemy, H = House, T = Trap, B = Bridge");
        }

        private char[,] CreateEmptyMap(int width, int height)
        {
            char[,] map = new char[height, width];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    map[y, x] = ' ';
                }
            }

            return map;
        }

        private void DrawBorders(char[,] map, int width, int height)
        {
            for (int x = 0; x < width; x++)
            {
                map[0, x] = '#';
                map[height - 1, x] = '#';
            }

            for (int y = 0; y < height; y++)
            {
                map[y, 0] = '#';
                map[y, width - 1] = '#';
            }
        }

        private void DrawPlatforms(char[,] map, int width, int height)
        {
            for (int x = 15; x <= 20 && x < width - 1; x++)
            {
                if (5 > 0 && 5 < height - 1)
                {
                    map[5, x] = '=';
                }
            }
        }

        private void DrawStructures(char[,] map, List<Structure> structures, int width, int height)
        {
            foreach (Structure structure in structures)
            {
                if (IsInside(structure.X, structure.Y, width, height))
                {
                    map[structure.Y, structure.X] = structure.MapSymbol;
                }
            }
        }

        private void DrawCharacters(char[,] map, List<Character> characters, int width, int height)
        {
            foreach (Character character in characters)
            {
                if (IsInside(character.X, character.Y, width, height))
                {
                    map[character.Y, character.X] = character.MapSymbol;
                }
            }
        }

        private bool IsInside(int x, int y, int width, int height)
        {
            return x > 0 && x < width - 1 && y > 0 && y < height - 1;
        }
    }
}