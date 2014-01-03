using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.Command.Interfaces
{
    public interface ICommandExecutionEngine
    {
        void ExecuteCommands(IEnumerable<ICommand> compilerCommands);
    }
}
