using GameEngine2D.Entities.Characters;

namespace GameEngine2D.States
{
    public class AttackingState : IPlayerState
    {
        public string Name => "Attacking";

        public void Handle(Player player)
        {
            player.ExecuteAttackAction();
            player.SetState(new IdleState(), false);
        }
    }
}