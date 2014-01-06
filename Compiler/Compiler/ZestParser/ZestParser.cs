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
        private readonly ImmediateRepresentation immediateRep;
        public ZestParser(Grammar grammar, ImmediateRepresentation immediateRep)
        {
            this.grammar = grammar;
            this.immediateRep = immediateRep;
        }
        
        public void Execute()
        {
            ParseSourceCode();
        }

        private void ParseSourceCode()
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(immediateRep.ZestSourceCode);
            immediateRep.HasErrors = parseTree.HasErrors();
            immediateRep.ParseTreeRoot = parseTree.Root;
            immediateRep.ErrorList = parseTree.ParserMessages;
        }
    }
}
