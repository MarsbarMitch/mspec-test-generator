using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;
/// Code in this class is based on http://en.wikibooks.org/wiki/Irony_-_Language_Implementation_Kit/Introduction

namespace ScratchProject.GridWorldDemo
{
    class GridWorldGrammar : Grammar
    {
        public GridWorldGrammar()
        {
            this.GrammarComments = "A grammar for GridWorld";

            //Terminals
            RegexBasedTerminal number = new RegexBasedTerminal("number", "[0-9]+");

            //Non terminals
            NonTerminal program = new NonTerminal("program");
            NonTerminal createStatement = new NonTerminal("createStatement", typeof(StartNode));
            NonTerminal startStatement = new NonTerminal("startStatement", typeof(StartNode));
            NonTerminal moveStatements = new NonTerminal("moveStatements", typeof(MovesNode));
            NonTerminal moveStatement = new NonTerminal("moveStatement", typeof(MoveNode));
            NonTerminal direction = new NonTerminal("direction", typeof(DirectionNode));

            //BNF rules
            program.Rule = createStatement + startStatement + moveStatements;
            createStatement.Rule = ToTerm("Create") + "a" + number + "by" + number + "grid" + ".";
            startStatement.Rule = ToTerm("Start") + "at" + "location" + number + "," + number + ".";
            moveStatements.Rule = MakePlusRule(moveStatements, moveStatement);
            moveStatement.Rule = "Move" + direction + number + ".";
            direction.Rule = ToTerm("up") | "down" | "right" | "left";
            this.Root = program;
			this.LanguageFlags = LanguageFlags.CreateAst;
            MarkPunctuation("Create", "a", "grid", "by", "Start", "at","location", ",", ".", "Move");
        }
		


    }
}
