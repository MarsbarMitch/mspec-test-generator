using System;

namespace ScratchProject
{
	public interface IAstNodeVisitor
	{
		void Visit(StartNode startNode);
		void Visit(CreateNode createNode);
		void Visit(DirectionNode directionNode);
		void Visit(MoveNode moveNode);
		void Visit(MovesNode movesNode);
	}
}

