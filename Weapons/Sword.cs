namespace GameEngine2D.Weapons
{
    public class Sword : Weapon
    {
        public Sword(int damage, IAttackImplementor implementor)
            : base("Меч", damage, implementor)
        {
        }

        public override string Use()
        {
            return implementor.AttackImplementation(name, damage);
        }
    }
}