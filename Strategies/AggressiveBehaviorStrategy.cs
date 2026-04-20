using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Strategies
{
    public class AggressiveBehaviorStrategy : IEnemyBehaviorStrategy
    {
        private readonly int step;

        public string Name => "Aggressive";

        public AggressiveBehaviorStrategy(int step = 2)
        {
            this.step = step;
        }

        public void Execute(Enemy enemy)
        {
            enemy.Move(step, 0);
        }
    }
}