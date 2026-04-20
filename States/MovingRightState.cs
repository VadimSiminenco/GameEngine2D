using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public class MovingRightState : IPlayerState
    {
        public string Name => "Moving Right";

        public void Handle(Player player)
        {
            player.ExecuteMoveRightAction();
            player.SetState(new IdleState(), false);
        }
    }
}