using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;

namespace Compiler.CodeGenerator
{
    class CsharpTestGenerator : ICommand
    {
        private readonly ImmediateRepresentation immedieateRepresentation;
        public CsharpTestGenerator(ImmediateRepresentation immedieateRepresentation)
        {
            this.immedieateRepresentation = immedieateRepresentation;
        }

        public void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
