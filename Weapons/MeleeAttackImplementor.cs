using System;

namespace GameEngine2D.Weapons
{
    public class MeleeAttackImplementor : IAttackImplementor
    {
        public void AttackImplementation(string weaponName, int damage)
        {
            Console.WriteLine($"{weaponName} наносит {damage} урона в ближнем бою.");
        }
    }
}