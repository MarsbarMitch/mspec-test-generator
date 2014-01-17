using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Tests.Command.Specifications;
using Compiler.Command.Interfaces;
using Machine.Specifications;
using c = Compiler.ZestParser;
using Compiler.ZestErrorHandler;
using Compiler.CodeGenerator;
namespace Compiler.Tests.Command
{
    [Subject(typeof(ICommandFactory))]
    class when_calling_CreateCompilerCommands : CommandFactorySpecification
    {
        private static IEnumerable<ICommand> result;
        Establish context = () => CreatesSut();
        Because of = () => result = Sut.CreateCompilerCommands();
        It should_create_three_commands = () => result.Count().ShouldEqual(3);
        It should_create_a_parse_command = () => result.ElementAt(0).ShouldBeOfType<c.ZestParser>();
        It should_create_an_ErrorHandling_command = () => result.ElementAt(1).ShouldBeOfType<ZestSyntaxHandling>();
        It should_create_a_CodeGenerator_command = () => result.ElementAt(2).ShouldBeOfType<CsharpTestGenerator>();
    }
}
