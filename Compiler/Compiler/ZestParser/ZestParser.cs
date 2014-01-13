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
        private readonly ImmediateRepresentation immediateRepresentation;
        public ZestParser(Grammar grammar, ImmediateRepresentation immediateRepresentation)
        {
            this.grammar = grammar;
            this.immediateRepresentation = immediateRepresentation;
        }
        
        public void Execute()
        {
            ParseSourceCode();
        }

        private void ParseSourceCode()
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(immediateRepresentation.ZestSourceCode);
            immediateRepresentation.HasErrors = parseTree.HasErrors();
            immediateRepresentation.ParseTreeRoot = parseTree.Root;
            immediateRepresentation.ErrorList = parseTree.ParserMessages;
        }
    }
}
