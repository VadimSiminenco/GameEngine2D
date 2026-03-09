using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Cloning
{
    public class SlimePrototype : Enemy, IEnemyPrototype
    {
        public SlimePrototype(string name, int health, int damage)
            : this(name, health, damage, 28, 8)
        {
        }

        public SlimePrototype(string name, int health, int damage, int x, int y)
            : base(name, health, damage, x, y)
        {
        }

        public Enemy Clone()
        {
            return new SlimePrototype(Name, Health, Damage, X, Y);
        }
    }
}