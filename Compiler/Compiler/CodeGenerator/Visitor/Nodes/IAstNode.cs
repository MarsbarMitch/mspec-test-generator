using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Compiler.CodeGenerator.Visitor.Nodes
{
    interface IAstNode
    {
        void Accept(INodeVisitor visitor);
    }
}
