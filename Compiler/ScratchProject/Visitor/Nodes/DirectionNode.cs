using System;

namespace ScratchProject
{
	public class DirectionNode : IAstNode
	{
		public DirectionNode ()
		{
		}
		
		public void Accept(IAstNodeVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}

