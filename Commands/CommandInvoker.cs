namespace GameEngine2D.Commands
{
    public class CommandInvoker
    {
        private ICommand? command;

        public void SetCommand(ICommand command)
        {
            this.command = command;
        }

        public void ExecuteCommand()
        {
            command?.Execute();
        }
    }
}