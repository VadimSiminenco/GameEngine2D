using GameEngine2D.Biomes;
using GameEngine2D.Cloning;
using GameEngine2D.Construction;
using GameEngine2D.Core.Configuration;
using GameEngine2D.Entities.Characters;
using GameEngine2D.Entities.Structures;
using GameEngine2D.Spawning;

namespace GameEngine2D.Tests
{
    public static class EngineTests
    {
        public static void RunAllTests()
        {
            Console.WriteLine();
            Console.WriteLine("===== ENGINE TESTS =====");

            TestSingleton();
            TestFactoryMethod();
            TestAbstractFactory();
            TestBuilder();
            TestPrototype();
            TestMapCoordinates();

            Console.WriteLine("===== TESTS FINISHED =====");
            Console.WriteLine();
        }

        private static void TestSingleton()
        {
            var a = GameConfig.Instance;
            var b = GameConfig.Instance;

            if (ReferenceEquals(a, b))
                PrintSuccess("Singleton");
            else
                PrintFail("Singleton");
        }

        private static void TestFactoryMethod()
        {
            CharacterFactory playerFactory = new PlayerFactory();
            CharacterFactory npcFactory = new NpcFactory();
            CharacterFactory enemyFactory = new EnemyFactory();

            var p = playerFactory.SpawnCharacter("Hero");
            var n = npcFactory.SpawnCharacter("Guide");
            var e = enemyFactory.SpawnCharacter("Goblin");

            if (p is Player && n is Npc && e is Enemy)
                PrintSuccess("Factory Method");
            else
                PrintFail("Factory Method");
        }

        private static void TestAbstractFactory()
        {
            IBiomeFactory factory = new ForestBiomeFactory();

            var bg = factory.CreateBackground();
            var plat = factory.CreatePlatformStyle();
            var deco = factory.CreateDecorationStyle();

            if (bg is ForestBackground &&
                plat is ForestPlatformStyle &&
                deco is ForestDecorationStyle)
            {
                PrintSuccess("Abstract Factory");
            }
            else
            {
                PrintFail("Abstract Factory");
            }
        }

        private static void TestBuilder()
        {
            StructureDirector director = new StructureDirector();
            HouseBuilder builder = new HouseBuilder();

            director.Construct(builder);
            Structure house = builder.GetResult();

            if (house.Name == "House" &&
                house.BasePart == "Stone Foundation" &&
                house.MiddlePart == "Wooden Walls" &&
                house.TopPart == "Tile Roof")
            {
                PrintSuccess("Builder");
            }
            else
            {
                PrintFail("Builder");
            }
        }

        private static void TestPrototype()
        {
            GoblinPrototype proto = new GoblinPrototype("Goblin", 50, 10, 24, 8);
            Enemy clone = proto.Clone();

            if (!ReferenceEquals(proto, clone) &&
                clone.Name == proto.Name &&
                clone.Health == proto.Health &&
                clone.Damage == proto.Damage &&
                clone.X == proto.X &&
                clone.Y == proto.Y)
            {
                PrintSuccess("Prototype");
            }
            else
            {
                PrintFail("Prototype");
            }
        }

        private static void TestMapCoordinates()
        {
            Player p = new Player("TestPlayer");

            if (p.X >= 0 && p.Y >= 0)
                PrintSuccess("Map coordinates");
            else
                PrintFail("Map coordinates");
        }

        private static void PrintSuccess(string test)
        {
            Console.WriteLine($"✔ {test} OK");
        }

        private static void PrintFail(string test)
        {
            Console.WriteLine($"✘ {test} FAILED");
        }
    }
}