using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;

namespace Compiler.ZestErrorHandler
{
    class ZestSyntaxHandling : ICommand
    {
        private readonly ImmediateRepresentation immediateRepresentation;

        public ZestSyntaxHandling(ImmediateRepresentation immediateRepresentation)
        {
            this.immediateRepresentation = immediateRepresentation;
        }

        public void Execute()
        {
            if (immediateRepresentation.HasErrors)
            {
                Console.WriteLine("Failed to compile, {0} syntax errors were found:",immediateRepresentation.ErrorList.Count);
                foreach (var error in immediateRepresentation.ErrorList)
                {
                    Console.WriteLine("  --> {0} at line {1}", error.Message, error.Location);
                }
            }
        }
    }
}
