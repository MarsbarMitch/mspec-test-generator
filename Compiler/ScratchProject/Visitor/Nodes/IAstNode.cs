using System;

namespace ScratchProject
{
	public interface IAstNode
	{
		void Accept(IAstNodeVisitor visitor);
	}
}

