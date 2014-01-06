using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;

namespace Compiler.CodeGenerator
{
    class CsharpTestGenerator : ICommand
    {
        private readonly ImmediateRepresentation sourceCode;
        public CsharpTestGenerator(ImmediateRepresentation sourceCode)
        {
            this.sourceCode = sourceCode;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
