namespace GameEngine2D.Bosses
{
    public interface IBoss
    {
        void Spawn();
        void Attack();
        string GetInfo();
    }
}