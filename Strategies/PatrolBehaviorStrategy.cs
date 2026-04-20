using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Strategies
{
    public class PatrolBehaviorStrategy : IEnemyBehaviorStrategy
    {
        private readonly int leftBoundary;
        private readonly int rightBoundary;
        private int direction = 1;

        public string Name => "Patrol";

        public PatrolBehaviorStrategy(int leftBoundary, int rightBoundary)
        {
            this.leftBoundary = leftBoundary;
            this.rightBoundary = rightBoundary;
        }

        public void Execute(Enemy enemy)
        {
            enemy.Move(direction, 0);

            if (enemy.X >= rightBoundary || enemy.X <= leftBoundary)
            {
                direction *= -1;
            }
        }
    }
}