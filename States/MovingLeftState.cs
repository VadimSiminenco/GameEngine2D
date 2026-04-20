using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public class MovingLeftState : IPlayerState
    {
        public string Name => "Moving Left";

        public void Handle(Player player)
        {
            player.ExecuteMoveLeftAction();
            player.SetState(new IdleState(), false);
        }
    }
}