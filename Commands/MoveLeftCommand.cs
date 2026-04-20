using GameEngine2D.Entities.Characters;

namespace GameEngine2D.Commands
{
    public class MoveLeftCommand : ICommand
    {
        private readonly Player receiver;

        public MoveLeftCommand(Player receiver)
        {
            this.receiver = receiver;
        }

        public void Execute()
        {
            receiver.MoveLeft();
        }
    }
}