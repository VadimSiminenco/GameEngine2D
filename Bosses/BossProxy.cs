namespace GameEngine2D.Bosses
{
    public class BossProxy : IBoss
    {
        private readonly string name;
        private readonly int health;
        private readonly int damage;

        private RealBoss? realSubject;
        private bool isSpawned;

        public BossProxy(string name, int health, int damage)
        {
            this.name = name;
            this.health = health;
            this.damage = damage;
            isSpawned = false;
        }

        public int Health => realSubject?.Health ?? health;
        public int MaxHealth => realSubject?.MaxHealth ?? health;
        public int Damage => damage;
        public bool IsDefeated => realSubject?.IsDefeated ?? false;

        private RealBoss CreateRealBossIfNeeded()
        {
            if (realSubject == null)
            {
                realSubject = new RealBoss(name, health, damage);
            }

            return realSubject;
        }

        public string Spawn()
        {
            RealBoss boss = CreateRealBossIfNeeded();
            isSpawned = true;
            return boss.Spawn();
        }

        public string Attack()
        {
            if (!isSpawned)
            {
                return $"Boss '{name}' cannot attack yet because it has not spawned.";
            }

            return CreateRealBossIfNeeded().Attack();
        }

        public string TakeDamage(int damage)
        {
            if (!isSpawned)
            {
                return $"Boss '{name}' cannot be damaged before spawn.";
            }

            return CreateRealBossIfNeeded().TakeDamage(damage);
        }

        public string GetInfo()
        {
            if (!isSpawned)
            {
                return $"Boss preview: {name} (hidden enemy, HP: ?, DMG: ?)";
            }

            return CreateRealBossIfNeeded().GetInfo();
        }
    }
}