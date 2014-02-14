using System;

namespace ScratchProject
{
	public class StartNode : IAstNode
	{
		public StartNode ()
		{
		}
		public void Accept(IAstNodeVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}

