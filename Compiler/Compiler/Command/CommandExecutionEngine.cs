using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;

namespace Compiler.Command
{
    class CommandExecutionEngine : ICommandExecutionEngine
    {
        public void ExecuteCommands(IEnumerable<ICommand> compilerCommands)
        {
            foreach (var command in compilerCommands)
            {
                command.Execute();
            }
        }
    }
}
