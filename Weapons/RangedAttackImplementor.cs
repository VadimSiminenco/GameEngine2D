namespace GameEngine2D.Weapons
{
    public class RangedAttackImplementor : IAttackImplementor
    {
        public string AttackImplementation(string weaponName, int damage)
        {
            return $"{weaponName} наносит {damage} урона на расстоянии.";
        }
    }
}