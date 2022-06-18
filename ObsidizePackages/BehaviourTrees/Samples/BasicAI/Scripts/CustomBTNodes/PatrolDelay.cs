namespace Obsidize.BehaviourTrees.Samples
{
	public class PatrolDelay : ActionNode<ActorAIState>
	{

		protected override NodeState OnUpdate()
		{
			return Blackboard.TryWaitForPatrolDelay()
				? NodeState.Success
				: NodeState.Running;
		}
	}
}
