namespace GameEngine2D.Weapons
{
    public class IceWeapon : WeaponEffectDecorator
    {
        public IceWeapon(IWeapon component)
            : base(component)
        {
        }

        public override string Use()
        {
            string baseMessage = base.Use();
            return baseMessage + " Ледяной эффект замедляет противника.";
        }

        public override IReadOnlyList<WeaponEffectType> GetEffects()
        {
            return component.GetEffects()
                .Append(WeaponEffectType.Ice)
                .ToList();
        }
    }
}