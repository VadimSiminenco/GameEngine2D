namespace GameEngine2D.Bosses
{
    public class RealBoss : IBoss
    {
        private readonly string name;

        public int Health { get; private set; }
        public int MaxHealth { get; }
        public int Damage { get; }
        public bool IsDefeated => Health <= 0;

        public RealBoss(string name, int health, int damage)
        {
            this.name = name;
            Health = health;
            MaxHealth = health;
            Damage = damage;
        }

        public string Spawn()
        {
            return $"Boss '{name}' has appeared in the arena.";
        }

        public string Attack()
        {
            if (IsDefeated)
            {
                return $"Boss '{name}' is defeated and cannot attack.";
            }

            return $"Boss '{name}' attacks and deals {Damage} damage.";
        }

        public string TakeDamage(int damage)
        {
            if (IsDefeated)
            {
                return $"Boss '{name}' is already defeated.";
            }

            Health -= damage;

            if (Health < 0)
            {
                Health = 0;
            }

            if (IsDefeated)
            {
                return $"Boss '{name}' took {damage} damage and was defeated.";
            }

            return $"Boss '{name}' took {damage} damage. HP: {Health}/{MaxHealth}.";
        }

        public string GetInfo()
        {
            return $"Boss: {name}, HP: {Health}/{MaxHealth}, DMG: {Damage}";
        }
    }
}