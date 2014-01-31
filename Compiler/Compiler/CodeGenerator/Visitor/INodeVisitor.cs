using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Compiler.CodeGenerator.Visitor.Nodes;

namespace Compiler.CodeGenerator.Visitor
{
    interface INodeVisitor
    {
        void Visit(WhenNode whenNode);
        void Visit(BecauseNode becauseNode);
        void Visit(ShouldNode shouldNode);
        void Visit(EndNode endNode);
        
    }
}
