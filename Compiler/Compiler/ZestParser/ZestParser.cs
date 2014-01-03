using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
using Irony;

namespace Compiler.ZestParser
{
    class ZestParser
    {
        private readonly Grammar grammar;
        public ZestParser(Grammar grammar)
        {
            this.grammar = grammar;
        }
        
        public ParsedSourceCode Parse(string sourceCode)
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(sourceCode);
            return new ParsedSourceCode(parseTree.HasErrors(), parseTree.Root, parseTree.ParserMessages);
        }        
    }
}
