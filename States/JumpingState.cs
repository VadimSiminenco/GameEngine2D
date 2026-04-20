using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public class JumpingState : IPlayerState
    {
        public string Name => "Jumping";

        public void Handle(Player player)
        {
            player.ExecuteJumpAction();
            player.SetState(new IdleState(), false);
        }
    }
}