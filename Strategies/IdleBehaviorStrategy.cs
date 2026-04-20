using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Strategies
{
    public class IdleBehaviorStrategy : IEnemyBehaviorStrategy
    {
        public string Name => "Idle";

        public void Execute(Enemy enemy)
        {
        }
    }
}