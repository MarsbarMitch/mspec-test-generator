using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;
using Machine.Specifications;
using Compiler.Command;

namespace Compiler.Tests.Command.Specifications
{
    abstract class CommandExecutionEngineSpecification : Specification
    {
        protected static ICommandExecutionEngine Sut;
        protected static IEnumerable<Moq.Mock<ICommand>> DefaultCommandList;
        Establish context = () => 
        {
            var commandList = new List<Moq.Mock<ICommand>>();
            for (int i = 0; i < 3; i++)
            {
                var mockCommand = new Moq.Mock<ICommand>();
                mockCommand.Setup((c) => c.Execute());
                commandList.Add(mockCommand);
            }
            DefaultCommandList = commandList;
        };

        protected static void CreateSut()
        {
            Sut = new CommandExecutionEngine();
        }
    }
}
