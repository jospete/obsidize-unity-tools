namespace Obsidize.BehaviourTrees.Samples
{
	public class MoveTowardsThreat : ActionNode<ActorAIState>
	{
		protected override NodeState OnUpdate()
		{

			if (Blackboard.IsWithinReachOfThreat())
			{
				return NodeState.Success;
			}

			if (Blackboard.TryMoveTowardsThreat())
			{
				return NodeState.Running;
			}

			return NodeState.Failure;
		}
	}
}
