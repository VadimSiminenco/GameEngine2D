using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public interface IPlayerState
    {
        string Name { get; }
        void Handle(Player player);
    }
}