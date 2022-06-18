namespace Obsidize.BehaviourTrees
{

	public class SequenceNode : CompositeNode
	{

		protected sealed override NodeState TerminalChildState => NodeState.Failure;
		protected sealed override NodeState AllChildrenProcessedState => NodeState.Success;
	}
}
