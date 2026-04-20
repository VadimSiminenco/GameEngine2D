using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Commands
{
    public class AttackCommand : ICommand
    {
        private readonly Player receiver;

        public AttackCommand(Player receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.Attack();
        }
    }
}