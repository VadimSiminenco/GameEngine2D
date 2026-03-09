namespace GameEngine2D.Core.Configuration
{
    public sealed class GameConfig
    {
        private static GameConfig? _instance;
        private static readonly object _lock = new();

        public string Difficulty { get; private set; }
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int MaxEnemies { get; private set; }
        public int Volume { get; private set; }

        private GameConfig()
        {
            Difficulty = "Normal";
            MapWidth = 40;
            MapHeight = 12;
            MaxEnemies = 5;
            Volume = 70;
        }

        public static GameConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    _instance ??= new GameConfig();
                    return _instance;
                }
            }
        }

        public void Print()
        {
            Console.WriteLine("[Singleton] Game configuration loaded:");
            Console.WriteLine($"Difficulty: {Difficulty}");
            Console.WriteLine($"Map Size: {MapWidth} x {MapHeight}");
            Console.WriteLine($"Max Enemies: {MaxEnemies}");
            Console.WriteLine($"Volume: {Volume}");
        }
    }
}