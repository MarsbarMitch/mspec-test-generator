using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace ScratchProject
{
	public class VisitorCompiler
	{
		private readonly Grammar grammar;

        public VisitorCompiler(Grammar grammar)
        {
            this.grammar = grammar;
        }
		
		public IAstNode GetAstRoot(string sourceCode)
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(sourceCode);
			if(parseTree.HasErrors())
			{
				Console.WriteLine(parseTree.ParserMessages.First().Message);
				Console.WriteLine("scream");
			}
			   
            return (IAstNode) parseTree.Root.AstNode;
        }
	}
}

