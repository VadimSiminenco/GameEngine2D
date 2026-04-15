namespace GameEngine2D.Weapons
{
    public abstract class WeaponEffectDecorator : IWeapon
    {
        protected IWeapon component;

        protected WeaponEffectDecorator(IWeapon component)
        {
            this.component = component;
        }

        public virtual void Use()
        {
            component.Use();
        }
    }
}