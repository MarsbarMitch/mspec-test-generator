using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony;

namespace Compiler.ZestParser
{
    class ParsedSourceCode
    {
        public bool HasErrors { get; private set; }
        public ParseTreeNode ParseTreeRoot { get; private set; }
        public LogMessageList ErrorList { get; private set; }

        public ParsedSourceCode(bool hasErrors, ParseTreeNode treeRoot, LogMessageList errorList)
        {
            HasErrors = hasErrors;
            ParseTreeRoot = treeRoot;
            ErrorList = errorList;
        }
    }
}
