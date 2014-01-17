using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;
using Irony.Parsing;
using Compiler.ZestErrorHandler;
using Compiler.CodeGenerator;

namespace Compiler.Command
{
    class CommandFactory :ICommandFactory
    {
        private readonly ImmediateRepresentation immediateRep;
        private readonly Grammar zestGrammar;
        public CommandFactory(ImmediateRepresentation immediateRep, Grammar zestGrammar)
        {
            this.immediateRep = immediateRep;
            this.zestGrammar = zestGrammar;
        }

        public IEnumerable<ICommand> CreateCompilerCommands()
        {
            var compilerCommands = new List<ICommand>();
            compilerCommands.Add(new ZestParser.ZestParser(zestGrammar, immediateRep));
            compilerCommands.Add(new ZestSyntaxHandling(immediateRep));
            compilerCommands.Add(new CsharpTestGenerator(immediateRep));
            return compilerCommands;
        }
    }
}
