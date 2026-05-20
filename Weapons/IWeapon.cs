namespace GameEngine2D.Weapons
{
    public interface IWeapon
    {
        string Use();
        IReadOnlyList<WeaponEffectType> GetEffects();
    }
}