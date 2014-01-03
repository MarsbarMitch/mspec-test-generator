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
        private readonly SourceCode sourceCode;
        private readonly Grammar zestGrammar;
        public CommandFactory(SourceCode sourceCode, Grammar zestGrammar)
        {
            this.sourceCode = sourceCode;
            this.zestGrammar = zestGrammar;
        }

        public IEnumerable<ICommand> CreateCompilerCommands()
        {
            var compilerCommands = new List<ICommand>();
            compilerCommands.Add(new ZestParser.ZestParser(zestGrammar, sourceCode));
            compilerCommands.Add(new ZestErrorHandling(sourceCode));
            compilerCommands.Add(new CsharpTestGenerator(sourceCode));
            return compilerCommands;
        }
    }
}
