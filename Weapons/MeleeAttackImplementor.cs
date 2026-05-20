namespace GameEngine2D.Weapons
{
    public class MeleeAttackImplementor : IAttackImplementor
    {
        public string AttackImplementation(string weaponName, int damage)
        {
            return $"{weaponName} наносит {damage} урона в ближнем бою.";
        }
    }
}