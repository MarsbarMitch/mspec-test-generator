using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace Compiler.ZestParser
{
    class ZestParser
    {
        private readonly Grammar grammar;
        public ZestParser(Grammar grammar)
        {
            this.grammar = grammar;
            
        }
        //why not refactor this to return error list if there are errors?
        public bool isSourceCodeValid(string sourceCode)
        {
            return GetRoot(sourceCode) != null;
        }

        public ParseTreeNode GetRoot(string sourceCode)
        {
            Console.WriteLine(grammar.GrammarComments);
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(sourceCode);
            if (parseTree.HasErrors())
            {
                Console.WriteLine("{0} at line: {1}", parseTree.ParserMessages.First().Message, parseTree.ParserMessages.First().Location.Line.ToString());
            }
            return parseTree.Root;
        }
    }
}
