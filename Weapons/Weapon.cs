namespace GameEngine2D.Weapons
{
    public abstract class Weapon : IWeapon
    {
        protected IAttackImplementor implementor;
        protected string name;
        protected int damage;

        protected Weapon(string name, int damage, IAttackImplementor implementor)
        {
            this.name = name;
            this.damage = damage;
            this.implementor = implementor;
        }

        public abstract string Use();

        public virtual IReadOnlyList<WeaponEffectType> GetEffects()
        {
            return Array.Empty<WeaponEffectType>();
        }
    }
}