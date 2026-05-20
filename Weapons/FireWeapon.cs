namespace GameEngine2D.Weapons
{
    public class FireWeapon : WeaponEffectDecorator
    {
        private readonly int fireDamage;

        public FireWeapon(IWeapon component, int fireDamage)
            : base(component)
        {
            this.fireDamage = fireDamage;
        }

        public override string Use()
        {
            string baseMessage = base.Use();
            return baseMessage + $" Огненный эффект наносит дополнительно {fireDamage} урона.";
        }

        public override IReadOnlyList<WeaponEffectType> GetEffects()
        {
            return component.GetEffects()
                .Append(WeaponEffectType.Fire)
                .ToList();
        }
    }
}