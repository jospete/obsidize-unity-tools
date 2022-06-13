namespace Obsidize.BehaviourTrees.Samples
{
	public class MoveTowardsThreat : ActionNode<ActorAIState>
	{
		protected override NodeState OnUpdate()
		{

			if (ControllerState.IsWithinReachOfThreat())
			{
				return NodeState.Success;
			}

			if (ControllerState.TryMoveTowardsThreat())
			{
				return NodeState.Running;
			}

			return NodeState.Failure;
		}
	}
}
