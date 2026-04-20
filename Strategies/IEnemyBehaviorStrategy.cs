using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Strategies
{
    public interface IEnemyBehaviorStrategy
    {
        string Name { get; }
        void Execute(Enemy enemy);
    }
}