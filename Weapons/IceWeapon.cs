using System;

namespace GameEngine2D.Weapons
{
    public class IceWeapon : WeaponEffectDecorator
    {
        public IceWeapon(IWeapon component)
            : base(component)
        {
        }

        public override void Use()
        {
            base.Use();
            AddedBehavior();
        }

        private void AddedBehavior()
        {
            Console.WriteLine("Ледяной эффект замедляет противника.");
        }
    }
}