namespace Obsidize.BehaviourTrees
{

	public class SelectorNode : CompositeNode
	{

		protected sealed override NodeState TerminalChildState => NodeState.Success;
		protected sealed override NodeState AllChildrenProcessedState => NodeState.Failure;
	}
}
