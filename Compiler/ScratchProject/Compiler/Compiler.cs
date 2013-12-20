using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace ScratchProject.Compiler
{
    class Compiler
    {
        private readonly Grammar grammar;

        public Compiler(Grammar grammar)
        {
            this.grammar = grammar;
        }

        public bool isValid(string sourceCode)
        {
            return GetRoot(sourceCode) != null;
        }

        public void DisplayTree(ParseTreeNode node, int level)
        {
            for (int i = 0; i < level; i++)
            {
                Console.Write("   ");
            }
            Console.WriteLine(node);
            foreach (var child in node.ChildNodes)
            {
                DisplayTree(child, level + 1);
            }

        }

        public void CompileCode(string sourceCode)
        {
            if (!isValid(sourceCode))
            {
                throw new Exception("Source not valid;");
            }
            ParseTreeNode root = GetRoot(sourceCode);
            
            ParseTreeNodeList nodeList = root.ChildNodes;
            //first expecting a createStatement
            ParseTreeNode create = nodeList.First();
            var x = GetNodeValue(create.ChildNodes[0]);
            var y = GetNodeValue(create.ChildNodes[1]);
            Console.WriteLine("Creating grid {0},{1}", x, y);
            int[,] grid = new int[int.Parse(x), int.Parse(y)];

            //now expecting a start statement
            ParseTreeNode start = nodeList[1];
            var xCurrent = int.Parse(GetNodeValue(start.ChildNodes[0]));
            var yCurrent = int.Parse(GetNodeValue(start.ChildNodes[1]));
            Console.WriteLine("Start position: {0},{1}", xCurrent, yCurrent);
            int moveCount = 1;
            grid[yCurrent, xCurrent] = moveCount;

            //now process the move statements
            foreach (var move in nodeList[2].ChildNodes)
            {
                moveCount++;
                int numPlaces = int.Parse(GetNodeValue(move.ChildNodes[1]));
                string direction = GetNodeValue(move.ChildNodes[0].ChildNodes[0]);
                
                
                for (int i = 0; i < numPlaces; i++)
                {
                    int offset = 0;
                    if (direction.Equals("up") || direction.Equals("left"))
                    {
                        offset--;
                    }
                    else
                    {
                        offset++;
                    }
                    if (direction.Equals("up") || direction.Equals("down"))
                    {
                        yCurrent += offset;
                    }
                    else
                    {
                        xCurrent += offset;
                    }
                    grid[yCurrent, xCurrent] = moveCount;
                }
            }
            OutPutResult(grid);
        }

        public ParseTreeNode GetRoot(string sourceCode)
        {
            LanguageData language = new LanguageData(grammar);
            Parser gridWorldParser = new Parser(language);
            ParseTree parseTree = gridWorldParser.Parse(sourceCode);
            return parseTree.Root;
        }

        private static string GetNodeValue(ParseTreeNode node)
        {
            return node.Token.Value.ToString();
        }

        private static void OutPutResult(int[,] grid)
        {
            
            for (int i = 0; i < grid.GetUpperBound(0); i++)
            {
                for (int j = 0; j < grid.GetUpperBound(1); j++)
                {
                    int value = grid[i, j];
                    Console.Write("  {0}  ",(value>0) ? value.ToString() : "-");
                }
                Console.WriteLine("");
                Console.WriteLine("");
            }
        }
    }
}
