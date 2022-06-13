namespace Obsidize.BehaviourTrees
{

	public class SuccessNode : ActionNode
	{

		protected override NodeState OnUpdate()
		{
			return NodeState.Success;
		}
	}
}
