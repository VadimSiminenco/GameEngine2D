using System;

namespace GameEngine2D.Weapons
{
    public class RangedAttackImplementor : IAttackImplementor
    {
        public void AttackImplementation(string weaponName, int damage)
        {
            Console.WriteLine($"{weaponName} наносит {damage} урона на расстоянии.");
        }
    }
}