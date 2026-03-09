using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Cloning
{
    public class SkeletonPrototype : Enemy, IEnemyPrototype
    {
        public SkeletonPrototype(string name, int health, int damage)
            : this(name, health, damage, 30, 8)
        {
        }

        public SkeletonPrototype(string name, int health, int damage, int x, int y)
            : base(name, health, damage, x, y)
        {
        }

        public Enemy Clone()
        {
            return new SkeletonPrototype(Name, Health, Damage, X, Y);
        }
    }
}