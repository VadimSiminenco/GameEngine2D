using System;

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

        private RealBoss CreateRealBossIfNeeded()
        {
            if (realSubject == null)
            {
                realSubject = new RealBoss(name, health, damage);
            }

            return realSubject;
        }

        public void Spawn()
        {
            RealBoss boss = CreateRealBossIfNeeded();
            boss.Spawn();
            isSpawned = true;
        }

        public void Attack()
        {
            if (!isSpawned)
            {
                Console.WriteLine($"Boss '{name}' cannot attack yet because it has not spawned.");
                return;
            }

            CreateRealBossIfNeeded().Attack();
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