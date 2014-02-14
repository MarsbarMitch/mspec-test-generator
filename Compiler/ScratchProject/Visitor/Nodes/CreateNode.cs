using System;

namespace ScratchProject
{
	public class CreateNode : IAstNode
	{
		public CreateNode ()
		{
		}
		
		public void Accept(IAstNodeVisitor visitor)
		{
			visitor.Visit(this);
		}
	}
}

