using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Commands
{
    public class JumpCommand : ICommand
    {
        private readonly Player receiver;

        public JumpCommand(Player receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.Jump();
        }
    }
}