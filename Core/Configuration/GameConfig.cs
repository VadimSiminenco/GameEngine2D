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

        public string BossName { get; private set; }
        public int BossHealth { get; private set; }
        public int BossDamage { get; private set; }
        public int PlayerWeaponDamage { get; private set; }
        public int FireEffectDamage { get; private set; }
        public bool UseIceEffect { get; private set; }

        private GameConfig()
        {
            Difficulty = "Normal";
            MapWidth = 40;
            MapHeight = 12;
            MaxEnemies = 5;
            Volume = 70;

            BossName = "Ancient Dragon";
            BossHealth = 300;
            BossDamage = 50;

            PlayerWeaponDamage = 25;
            FireEffectDamage = 10;
            UseIceEffect = true;
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
            Console.WriteLine($"Boss: {BossName} (HP: {BossHealth}, DMG: {BossDamage})");
            Console.WriteLine($"Player Weapon Damage: {PlayerWeaponDamage}");
            Console.WriteLine($"Fire Effect Damage: {FireEffectDamage}");
            Console.WriteLine($"Use Ice Effect: {UseIceEffect}");
        }
    }
}