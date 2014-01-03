using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.Command.Interfaces;

namespace Compiler.ZestErrorHandler
{
    class ZestErrorHandling : ICommand
    {
        private readonly SourceCode sourceCode;

        public ZestErrorHandling(SourceCode sourceCode)
        {
            this.sourceCode = sourceCode;
        }

        public void Execute()
        {
            //if HasErrors then print out informative error details
            throw new NotImplementedException();
        }
    }
}
