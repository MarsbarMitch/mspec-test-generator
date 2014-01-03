using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony;
using Compiler.Command.Interfaces;

namespace Compiler.ZestParser
{
    class ZestParser : ICommand
    {
        private readonly Grammar grammar;
        private readonly SourceCode sourceCode;
        public ZestParser(Grammar grammar, SourceCode sourceCode)
        {
            this.grammar = grammar;
            this.sourceCode = sourceCode;
        }
        
        public void Execute()
        {
            ParseSourceCode();
        }

        private void ParseSourceCode()
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(sourceCode.ZestSourceCode);
            sourceCode.HasErrors = parseTree.HasErrors();
            sourceCode.ParseTreeRoot = parseTree.Root;
            sourceCode.ErrorList = parseTree.ParserMessages;
        }
    }
}
