namespace Obsidize.BehaviourTrees.Samples
{
    public class PatrolAction : ActionNode<ActorAIState>
    {

		protected override NodeState OnUpdate()
		{
			return Blackboard.TryMoveToPatrolDestination()
				? NodeState.Success
				: NodeState.Running;
		}
    }
}
