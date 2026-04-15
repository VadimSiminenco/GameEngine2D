using System;

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

        public override void Use()
        {
            base.Use();
            Console.WriteLine($"Огненный эффект наносит дополнительно {fireDamage} урона.");
        }
    }
}