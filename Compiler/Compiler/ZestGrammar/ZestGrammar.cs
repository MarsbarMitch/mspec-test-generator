using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace Compiler.ZestGrammar
{
    class ZestGrammar : Grammar
    {
        public ZestGrammar()
        {
            this.GrammarComments = "A grammar for Zest";
            //Terminals
            StringLiteral stringTerm = new StringLiteral("<String>", "\"");
            IdentifierTerminal identifier = new IdentifierTerminal("<Identifier>");
            RegexBasedTerminal number = new RegexBasedTerminal("number", "[0-9]+");

            //Non terminals
            NonTerminal program = new NonTerminal("<Program>");
            NonTerminal testSpecification = new NonTerminal("<TestSpecification>");
            NonTerminal definition = new NonTerminal("<Definition>");
            NonTerminal specificationDeclaration = new NonTerminal("<SpecificationDeclaration>");
            NonTerminal testDeclaration = new NonTerminal("<TestDeclaration>");
            NonTerminal end = new NonTerminal("<End>");
            NonTerminal testHeader = new NonTerminal("<TestHeader>");
            NonTerminal testBody = new NonTerminal("<TestBody>");
            NonTerminal becauseStatement = new NonTerminal("<BecauseStatement>");
            NonTerminal shouldStatement = new NonTerminal("<ShouldStatement>");
            NonTerminal identifierList = new NonTerminal("<IdentifierList>");
            NonTerminal returnExpr = new NonTerminal("<ReturnExpr>");
            NonTerminal numberList = new NonTerminal("<NumberList>");
            //bnf rules
            program.Rule = testSpecification;
            testSpecification.Rule = definition + ToTerm("sut") + specificationDeclaration + MakeStarRule(testDeclaration, testDeclaration) + end;//not sure the makeplus rule is correct
            definition.Rule = ToTerm("def");
            specificationDeclaration.Rule = identifier | identifier + ToTerm(":") + identifier;
            testDeclaration.Rule = definition + testHeader + MakePlusRule(testBody, testBody) + end;
            end.Rule = ToTerm("end");
            testHeader.Rule = ToTerm("when") + stringTerm;
            testBody.Rule = "because" + becauseStatement | "should" + shouldStatement;
            becauseStatement.Rule = identifier + ToTerm("(") + MakeStarRule(identifierList, ToTerm(","), identifier) + ")" | "csut" + "(" + MakeStarRule(identifierList, identifier) + ")" | identifier + ToTerm("(") + MakeStarRule(numberList, ToTerm(","), number) + ")";
            shouldStatement.Rule = ToTerm("return") + returnExpr | "throw" + identifier;
            identifierList.Rule = identifier | MakePlusRule(identifierList, ToTerm(","), identifier);
            returnExpr.Rule = number | stringTerm;
            this.Root = program;
            MarkPunctuation("def", "when", "sut", "(", ")", "csut", "because", "should", "throw", "return", "end", ":");
        }
        
    }
}
