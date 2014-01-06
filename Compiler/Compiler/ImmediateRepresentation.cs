using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony;

namespace Compiler
{
    class ImmediateRepresentation
    {
        public bool HasErrors { get; set; }
        public ParseTreeNode ParseTreeRoot { get; set; }
        public LogMessageList ErrorList { get; set; }
        public string ZestSourceCode { get; private set; }

        public ImmediateRepresentation(string zestSourceCode)
        {
            ZestSourceCode = zestSourceCode;
        }
    }
}
