using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public class IdleState : IPlayerState
    {
        public string Name => "Idle";

        public void Handle(Player player)
        {
        }
    }
}