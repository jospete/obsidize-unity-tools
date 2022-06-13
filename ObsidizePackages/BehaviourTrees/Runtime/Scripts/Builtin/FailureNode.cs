namespace Obsidize.BehaviourTrees
{

    public class FailureNode : ActionNode
    {

		protected override NodeState OnUpdate()
		{
			return NodeState.Failure;
		}
	}
}
