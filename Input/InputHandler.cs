using System;
using System.Collections.Generic;
using GameEngine2D.Commands;

namespace GameEngine2D.Input
{
    public class InputHandler
    {
        private readonly Dictionary<ConsoleKey, ICommand> commands = new();

        public void SetCommand(ConsoleKey key, ICommand command)
        {
            commands[key] = command;
        }

        public bool HandleInput(ConsoleKey key)
        {
            if (commands.TryGetValue(key, out ICommand? command))
            {
                command.Execute();
                return true;
            }

            return false;
        }
    }
}