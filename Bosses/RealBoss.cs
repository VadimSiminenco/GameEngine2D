using System;

namespace GameEngine2D.Bosses
{
    public class RealBoss : IBoss
    {
        private readonly string name;
        private readonly int health;
        private readonly int damage;

        public RealBoss(string name, int health, int damage)
        {
            this.name = name;
            this.health = health;
            this.damage = damage;

            LoadBossData();
        }

        private void LoadBossData()
        {
            Console.WriteLine($"[RealBoss] Loading boss '{name}'...");
        }

        public void Spawn()
        {
            Console.WriteLine($"Boss '{name}' has appeared in the arena.");
        }

        public void Attack()
        {
            Console.WriteLine($"Boss '{name}' attacks and deals {damage} damage.");
        }

        public string GetInfo()
        {
            return $"Boss: {name}, HP: {health}, DMG: {damage}";
        }
    }
}