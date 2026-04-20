using GameEngine2D.Strategies;

namespace GameEngine2D.Entities.Characters
{
    public class Enemy : Character
    {
        public int Damage { get; set; }
        public IEnemyBehaviorStrategy BehaviorStrategy { get; private set; }

        public Enemy(string name, int health, int damage, int x, int y)
            : base(name, health, x, y)
        {
            Damage = damage;
            MapSymbol = 'E';
            SpriteKey = "enemy_sprite";
            BehaviorStrategy = new PatrolBehaviorStrategy(20, 35);
        }

        public void SetBehaviorStrategy(IEnemyBehaviorStrategy strategy)
        {
            BehaviorStrategy = strategy;
        }

        protected override void UpdateMainBehavior()
        {
            BehaviorStrategy.Execute(this);
        }

        protected override void UpdateSecondaryBehavior()
        {
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Damage={Damage}, Strategy={BehaviorStrategy.Name}";
        }
    }
}