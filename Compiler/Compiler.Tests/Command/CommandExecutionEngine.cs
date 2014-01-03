using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Machine.Specifications;
using Compiler.Command.Interfaces;
using Compiler.Tests.Command.Specifications;

namespace Compiler.Tests.Command
{
    [Subject(typeof(ICommandExecutionEngine))]
    class when_calling_ExecuteCommands_with_three_commands : CommandExecutionEngineSpecification
    {
        Establish context = () =>
        {
            CreateSut();
        };
        Because of = () => Sut.ExecuteCommands(DefaultCommandList.Select((c) =>c.Object));
        It should_execute_each_command = () =>
        {
            foreach (var mockCommand in DefaultCommandList)
            {
                mockCommand.Verify((c) => c.Execute(), Moq.Times.Once);
            }
        };
    }
}
