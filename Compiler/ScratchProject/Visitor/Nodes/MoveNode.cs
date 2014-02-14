using System;

namespace ScratchProject
{
	public class MoveNode : IAstNode
	{
		public MoveNode ()
		{
		}
		public void Accept(IAstNodeVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}

