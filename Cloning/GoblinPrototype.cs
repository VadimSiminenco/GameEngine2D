using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Cloning
{
    public class GoblinPrototype : Enemy, IEnemyPrototype
    {
        public GoblinPrototype(string name, int health, int damage)
            : this(name, health, damage, 24, 8)
        {
        }

        public GoblinPrototype(string name, int health, int damage, int x, int y)
            : base(name, health, damage, x, y)
        {
        }

        public Enemy Clone()
        {
            return new GoblinPrototype(Name, Health, Damage, X, Y);
        }
    }
}