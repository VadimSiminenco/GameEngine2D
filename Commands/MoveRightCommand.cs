using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Commands
{
    public class MoveRightCommand : ICommand
    {
        private readonly Player receiver;

        public MoveRightCommand(Player receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.MoveRight();
        }
    }
}