using GameEngine2D.Biomes;
using GameEngine2D.Cloning;
using GameEngine2D.Construction;
using GameEngine2D.Core.Configuration;
using GameEngine2D.Core.World;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;
using GameEngine2D.Spawning;

namespace GameEngine2D.Core.Engine
{
    public class GameEngine
    {
        public void RunDemo()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("        2D PLATFORMER GAME ENGINE       ");
            Console.WriteLine("========================================");
            Console.WriteLine();

            LoadConfig();

            Level level = CreateLevel();

            SpawnCharacters(level);

            BuildStructures(level);

            CloneEnemies(level);

            level.PrintInfo();

            PrintAsciiMap(level);
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
                Background = background.Name,
                PlatformStyle = platformStyle.Name,
                DecorationStyle = decorationStyle.Name,
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

            level.Characters.Add(playerFactory.SpawnCharacter("Hero"));
            level.Characters.Add(npcFactory.SpawnCharacter("Old Guide"));
            level.Characters.Add(enemyFactory.SpawnCharacter("Wild Goblin"));

            Console.WriteLine("[Factory Method] Characters created:");
            Console.WriteLine("- Player: Hero");
            Console.WriteLine("- NPC: Old Guide");
            Console.WriteLine("- Enemy: Wild Goblin");
            Console.WriteLine();
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

            Enemy clone1 = goblinPrototype.Clone();
            clone1.Name = "Goblin Clone #1";
            clone1.X = 26;

            Enemy clone2 = goblinPrototype.Clone();
            clone2.Name = "Goblin Clone #2";
            clone2.X = 32;

            level.Characters.Add(original);
            level.Characters.Add(clone1);
            level.Characters.Add(clone2);

            Console.WriteLine("[Prototype] Enemies cloned:");
            Console.WriteLine("- Goblin Original");
            Console.WriteLine("- Goblin Clone #1");
            Console.WriteLine("- Goblin Clone #2");
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
                char symbol = structure.Name switch
                {
                    "House" => 'H',
                    "Trap" => 'T',
                    "Bridge" => 'B',
                    _ => 'S'
                };

                if (IsInside(structure.X, structure.Y, width, height))
                {
                    map[structure.Y, structure.X] = symbol;
                }
            }
        }

        private void DrawCharacters(char[,] map, List<Character> characters, int width, int height)
        {
            foreach (Character character in characters)
            {
                char symbol = character switch
                {
                    Player => 'P',
                    Npc => 'N',
                    Enemy => 'E',
                    _ => '?'
                };

                if (IsInside(character.X, character.Y, width, height))
                {
                    map[character.Y, character.X] = symbol;
                }
            }
        }

        private bool IsInside(int x, int y, int width, int height)
        {
            return x > 0 && x < width - 1 && y > 0 && y < height - 1;
        }
    }
}