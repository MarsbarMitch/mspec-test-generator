using System;

namespace ScratchProject
{
	public class MovesNode : IAstNode
	{
		public MovesNode ()
		{
		}
		
		public void Accept(IAstNodeVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}

