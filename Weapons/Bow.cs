namespace GameEngine2D.Weapons
{
    public class Bow : Weapon
    {
        public Bow(int damage, IAttackImplementor implementor)
            : base("Лук", damage, implementor)
        {
        }

        public override void Use()
        {
            implementor.AttackImplementation(name, damage);
        }
    }
}