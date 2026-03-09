using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Cloning
{
    public interface IEnemyPrototype
    {
        Enemy Clone();
    }
}