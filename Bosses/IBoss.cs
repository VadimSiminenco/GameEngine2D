namespace GameEngine2D.Bosses
{
    public interface IBoss
    {
        int Health { get; }
        int MaxHealth { get; }
        int Damage { get; }
        bool IsDefeated { get; }

        string Spawn();
        string Attack();
        string TakeDamage(int damage);
        string GetInfo();
    }
}